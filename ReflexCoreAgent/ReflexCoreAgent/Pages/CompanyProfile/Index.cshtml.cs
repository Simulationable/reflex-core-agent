using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces.Services;
using System.ComponentModel.DataAnnotations;

namespace ReflexCoreAgent.Pages.CompanyProfile
{
    public class IndexModel : PageModel
    {
        private readonly ICompanyProfileService _companyProfileService;

        public IndexModel(ICompanyProfileService companyProfileService)
        {
            _companyProfileService = companyProfileService;
        }

        [BindProperty]
        public Domain.Entities.CompanyProfile Profile { get; set; } = new();

        [BindProperty]
        public IFormFile? LogoFile { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {
            var existing = await _companyProfileService.GetAsync();
            if (existing != null)
            {
                Profile = existing;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (LogoFile != null)
            {
                using var ms = new MemoryStream();
                await LogoFile.CopyToAsync(ms);
                Profile.LogoBase64 = Convert.ToBase64String(ms.ToArray());
            }

            var existing = await _companyProfileService.GetAsync();
            if (existing == null)
            {
                await _companyProfileService.AddAsync(Profile);
            }
            else
            {
                existing.CompanyName = Profile.CompanyName;
                existing.Address = Profile.Address;
                existing.Phone = Profile.Phone;
                existing.LogoBase64 = Profile.LogoBase64;

                await _companyProfileService.UpdateAsync(existing);
            }

            return RedirectToPage();
        }
    }
}