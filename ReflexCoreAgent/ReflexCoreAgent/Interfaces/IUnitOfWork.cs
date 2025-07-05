using ReflexCoreAgent.Interfaces.Repositories;

namespace ReflexCoreAgent.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IUserInteractionRepository UserInteractions { get; }
        IAgentRepository Agents { get; }
        IModerationRuleRepository ModerationRules { get; }
        ILlamaRequestConfigRepository LlamaRequestConfig { get; }
        IKnowledgeRepository Knowledge { get; }
        ICompanyProfileRepository CompanyProfiles { get; }
        Task<int> SaveChangesAsync();
    }
}
