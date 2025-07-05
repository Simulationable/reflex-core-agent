using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Applications
{
    public class CompanyProfileService : ICompanyProfileService
    {
        private readonly IUnitOfWork _uow;

        public CompanyProfileService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<CompanyProfile?> GetAsync()
        {
            return await _uow.CompanyProfiles.GetAsync();
        }

        public async Task AddAsync(CompanyProfile profile)
        {
            await _uow.CompanyProfiles.AddAsync(profile);
            await _uow.SaveChangesAsync();
        }

        public async Task UpdateAsync(CompanyProfile updatedProfile)
        {
            var existing = await _uow.CompanyProfiles.GetAsync();
            if (existing == null)
                throw new InvalidOperationException("Company profile not found");

            existing.CompanyName = updatedProfile.CompanyName;
            existing.Address = updatedProfile.Address;
            existing.Phone = updatedProfile.Phone;
            existing.LogoBase64 = updatedProfile.LogoBase64;

            _uow.CompanyProfiles.Update(existing);
            await _uow.SaveChangesAsync();
        }

        public async Task DeleteAsync()
        {
            var existing = await _uow.CompanyProfiles.GetAsync();
            if (existing != null)
            {
                _uow.CompanyProfiles.Delete(existing);
                await _uow.SaveChangesAsync();
            }
        }
    }
}
