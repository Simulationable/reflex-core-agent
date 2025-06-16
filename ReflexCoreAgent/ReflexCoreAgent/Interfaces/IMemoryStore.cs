using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces
{
    public interface IMemoryStore
    {
        Task<UserProfile?> LoadProfileAsync(string userId);
        Task SaveInteractionAsync(string userId, string inputTh, string responseTh);
    }
}
