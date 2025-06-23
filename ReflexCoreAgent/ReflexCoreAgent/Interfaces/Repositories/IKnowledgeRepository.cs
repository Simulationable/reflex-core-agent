using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface IKnowledgeRepository
    {
        Task<List<KnowledgeEntry>> GetActiveByAgentIdAsync(Guid agentId);
        Task<KnowledgeEntry?> SearchBestMatchAsync(Guid agentId, string question);

        Task<KnowledgeEntry?> GetByIdAsync(Guid id);
        Task<List<KnowledgeEntry>> GetAllAsync();
        Task AddAsync(KnowledgeEntry entry);
        void Update(KnowledgeEntry entry);
        void Delete(KnowledgeEntry entry);
    }
}
