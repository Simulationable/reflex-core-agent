using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Pages.Agents
{
    public class IndexModel : PageModel
    {
        private readonly IAgentService _agentService;

        public IndexModel(IAgentService agentService)
        {
            _agentService = agentService;
        }

        public PaginatedResult<Agent> Result { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public string? Search { get; set; }

        [BindProperty(SupportsGet = true)]
        public bool? ModerationFilter { get; set; }

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
            else if (!string.IsNullOrEmpty(page))
            {
                Page = int.Parse(page);
            }
            Result = await _agentService.GetPaginatedAsync(Search, ModerationFilter, Page, PageSize);
        }

        [BindProperty]
        public Guid DeleteAgentId { get; set; }

        public async Task<IActionResult> OnPostDeleteAgentAsync(Guid id)
        {
            await _agentService.DeleteAsync(id);

            var page = Request.Query["page"];
            if (string.IsNullOrEmpty(page))
            {
                Page = 1;
            }
            else if (!string.IsNullOrEmpty(page))
            {
                Page = int.Parse(page);
            }
            Result = await _agentService.GetPaginatedAsync(Search, ModerationFilter, Page, PageSize);

            return new JsonResult(new { success = true });
        }
    }
}
