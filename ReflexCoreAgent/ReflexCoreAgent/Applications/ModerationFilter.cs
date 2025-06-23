using ReflexCoreAgent.Interfaces.Repositories;
using ReflexCoreAgent.Interfaces;
using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Applications
{
    public class ModerationFilter : IModerationFilter
    {
        private readonly IUnitOfWork _uow;

        public ModerationFilter(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<(bool isBlocked, string? message, Agent? agent)> CheckAsync(string intent, Guid agentId)
        {
            var agent = await _uow.Agents.GetByIdAsync(agentId);
            if (agent == null || !agent.EnableModeration) return (false, null, agent);

            foreach (var rule in agent.ModerationRules.Where(r => r.IsActive))
            {
                if (intent.Contains(rule.Keyword, StringComparison.OrdinalIgnoreCase))
                {
                    return (true, rule.ResponseMessage, agent);
                }
            }

            return (false, null, agent);
        }
    }
}
