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
                Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
            };

            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromMinutes(2);
            var content = new StringContent(JsonSerializer.Serialize(config, options), Encoding.UTF8, "application/json");

            Console.WriteLine($"[LLM DEBUG] Config : '{config}'");

            var response = await http.PostAsync($"http://localhost:{port}/completion", content);

            var raw = await response.Content.ReadAsStringAsync();

            Console.WriteLine($"[LLM DEBUG] Raw Response: '{raw}'");

            if (!response.IsSuccessStatusCode)
            {
                Console.Error.WriteLine($"[LLM ERROR] Status: {response.StatusCode} | Response: {raw}");
                throw new HttpRequestException($"LLM returned {response.StatusCode}: {raw}");
            }

            return raw;
        }
    }
}