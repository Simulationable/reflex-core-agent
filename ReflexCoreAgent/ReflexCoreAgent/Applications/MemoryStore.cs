using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Applications
{
    public class InteractionService : IInteractionService
    {
        private readonly IUnitOfWork _uow;

        public InteractionService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task SaveInteractionAsync(string userId, string inputTh, string responseTh)
        {
            var interaction = new UserInteraction
            {
                UserId = userId,
                InputTh = inputTh,
                ResponseTh = responseTh
            };

            await _uow.UserInteractions.SaveInteractionAsync(interaction);
            await _uow.SaveChangesAsync();
        }

        public async Task<List<UserInteraction>> GetHistoryAsync(string userId)
        {
            return await _uow.UserInteractions.GetByUserIdAsync(userId);
        }
    }

}
