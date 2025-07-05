using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Applications
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _uow;

        public UserService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<User?> AuthenticateAsync(string username, string password)
        {
            var user = await _uow.Users.GetByUsernameAsync(username);
            if (user == null) return null;

            if (user != null && BCrypt.Net.BCrypt.Verify(password, user.PasswordHash))
            {
                return user;
            }

            return null;
        }
    }
}
