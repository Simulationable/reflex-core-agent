using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Repositories
{
    public interface ICompanyProfileRepository
    {
        Task<CompanyProfile?> GetAsync();
        Task AddAsync(CompanyProfile profile);
        void Update(CompanyProfile profile);
        void Delete(CompanyProfile profile);
    }
}
