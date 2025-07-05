using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IInteractionService
    {
        Task SaveInteractionAsync(string userId, string inputTh, string responseTh);
        Task<List<UserInteraction>> GetHistoryAsync(string userId);
    }
}
