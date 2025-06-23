using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;

public class ModerationRuleRepository : IModerationRuleRepository
{
    private readonly AppDbContext _context;

    public ModerationRuleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<ModerationRule>> GetActiveRulesAsync()
    {
        return await _context.ModerationRules
            .Where(r => r.IsActive)
            .AsNoTracking()
            .ToListAsync();
    }

    public async Task<List<ModerationRule>> GetByAgentIdAsync(Guid agentId)
    {
        return await _context.ModerationRules
            .Where(r => r.AgentId == agentId)
            .ToListAsync();
    }

    public Task AddAsync(ModerationRule rule)
    {
        var tracked = _context.ChangeTracker.Entries<ModerationRule>()
            .Any(e => e.Entity.Id == rule.Id);

        if (!tracked)
        {
            _context.ModerationRules.Add(rule);
        }

        return Task.CompletedTask;
    }

    public async Task UpdateAsync(ModerationRule updatedRule)
    {
        var existingRule = await _context.ModerationRules
            .FirstOrDefaultAsync(r => r.Id == updatedRule.Id);

        if (existingRule != null)
        {
            existingRule.Keyword = updatedRule.Keyword;
            existingRule.ResponseMessage = updatedRule.ResponseMessage;
            existingRule.IsActive = updatedRule.IsActive;
            existingRule.AgentId = updatedRule.AgentId;
        }
    }

    public Task DeleteAsync(ModerationRule rule)
    {
        var tracked = _context.ChangeTracker.Entries<ModerationRule>()
            .Any(e => e.Entity.Id == rule.Id);

        if (!tracked)
        {
            _context.ModerationRules.Remove(rule);
        }
        else
        {
            _context.Entry(rule).State = EntityState.Deleted;
        }

        return Task.CompletedTask;
    }
}