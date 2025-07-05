using PdfSharpCore.Drawing;
using System.Text.RegularExpressions;
using ReflexCoreAgent.Domain.Model;
using PdfSharpCore.Pdf;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Applications
{
    public class PdfService : IPdfService
    {
        private readonly IWebHostEnvironment _env;
        private readonly ICompanyProfileService _companyProfileService;
        private readonly ILogger<PdfService> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public PdfService(IWebHostEnvironment env,
            ICompanyProfileService companyProfileService,
            ILogger<PdfService> logger, 
            IHttpContextAccessor httpContextAccessor)
        {
            _env = env;
            _companyProfileService = companyProfileService;
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GenerateQuotationAsync(string userInput, Guid agentId)
        {
            var fileName = $"{Guid.NewGuid():N}.pdf";
            var outputDir = Path.Combine(_env.WebRootPath ?? "wwwroot", "pdfs");
            Directory.CreateDirectory(outputDir);
            var fullPath = Path.Combine(outputDir, fileName);

            var company = await _companyProfileService.GetAsync();
            if (company == null)
                throw new InvalidOperationException("ไม่พบข้อมูลบริษัท");

            using var doc = new PdfDocument();
            var page = doc.AddPage();
            var gfx = XGraphics.FromPdfPage(page);

            var fontTitle = new XFont("Sarabun", 20, XFontStyle.Bold);
            var fontHeader = new XFont("Sarabun", 12, XFontStyle.Bold);
            var fontText = new XFont("Sarabun", 12, XFontStyle.Regular);

            var darkGray = new XSolidBrush(XColors.DarkSlateGray);
            var lightGray = new XSolidBrush(XColors.LightGray);
            var black = new XSolidBrush(XColors.Black);
            var pen = new XPen(XColors.Black, 0.8);

            // วาดโลโก้ถ้ามี
            if (!string.IsNullOrWhiteSpace(company.LogoBase64))
            {
                var logoBytes = Convert.FromBase64String(company.LogoBase64);
                using var logoStream = new MemoryStream(logoBytes);
                var logoImage = XImage.FromStream(() => logoStream);
                gfx.DrawImage(logoImage, 30, 30, 80, 80);
            }

            gfx.DrawString(company.CompanyName ?? "ชื่อบริษัท", fontText, black, new XRect(120, 30, 300, 20), XStringFormats.TopLeft);
            gfx.DrawString(company.Address ?? "ที่อยู่", fontText, black, new XRect(120, 48, 400, 20), XStringFormats.TopLeft);
            gfx.DrawString($"โทร: {company.Phone ?? "-"}", fontText, black, new XRect(120, 66, 400, 20), XStringFormats.TopLeft);

            gfx.DrawString("ใบเสนอราคา", fontTitle, darkGray, new XRect(0, 100, page.Width, 30), XStringFormats.TopCenter);

            gfx.DrawString($"ลูกค้า: ไม่ระบุชื่อ", fontText, black, new XRect(40, 140, page.Width - 80, 20), XStringFormats.TopLeft);
            gfx.DrawString($"วันที่: {DateTime.Now:dd MMMM yyyy}", fontText, black, new XRect(40, 160, page.Width - 80, 20), XStringFormats.TopLeft);

            double y = 200;
            double[] colWidths = { 220, 60, 100, 100 };
            string[] headers = { "รายการ", "จำนวน", "ราคา/หน่วย", "รวม" };
            double tableLeft = 40;
            double rowHeight = 24;

            for (int i = 0; i < headers.Length; i++)
            {
                double x = tableLeft + colWidths.Take(i).Sum();
                gfx.DrawRectangle(pen, lightGray, x, y, colWidths[i], rowHeight);
                gfx.DrawString(headers[i], fontHeader, black, new XRect(x + 4, y + 5, colWidths[i], rowHeight), XStringFormats.TopLeft);
            }

            y += rowHeight;
            var items = ParseQuotationItems(userInput);
            foreach (var item in items)
            {
                var total = item.Quantity * item.UnitPrice;
                string[] values = {
            item.Name,
            item.Quantity.ToString(),
            item.UnitPrice.ToString("N2"),
            total.ToString("N2")
        };

                for (int i = 0; i < values.Length; i++)
                {
                    double x = tableLeft + colWidths.Take(i).Sum();
                    gfx.DrawRectangle(pen, x, y, colWidths[i], rowHeight);
                    gfx.DrawString(values[i], fontText, black, new XRect(x + 4, y + 5, colWidths[i], rowHeight), XStringFormats.TopLeft);
                }

                y += rowHeight;
            }

            decimal grandTotal = items.Sum(i => i.Quantity * i.UnitPrice);
            gfx.DrawString("รวมเป็นเงินสุทธิ", fontHeader, black,
                new XRect(tableLeft + colWidths[0] + colWidths[1], y + 8, colWidths[2], rowHeight), XStringFormats.TopLeft);

            gfx.DrawString($"{grandTotal:N2} บาท", fontHeader, black,
                new XRect(tableLeft + colWidths[0] + colWidths[1] + colWidths[2] - 4, y + 8, colWidths[3], rowHeight), XStringFormats.TopRight);

            doc.Save(fullPath);

            var baseUrl = $"{GetServerUrl()}/pdfs/{fileName}";
            _logger.LogInformation("PDF สร้างแล้ว: {Url}", baseUrl);
            return baseUrl;
        }

        private string GetServerUrl()
        {
            var request = _httpContextAccessor.HttpContext?.Request;
            if (request == null)
                return "http://localhost"; // fallback เผื่อใช้ใน CLI/test

            var host = request.Host.HasValue ? request.Host.Value : "localhost";
            var scheme = request.Scheme;

            return $"{scheme}://{host}";
        }

        private List<QuotationItem> ParseQuotationItems(string text)
        {
            var items = new List<QuotationItem>();
            var regex = new Regex(@"(?<name>\w+)\s(?<qty>\d+)", RegexOptions.IgnoreCase);

            foreach (Match match in regex.Matches(text))
            {
                items.Add(new QuotationItem
                {
                    Name = match.Groups["name"].Value,
                    Quantity = int.Parse(match.Groups["qty"].Value),
                    UnitPrice = GetMockPrice(match.Groups["name"].Value)
                });
            }

            if (!items.Any())
            {
                items.Add(new QuotationItem
                {
                    Name = "รายการทั่วไป",
                    Quantity = 1,
                    UnitPrice = 100
                });
            }

            return items;
        }

        private decimal GetMockPrice(string name)
        {
            return name switch
            {
                "ปากกา" => 20,
                "สมุด" => 50,
                "ดินสอ" => 10,
                _ => 100
            };
        }

    }

}
