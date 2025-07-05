using ReflexCoreAgent.Interfaces.Services;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ReflexCoreAgent.Applications
{
    public class LineMessenger : ILineMessenger
    {
        private readonly ILogger<LineMessenger> _logger;
        public LineMessenger(ILogger<LineMessenger> logger)
        {
            _logger = logger;
        }

        public async Task ReplyAsync(string replyToken, string? message)
        {
            message = string.IsNullOrWhiteSpace(message)
                ? "ขออภัย ระบบไม่สามารถประมวลผลคำตอบได้ในขณะนี้"
                : message;

            _logger.LogInformation("Reply To: {ReplyToken}, With Message: {Message}", replyToken, message);

            var payload = new
            {
                replyToken = replyToken,
                messages = new[]
                {
                    new { type = "text", text = message }
                }
            };

            var payloadJson = JsonSerializer.Serialize(payload);
            _logger.LogInformation("Payload JSON: {Payload}", payloadJson);

            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "/yewuzL7Vuv5uorTZW9C3Vka34kLQtOqCOuSCqBqU1lnjsbQVL2pn5QfMEQLq287tX+BFtl6JU0r2MHcneBf7omQ+EKhqJDCKeAP2lxc5U/cE/9ualLPaobIE1BDrEeUmWggtpvquVlB3PQP1X2JlwdB04t89/1O/w1cDnyilFU=");

            var content = new StringContent(payloadJson, Encoding.UTF8, "application/json");

            try
            {
                var response = await client.PostAsync("https://api.line.me/v2/bot/message/reply", content);
                var responseText = await response.Content.ReadAsStringAsync();

                if (!response.IsSuccessStatusCode)
                {
                    _logger.LogError("❌ LINE Reply Failed | Status: {Status} | Response: {Response}",
                        response.StatusCode, responseText);
                }
                else
                {
                    _logger.LogInformation("✅ LINE Reply Success");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "❌ Exception ขณะตอบกลับ LINE");
            }
        }
    }
}
