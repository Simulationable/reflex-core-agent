namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IUserService
    {
        Task<User?> AuthenticateAsync(string username, string password);
    }
}
