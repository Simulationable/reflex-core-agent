using ReflexCoreAgent.Domain.Model;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;

namespace ReflexCoreAgent.Helpers
{
    public static class LlamaCppHelper
    {
        public static async Task<string> RunAsync(string model, string prompt, string taskType = "default")
        {
            string port = model switch
            {
                "thaigpt" => "8001",
                _ => throw new ArgumentException("Unknown model: " + model)
            };

            var config = GetConfigForTask(prompt, taskType);
            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };

            using var http = new HttpClient();
            http.Timeout = TimeSpan.FromMinutes(2);
            var content = new StringContent(JsonSerializer.Serialize(config, options), Encoding.UTF8, "application/json");

            var response = await http.PostAsync($"http://localhost:{port}/completion", content);

            var raw = await response.Content.ReadAsStringAsync();

            if (!response.IsSuccessStatusCode)
            {
                Console.Error.WriteLine($"[LLM ERROR] Status: {response.StatusCode} | Response: {raw}");
                throw new HttpRequestException($"LLM returned {response.StatusCode}: {raw}");
            }

            return raw;
        }


        private static LlamaRequestConfig GetConfigForTask(string prompt, string taskType)
        {
            return taskType switch
            {
                "translation" => new LlamaRequestConfig
                {
                    Prompt = prompt,
                    N_Predict = 64,
                    Temperature = 0.7,
                    TopP = 0.9,
                    TopK = 40,
                    Stop = new[] { "\n\n", "\n ", "\n#", "คำถาม", "สอบถาม", "ทีมงาน" }
                },
                _ => new LlamaRequestConfig
                {
                    Prompt = prompt
                }
            };
        }
    }
}