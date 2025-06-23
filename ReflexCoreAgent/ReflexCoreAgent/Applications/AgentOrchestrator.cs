using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;
using System.Text.Json;

namespace ReflexCoreAgent.Applications
{
    public class AgentOrchestrator : IAgentOrchestrator
    {
        private readonly ITranslator _translator;
        private readonly ILineMessenger _lineMessenger;
        private readonly IKnowledgeService _knowledgeService;
        private readonly ILogger<AgentOrchestrator> _logger;

        public AgentOrchestrator(ITranslator translator,
            ILineMessenger lineMessenger,
            IKnowledgeService knowledgeService,
            ILogger<AgentOrchestrator> logger)
        {
            _translator = translator;
            _lineMessenger = lineMessenger;
            _knowledgeService = knowledgeService;
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
            var userMessage = evt.Message.Text;
            _logger.LogInformation("User Message: {UserMessage}", userMessage);
            string knowledge = await _knowledgeService.SearchAnswerAsync(userMessage, agentId);
            _logger.LogInformation("Knowledge: {Knowledge}", knowledge);
            var responseTh = await _translator.Answer(userMessage, knowledge, agentId);
            _logger.LogInformation("Translated response (TH): {ResponseTh}", responseTh);
            await _lineMessenger.ReplyAsync(evt.ReplyToken, responseTh);
            return responseTh;
        }
    }
}
