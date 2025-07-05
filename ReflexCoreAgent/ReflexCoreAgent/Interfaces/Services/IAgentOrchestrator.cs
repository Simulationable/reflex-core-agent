using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IAgentOrchestrator
    {
        Task<string> HandleMessageAsync(LineWebhookPayload evt, Guid agentId);
    }
}
