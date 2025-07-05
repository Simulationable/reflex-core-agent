using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Pages.Knowledge
{
    public class EditModel : PageModel
    {
        private readonly IKnowledgeService _knowledgeService;
        private readonly IAgentService _agentService;

        public EditModel(IKnowledgeService knowledgeService,
            IAgentService agentService)
        {
            _knowledgeService = knowledgeService;
            _agentService = agentService;
        }

        [BindProperty]
        public KnowledgeEntry Knowledge { get; set; }

        public SelectList AgentOptions { get; set; }

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            Knowledge = await _knowledgeService.GetKnowledgeAsync(id);
            if (Knowledge == null)
                return NotFound();

            var agents = await _agentService.GetActiveAllAsync();
            AgentOptions = new SelectList(agents, "Id", "Name", Knowledge.AgentId);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                var agents = await _agentService.GetActiveAllAsync();
                AgentOptions = new SelectList(agents, "Id", "Name", Knowledge.AgentId);
                return Page();
            }

            var success = await _knowledgeService.UpdateAsync(Knowledge);

            return RedirectToPage("Index");
        }
    }
}
