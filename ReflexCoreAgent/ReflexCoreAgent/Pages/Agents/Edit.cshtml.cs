using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Pages.Agents
{
    public class EditModel : PageModel
    {
        private readonly IAgentService _agentService;

        public EditModel(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [BindProperty]
        public Agent Agent { get; set; } = new();

        public async Task<IActionResult> OnGetAsync(Guid id)
        {
            var agent = await _agentService.GetByIdAsync(id);
            if (agent == null)
            {
                return NotFound();
            }

            Agent = agent;
            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            Agent.Id = Agent.Id == Guid.Empty ? Guid.NewGuid() : Agent.Id;

            if (Agent.Config != null)
            {
                Agent.Config.AgentId = Agent.Id;
            }

            foreach (var rule in Agent.ModerationRules)
            {
                rule.AgentId = Agent.Id;
            }

            await _agentService.UpdateAsync(Agent);
            return RedirectToPage("/Agents/Index");
        }
    }
}