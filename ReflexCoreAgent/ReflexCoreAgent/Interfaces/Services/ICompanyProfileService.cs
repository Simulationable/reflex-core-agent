using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface ICompanyProfileService
    {
        Task<CompanyProfile?> GetAsync();
        Task AddAsync(CompanyProfile profile);
        Task UpdateAsync(CompanyProfile updatedProfile);
        Task DeleteAsync();
    }
}
