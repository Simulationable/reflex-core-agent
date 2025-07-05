using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Interfaces.Services;

namespace ReflexCoreAgent.Pages.Calendar
{
    public class IndexModel : PageModel
    {
        private readonly ICalendarService _calendarService;
        private readonly IAgentService _agentService;

        public IndexModel(ICalendarService calendarService, IAgentService agentService)
        {
            _calendarService = calendarService;
            _agentService = agentService;
        }

        public List<Appointment> Appointments { get; set; } = new();
        public List<Agent> Agents { get; set; } = new();

        [BindProperty(SupportsGet = true)]
        public Guid? AgentId { get; set; }

        public Guid? SelectedAgentId => AgentId;

        public async Task OnGetAsync()
        {
            Agents = await _agentService.GetActiveAllAsync();

            if (AgentId.HasValue)
            {
                Appointments = await _calendarService.GetAppointmentsAsync(AgentId.Value);
            }
        }
    }
}