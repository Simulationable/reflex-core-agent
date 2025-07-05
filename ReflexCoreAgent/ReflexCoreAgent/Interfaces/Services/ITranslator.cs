namespace ReflexCoreAgent.Interfaces.Services
{
    public interface ITranslator
    {
        Task<string> Answer(string intent, string knowledge, Guid agentId, string? context = null);
    }
}
