using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface IUserInteractionRepository
    {
        Task SaveInteractionAsync(UserInteraction interaction);
        Task<List<UserInteraction>> GetByUserIdAsync(string userId);
    }
}
