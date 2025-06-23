using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;

namespace ReflexCoreAgent.Infrastructure.Repositories
{
    public class LlamaRequestConfigRepository : ILlamaRequestConfigRepository
    {
        private readonly AppDbContext _context;

        public LlamaRequestConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<LlamaRequestConfig?> GetByIdAsync(Guid id)
        {
            return await _context.LlamaRequestConfig
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<LlamaRequestConfig?> GetByAgentIdAsync(Guid agentId)
        {
            return await _context.LlamaRequestConfig
                .AsNoTracking()
                .FirstOrDefaultAsync(c => c.AgentId == agentId);
        }

        public async Task<List<LlamaRequestConfig>> GetAllAsync()
        {
            return await _context.LlamaRequestConfig
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task AddAsync(LlamaRequestConfig config)
        {
            await _context.LlamaRequestConfig.AddAsync(config);
        }

        public Task UpdateAsync(LlamaRequestConfig config)
        {
            _context.LlamaRequestConfig.Update(config);
            return Task.CompletedTask;
        }

        public Task DeleteAsync(LlamaRequestConfig config)
        {
            _context.LlamaRequestConfig.Remove(config);
            return Task.CompletedTask;
        }
    }

}
