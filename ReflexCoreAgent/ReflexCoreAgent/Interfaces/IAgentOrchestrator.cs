using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces
{
    public interface IAgentOrchestrator
    {
        Task<string> HandleMessageAsync(LineWebhookPayload evt, Guid agentId);
    }
}
