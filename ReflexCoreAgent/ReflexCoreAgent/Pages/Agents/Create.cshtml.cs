using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Pages.Agents
{
    public class CreateModel : PageModel
    {
        private readonly IAgentService _agentService;

        public CreateModel(IAgentService agentService)
        {
            _agentService = agentService;
        }

        [BindProperty]
        public Agent Agent { get; set; } = new();

        public void OnGet()
        {
            if (Agent.Config == null)
                Agent.Config = new LlamaRequestConfig();

            if (Agent.ModerationRules == null || !Agent.ModerationRules.Any())
            {
                Agent.ModerationRules = new List<ModerationRule>
            {
                new ModerationRule { Keyword = "ห้ามพูดคำนี้" },
                new ModerationRule { Keyword = "คำต้องห้าม 2" }
            };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            Agent.Id = Guid.NewGuid();
            await _agentService.AddAsync(Agent);
            return RedirectToPage("/Agents/Index");
        }
    }

}
