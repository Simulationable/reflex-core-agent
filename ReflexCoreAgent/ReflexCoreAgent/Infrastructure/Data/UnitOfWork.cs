using ReflexCoreAgent.Interfaces.Repositories;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Infrastructure.Data
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly AppDbContext _db;
        public IUserRepository Users { get; }
        public IUserInteractionRepository UserInteractions { get; }
        public IAgentRepository Agents { get; }
        public IModerationRuleRepository ModerationRules { get; }
        public ILlamaRequestConfigRepository LlamaRequestConfig { get; }
        public IKnowledgeRepository Knowledge { get; }
        public ICompanyProfileRepository CompanyProfiles { get; }

        public UnitOfWork(
            AppDbContext db, 
            IUserRepository userRepository, 
            IAgentRepository agentRepository, 
            IModerationRuleRepository moderationRuleRepository, 
            IKnowledgeRepository knowledge, 
            IUserInteractionRepository userInteractions, 
            ILlamaRequestConfigRepository llamaRequestConfig,
            ICompanyProfileRepository companyProfiles)
        {
            _db = db;
            Users = userRepository;
            Agents = agentRepository;
            ModerationRules = moderationRuleRepository;
            Knowledge = knowledge;
            UserInteractions = userInteractions;
            LlamaRequestConfig = llamaRequestConfig;
            CompanyProfiles = companyProfiles;
        }

        public async Task<int> SaveChangesAsync()
        {
            return await _db.SaveChangesAsync();
        }

        public void Dispose()
        {
            _db.Dispose();
        }
    }

}
