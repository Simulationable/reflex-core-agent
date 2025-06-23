using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Applications
{
    public class KnowledgeService : IKnowledgeService
    {
        private readonly IUnitOfWork _uow;

        public KnowledgeService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<KnowledgeEntry>> GetActiveAllAsync()
        {
            return await _uow.Knowledge.GetAllAsync();
        }

        public async Task<KnowledgeEntry> GetKnowledgeAsync(Guid id)
        {
            return await _uow.Knowledge.GetByIdAsync(id) ?? throw new KeyNotFoundException($"Knowledge entry with ID {id} not found.");
        }

        public async Task<PaginatedResult<KnowledgeEntry>> GetPaginatedAsync(string? search, bool? isActive, int page, int pageSize)
        {
            var query = await _uow.Knowledge.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(k =>
                    k.Question.Contains(search) || k.Answer.Contains(search)).ToList();
            }

            if (isActive.HasValue)
            {
                query = query.Where(k => k.IsActive == isActive.Value).ToList();
            }

            var total = query.Count();
            var items = query
                .OrderByDescending(k => k.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize);

            return new PaginatedResult<KnowledgeEntry>
            {
                Items = items.ToList(),
                Page = page,
                PageSize = pageSize,
                TotalCount = total
            };
        }

        public async Task<string?> SearchAnswerAsync(string userQuestion, Guid agentId)
        {
            var bestMatch = await _uow.Knowledge.SearchBestMatchAsync(agentId, userQuestion);
            return bestMatch?.Answer;
        }

        public async Task<IEnumerable<KnowledgeEntry>> GetAllByAgentAsync(Guid agentId)
        {
            return await _uow.Knowledge.GetActiveByAgentIdAsync(agentId);
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(Guid id)
        {
            return await _uow.Knowledge.GetByIdAsync(id);
        }

        public async Task<int> CreateAsync(KnowledgeEntry knowledge)
        {
            await _uow.Knowledge.AddAsync(knowledge);
            var match = await _uow.Knowledge.SearchBestMatchAsync(knowledge.AgentId, knowledge.Question);
            if (match != null)
            {
                return -1;
            }
            return await _uow.SaveChangesAsync();
        }

        public async Task<int> UpdateAsync(KnowledgeEntry knowledge)
        {
            _uow.Knowledge.Update(knowledge);
            return await _uow.SaveChangesAsync();
        }

        public async Task<int> DeleteAsync(Guid id)
        {
            var knowledge = await _uow.Knowledge.GetByIdAsync(id);
            if (knowledge == null) return 0;

            _uow.Knowledge.Delete(knowledge);
            return await _uow.SaveChangesAsync();
        }
    }
}
