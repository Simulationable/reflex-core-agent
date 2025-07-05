namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IPdfService
    {
        Task<string> GenerateQuotationAsync(string userInput, Guid agentId);
    }
}
