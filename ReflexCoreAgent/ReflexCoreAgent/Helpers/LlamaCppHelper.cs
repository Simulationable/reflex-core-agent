using ReflexCoreAgent.Domain.Entities;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;

namespace ReflexCoreAgent.Helpers
{
    public static class LlamaCppHelper
    {
        public static async Task<string> RunAsync(string model, string prompt, LlamaRequestConfig config)
        {
            string port = model switch
            {
                "thaigpt" => "8001",
                _ => throw new ArgumentException("Unknown model: " + model)
            };

            config.Prompt = prompt;

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                WriteIndented = true
            };

            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromMinutes(2);

            var json = JsonSerializer.Serialize(config, options);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            Console.WriteLine($"[LLM DEBUG] Port: {port}");
            Console.WriteLine($"[LLM DEBUG] Request JSON:\n{json}");

            HttpResponseMessage response;
            string raw = string.Empty;

            try
            {
                response = await http.PostAsync($"http://localhost:{port}/completion", content);
                raw = await response.Content.ReadAsStringAsync();

                Console.WriteLine($"[LLM DEBUG] Raw Response: '{raw}'");

                if (!response.IsSuccessStatusCode)
                {
                    Console.Error.WriteLine($"[LLM ERROR] Status: {response.StatusCode} | Response: {raw}");
                    return JsonSerializer.Serialize(new
                    {
                        content = $"[SYSTEM ERROR] LLM ตอบกลับด้วย status code {response.StatusCode}"
                    });
                }

                return raw;
            }
            catch (HttpRequestException ex)
            {
                Console.Error.WriteLine($"[LLM ERROR] Connection refused to port {port}: {ex.Message}");
                return JsonSerializer.Serialize(new
                {
                    content = $"[SYSTEM ERROR] ไม่สามารถเชื่อมต่อ LLM ได้ที่พอร์ต {port} กรุณาตรวจสอบการเปิดใช้งาน"
                });
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"[LLM ERROR] Unexpected error: {ex.Message}");
                if (!string.IsNullOrWhiteSpace(raw))
                    Console.Error.WriteLine($"[LLM ERROR] Raw content before crash: {raw}");

                return JsonSerializer.Serialize(new
                {
                    content = "[SYSTEM ERROR] เกิดข้อผิดพลาดภายในระบบประมวลผลคำตอบ"
                });
            }
        }

    }
}