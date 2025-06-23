using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface ILlamaRequestConfigRepository
    {
        Task<LlamaRequestConfig?> GetByIdAsync(Guid id);
        Task<LlamaRequestConfig?> GetByAgentIdAsync(Guid agentId);
        Task<List<LlamaRequestConfig>> GetAllAsync();
        Task AddAsync(LlamaRequestConfig config);
        Task UpdateAsync(LlamaRequestConfig config);
        Task DeleteAsync(LlamaRequestConfig config);
    }
}
