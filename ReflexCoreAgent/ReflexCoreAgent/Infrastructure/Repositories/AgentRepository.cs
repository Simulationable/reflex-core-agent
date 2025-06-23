using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;

namespace ReflexCoreAgent.Infrastructure.Repositories
{
    public class AgentRepository : IAgentRepository
    {
        private readonly AppDbContext _context;
        public AgentRepository(AppDbContext context) => _context = context;

        public async Task<Agent?> GetByIdAsync(Guid id) =>
            await _context.Agents
                .Include(a => a.ModerationRules)
                .Include(a => a.Config)
                .FirstOrDefaultAsync(a => a.Id == id);

        public async Task<List<Agent>> GetAllAsync()
        {
            return await _context.Agents
                .Include(a => a.ModerationRules)
                .Include(a => a.Config)
                .ToListAsync();
        }

        public async Task AddAsync(Agent agent)
        {
            await _context.Agents.AddAsync(agent);
        }

        public void Update(Agent agent)
        {
            _context.Agents.Update(agent);
        }

        public void Delete(Agent agent)
        {
            _context.Agents.Remove(agent);
        }
    }
}
