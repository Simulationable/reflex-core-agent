using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IAgentService
    {
        Task<List<Agent>> GetActiveAllAsync();
        Task<PaginatedResult<Agent>> GetPaginatedAsync(string? search, bool? moderationFilter, int page, int pageSize);
        Task<Agent?> GetByIdAsync(Guid id);
        Task AddAsync(Agent agent);
        Task UpdateAsync(Agent agent);
        Task DeleteAsync(Guid id);
    }
}
