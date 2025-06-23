using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface IAgentRepository
    {
        Task<List<Agent>> GetAllAsync();
        Task<Agent?> GetByIdAsync(Guid id);
        Task AddAsync(Agent agent);
        void Update(Agent agent);
        void Delete(Agent agent);
    }
}
