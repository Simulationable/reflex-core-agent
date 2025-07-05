using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Applications
{
    public class NotificationService : INotificationService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<NotificationService> _logger;
        private readonly IConfiguration _config;

        public NotificationService(IHttpClientFactory httpClientFactory, ILogger<NotificationService> logger, IConfiguration config)
        {
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _config = config;
        }

        public async Task AlertSalesTeamAsync(string userInput, Guid agentId)
        {
            var token = _config["LineNotify:SalesTeamToken"];
            var message = $"📣 ข้อความจากลูกค้า: \"{userInput}\"\nAgent: {agentId}";

            var client = _httpClientFactory.CreateClient();
            client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var content = new FormUrlEncodedContent(new Dictionary<string, string>
            {
                { "message", message }
            });

            var res = await client.PostAsync("https://notify-api.line.me/api/notify", content);
            if (!res.IsSuccessStatusCode)
            {
                _logger.LogError("แจ้งเตือนทีมขายล้มเหลว: {StatusCode}", res.StatusCode);
            }
        }
    }
}
