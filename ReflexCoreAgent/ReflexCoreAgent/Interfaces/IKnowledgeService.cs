using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces
{
    public interface IKnowledgeService
    {
        Task<List<KnowledgeEntry>> GetActiveAllAsync();
        Task<KnowledgeEntry> GetKnowledgeAsync(Guid id);
        Task<PaginatedResult<KnowledgeEntry>> GetPaginatedAsync(string? search, bool? isActive, int page, int pageSize);
        Task<string?> SearchAnswerAsync(string userQuestion, Guid agentId);
        Task<int> CreateAsync(KnowledgeEntry knowledge);
        Task<int> UpdateAsync(KnowledgeEntry knowledge);
        Task<int> DeleteAsync(Guid id);
    }
}
