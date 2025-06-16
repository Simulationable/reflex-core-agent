using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Applications
{
    public class MemoryStore : IMemoryStore
    {
        private readonly Dictionary<string, UserProfile> _memory = new();

        public Task<UserProfile?> LoadProfileAsync(string userId)
        {
            _memory.TryGetValue(userId, out var profile);
            return Task.FromResult(profile);
        }

        public Task SaveInteractionAsync(string userId, string inputTh, string responseTh)
        {
            if (!_memory.ContainsKey(userId))
                _memory[userId] = new UserProfile { UserId = userId };

            _memory[userId].RecentTopics.Add(inputTh);
            return Task.CompletedTask;
        }
    }
}
