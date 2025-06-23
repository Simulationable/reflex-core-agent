namespace ReflexCoreAgent.Interfaces
{
    public interface ITranslator
    {
        Task<string> Answer(string intent, string knowledge, Guid agentId);
    }
}
