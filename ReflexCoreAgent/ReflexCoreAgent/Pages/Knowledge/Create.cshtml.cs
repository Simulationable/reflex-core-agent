using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Pages.Knowledge
{
    public class CreateModel : PageModel
    {
        private readonly IKnowledgeService _knowledgeService;
        private readonly IAgentService _agentService;

        public CreateModel(IKnowledgeService knowledgeService, IAgentService agentService)
        {
            _knowledgeService = knowledgeService;
            _agentService = agentService;
        }

        [BindProperty]
        public KnowledgeEntry Knowledge { get; set; } = new();

        public List<SelectListItem> AgentOptions { get; set; } = new();

        public async Task<IActionResult> OnGetAsync()
        {
            var agents = await _agentService.GetActiveAllAsync();
            AgentOptions = agents.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                await LoadAgentsAsync();
                return Page();
            }

            await _knowledgeService.CreateAsync(Knowledge);
            return RedirectToPage("/Knowledge/Index");
        }

        private async Task LoadAgentsAsync()
        {
            var agents = await _agentService.GetActiveAllAsync();
            AgentOptions = agents.Select(a => new SelectListItem
            {
                Value = a.Id.ToString(),
                Text = a.Name
            }).ToList();
        }
    }
}
