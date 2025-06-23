using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Pages.Knowledge
{
    public class IndexModel : PageModel
    {
        private readonly IKnowledgeService _knowledgeService;

        public IndexModel(IKnowledgeService knowledgeService)
        {
            _knowledgeService = knowledgeService;
        }

        public PaginatedResult<KnowledgeEntry> Result { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? IsActiveFilter { get; set; }

        [BindProperty(SupportsGet = true)]
        public int Page { get; set; } = 1;

        [BindProperty(SupportsGet = true)]
        public int PageSize { get; set; } = 10;

        public async Task OnGetAsync()
        {
            var page = Request.Query["page"];
            if (string.IsNullOrEmpty(page))
            {
                Page = 1;
            }
            else if(!string.IsNullOrEmpty(page))
            {
                Page = int.Parse(page);
            }
            Result = await _knowledgeService.GetPaginatedAsync(Search, IsActiveFilter, Page, PageSize);
        }

        [BindProperty]
        public Guid DeleteKnowledgeId { get; set; }

        public async Task<IActionResult> OnPostDeleteKnowledgeAsync(Guid id)
        {
            await _knowledgeService.DeleteAsync(id);
            return new JsonResult(new { success = true });
        }
    }

}
