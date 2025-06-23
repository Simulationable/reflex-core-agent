using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ReflexCoreAgent.Domain.Model;
using ReflexCoreAgent.Interfaces;

namespace ReflexCoreAgent.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LineWebhookController : ControllerBase
    {
        private readonly IAgentOrchestrator _agent;

        public LineWebhookController(IAgentOrchestrator agent)
        {
            _agent = agent;
        }

        [HttpPost]
        [Route("webhook")]
        public async Task<IActionResult> Post([FromBody] LineWebhookPayload payload)
        {
            var agentName = new Guid(HttpContext.Request.Query["agent"].ToString());

            var reply = await _agent.HandleMessageAsync(payload, agentName);
            return Ok(reply);
        }
    }
}
