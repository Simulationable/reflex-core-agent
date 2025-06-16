using ReflexCoreAgent.Interfaces;
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
        public async Task ReplyAsync(string replyToken, string message)
        {
            _logger.LogInformation("Reply To: {ReplyToken}, With Message: {Message}", replyToken, message);
            var payload = new
            {
                replyToken = replyToken,
                messages = new[]
                {
                    new { type = "text", text = message }
                }
            };

            _logger.LogInformation("Payload : {Payload}", payload);
            using var client = new HttpClient();
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", "/yewuzL7Vuv5uorTZW9C3Vka34kLQtOqCOuSCqBqU1lnjsbQVL2pn5QfMEQLq287tX+BFtl6JU0r2MHcneBf7omQ+EKhqJDCKeAP2lxc5U/cE/9ualLPaobIE1BDrEeUmWggtpvquVlB3PQP1X2JlwdB04t89/1O/w1cDnyilFU=");

            var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");
            var response = await client.PostAsync("https://api.line.me/v2/bot/message/reply", content);
            if (!response.IsSuccessStatusCode)
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Failed to reply to LINE message. Status: {StatusCode}, Error: {ErrorContent}", response.StatusCode, errorContent);
                throw new HttpRequestException($"LINE API returned {response.StatusCode}: {errorContent}");
            }
        }
    }
}
