namespace ReflexCoreAgent.Interfaces.Services
{
    public interface INotificationService
    {
        Task AlertSalesTeamAsync(string userInput, Guid agentId);
    }
}
