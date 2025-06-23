using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface IModerationRuleRepository
    {
        Task<List<ModerationRule>> GetActiveRulesAsync();
        Task<List<ModerationRule>> GetByAgentIdAsync(Guid agentId);
        Task AddAsync(ModerationRule rule);
        Task UpdateAsync(ModerationRule rule);
        Task DeleteAsync(ModerationRule rule);
    }
}
