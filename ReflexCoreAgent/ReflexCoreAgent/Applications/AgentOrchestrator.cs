using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;
using System.Text.Json;

namespace ReflexCoreAgent.Applications
{
    public class AgentOrchestrator : IAgentOrchestrator
    {
        private readonly ITranslator _translator;
        private readonly ILineMessenger _lineMessenger;
        private readonly ILogger<AgentOrchestrator> _logger;

        public AgentOrchestrator(ITranslator translator,
            ILineMessenger lineMessenger,
            ILogger<AgentOrchestrator> logger)
        {
            _translator = translator;
            _lineMessenger = lineMessenger;
            _logger = logger;
        }

        private readonly Dictionary<string, string> _faq = new()
        {
            { "ระยะเวลารับประกัน", "สินค้าของเรารับประกัน 1 ปีเต็มนับจากวันซื้อ" },
            { "วิธีใช้งาน", "สามารถศึกษาวิธีใช้งานได้จากคู่มือในกล่องสินค้า หรือสอบถามเพิ่มเติมได้ที่ Line Official" },
        };

        private Task<string> SearchAsync(string question)
        {
            foreach (var kv in _faq)
                if (question.Contains(kv.Key)) return Task.FromResult(kv.Value);

            return Task.FromResult(string.Empty);
        }

        public async Task<string> HandleMessageAsync(LineWebhookPayload payload)
        {
            var evt = payload.Events.FirstOrDefault();
            if (evt == null || evt.Type != "message" || evt.Message?.Type != "text")
            {
                _logger.LogWarning("Unsupported or missing event.");
                return string.Empty;
            }
            var userMessage = evt.Message.Text;
            _logger.LogInformation("User Message: {UserMessage}", userMessage);
            var knowledge = await SearchAsync(userMessage);
            _logger.LogInformation("Knowledge: {Knowledge}", knowledge);
            var responseTh = await _translator.Answer(userMessage, knowledge);
            _logger.LogInformation("Translated response (TH): {ResponseTh}", responseTh);
            await _lineMessenger.ReplyAsync(evt.ReplyToken, responseTh);
            return responseTh;
        }
    }
}
