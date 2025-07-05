using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IModerationFilter
    {
        public Task<(bool isBlocked, string? message, Agent? agent)> CheckAsync(string intent, Guid agentId);
    }
}
