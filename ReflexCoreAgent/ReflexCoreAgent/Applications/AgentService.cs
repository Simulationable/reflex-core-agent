using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Applications
{
    public class AgentService : IAgentService
    {
        private readonly IUnitOfWork _uow;

        public AgentService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<List<Agent>> GetActiveAllAsync()
        {
            var agents = await _uow.Agents.GetAllAsync();
            return agents.OrderBy(a => a.Name).ToList();
        }

        public async Task<PaginatedResult<Agent>> GetPaginatedAsync(string? search, bool? moderationFilter, int page, int pageSize)
        {
            var agents = await _uow.Agents.GetAllAsync();

            if (!string.IsNullOrWhiteSpace(search))
                agents = agents.Where(a => a.Name.Contains(search, StringComparison.OrdinalIgnoreCase) || a.Purpose.Contains(search, StringComparison.OrdinalIgnoreCase)).ToList();

            if (moderationFilter.HasValue)
                agents = agents.Where(a => a.EnableModeration == moderationFilter.Value).ToList();

            var totalCount = agents.Count;
            var items = agents
                .OrderBy(a => a.Name)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return new PaginatedResult<Agent>
            {
                Items = items,
                TotalCount = totalCount,
                Page = page,
                PageSize = pageSize
            };
        }

        public async Task<Agent?> GetByIdAsync(Guid id)
        {
            return await _uow.Agents.GetByIdAsync(id);
        }

        public async Task AddAsync(Agent agent)
        {
            await _uow.Agents.AddAsync(agent);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(Agent updatedAgent)
        {
            var existingAgent = await _uow.Agents.GetByIdAsync(updatedAgent.Id);
            if (existingAgent == null)
                throw new InvalidOperationException("Agent not found");

            existingAgent.Name = updatedAgent.Name;
            existingAgent.Purpose = updatedAgent.Purpose;
            existingAgent.PromptTemplate = updatedAgent.PromptTemplate;
            existingAgent.EnableModeration = updatedAgent.EnableModeration;

            if (existingAgent.Config == null)
            {
                updatedAgent.Config.AgentId = updatedAgent.Id;
                await _uow.LlamaRequestConfig.AddAsync(updatedAgent.Config);
                existingAgent.Config = updatedAgent.Config;
            }
            else
            {
                existingAgent.Config.NPredict = updatedAgent.Config.NPredict;
                existingAgent.Config.Temperature = updatedAgent.Config.Temperature;
                existingAgent.Config.TopK = updatedAgent.Config.TopK;
                existingAgent.Config.TopP = updatedAgent.Config.TopP;
                existingAgent.Config.StopSerialized = updatedAgent.Config.StopSerialized;
                await _uow.LlamaRequestConfig.UpdateAsync(existingAgent.Config);
            }

            var existingRules = await _uow.ModerationRules.GetByAgentIdAsync(existingAgent.Id);
            var updatedIds = updatedAgent.ModerationRules.Select(r => r.Id).ToHashSet();

            foreach (var rule in existingRules)
            {
                if (!updatedIds.Contains(rule.Id))
                {
                    await _uow.ModerationRules.DeleteAsync(rule);
                }
            }

            foreach (var rule in updatedAgent.ModerationRules)
            {
                rule.AgentId = existingAgent.Id;

                if (rule.Id == Guid.Empty)
                {
                    rule.Id = Guid.NewGuid();
                    await _uow.ModerationRules.AddAsync(rule);
                }
                else
                {
                    await _uow.ModerationRules.UpdateAsync(rule);
                }
            }

            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var agent = await _uow.Agents.GetByIdAsync(id);
            if (agent != null)
            {
                _uow.Agents.Delete(agent);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
