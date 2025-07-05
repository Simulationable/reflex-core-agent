using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;

namespace ReflexCoreAgent.Infrastructure.Repositories
{
    public class CompanyProfileRepository : ICompanyProfileRepository
    {
        private readonly AppDbContext _context;

        public CompanyProfileRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<CompanyProfile?> GetAsync()
        {
            return await _context.CompanyProfiles.FirstOrDefaultAsync();
        }

        public async Task AddAsync(CompanyProfile profile)
        {
            await _context.CompanyProfiles.AddAsync(profile);
        }

        public void Update(CompanyProfile profile)
        {
            _context.CompanyProfiles.Update(profile);
        }

        public void Delete(CompanyProfile profile)
        {
            _context.CompanyProfiles.Remove(profile);
        }
    }
}
