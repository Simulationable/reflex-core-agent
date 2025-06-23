using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;

namespace ReflexCoreAgent.Infrastructure.Repositories
{
    public class UserInteractionRepository : IUserInteractionRepository
    {
        private readonly AppDbContext _db;

        public UserInteractionRepository(AppDbContext db)
        {
            _db = db;
        }

        public async Task SaveInteractionAsync(UserInteraction interaction)
        {
            await _db.UserInteractions.AddAsync(interaction);
        }

        public async Task<List<UserInteraction>> GetByUserIdAsync(string userId)
        {
            return await _db.UserInteractions
                .Where(i => i.UserId == userId)
                .OrderByDescending(i => i.Timestamp)
                .ToListAsync();
        }
    }
}
