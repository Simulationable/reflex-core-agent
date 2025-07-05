using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces.Services;
using System.Text.Json;

namespace ReflexCoreAgent.Applications
{
    public class AgentOrchestrator : IAgentOrchestrator
    {
        private readonly ITranslator _translator;
        private readonly ILineMessenger _lineMessenger;
        private readonly ICalendarService _calendarService;
        private readonly IPdfService _pdfService;
        private readonly INotificationService _notificationService;
        private readonly IKnowledgeService _knowledgeService;
        private readonly IInteractionService _interactionService;
        private readonly ILogger<AgentOrchestrator> _logger;

        public AgentOrchestrator(ITranslator translator,
            ILineMessenger lineMessenger,
            ICalendarService calendarService,
            IPdfService pdfService,
            INotificationService notificationService,
            IKnowledgeService knowledgeService,
            IInteractionService interactionService,
            ILogger<AgentOrchestrator> logger)
        {
            _translator = translator;
            _lineMessenger = lineMessenger;
            _calendarService = calendarService;
            _pdfService = pdfService;
            _notificationService = notificationService;
            _knowledgeService = knowledgeService;
            _interactionService = interactionService;
            _logger = logger;
        }

        public async Task<string> HandleMessageAsync(LineWebhookPayload payload, Guid agentId)
        {
            var evt = payload.Events.FirstOrDefault();
            if (evt == null || evt.Type != "message" || evt.Message?.Type != "text")
            {
                _logger.LogWarning("Unsupported or missing event.");
                return string.Empty;
            }

            var userMessage = evt.Message.Text?.Trim();
            var userId = evt.Source?.UserId ?? "anonymous";

            if (string.IsNullOrEmpty(userMessage)) return string.Empty;
            _logger.LogInformation("User Message: {UserMessage}", userMessage);

            var recentHistory = await _interactionService.GetHistoryAsync(userId);
            var context = string.Join("\n", recentHistory
                .OrderByDescending(i => i.Timestamp)
                .Take(5)
                .Select(i => $"👤: {i.InputTh}\n🤖: {i.ResponseTh}"));

            string knowledge = await _knowledgeService.SearchAnswerAsync(userMessage, agentId);

            _logger.LogInformation("Knowledge: {Knowledge}", knowledge);

            string responseTh;

            if (userMessage.Contains("นัดหมาย") || userMessage.Contains("นัด"))
            {
                if (await _calendarService.TryAddAppointmentAsync(userMessage, agentId))
                {
                    responseTh = "✅ เพิ่มนัดหมายสำเร็จใน Calendar";
                }
                else responseTh = "❌ ไม่สามารถเพิ่มนัดหมายได้";
            }
            else if (userMessage.Contains("ใบเสนอราคา"))
            {
                var pdfUrl = await _pdfService.GenerateQuotationAsync(userMessage, agentId);
                responseTh = string.IsNullOrWhiteSpace(pdfUrl)
                    ? "❌ ไม่สามารถสร้างใบเสนอราคาได้"
                    : $"📄 สร้างใบเสนอราคาเรียบร้อย: {pdfUrl}";
            }
            else if (userMessage.Contains("ราคาถูก"))
            {
                await _notificationService.AlertSalesTeamAsync(userMessage, agentId);
                responseTh = "📣 แจ้งทีมขายเรียบร้อยแล้ว";
            }
            else
            {
                responseTh = await _translator.Answer(userMessage, knowledge, agentId, context);
            }

            await _interactionService.SaveInteractionAsync(userId, userMessage, responseTh);

            await _lineMessenger.ReplyAsync(evt.ReplyToken, responseTh);
            return responseTh;
        }
    }
}
