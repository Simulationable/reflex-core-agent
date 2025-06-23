namespace ReflexCoreAgent.Domain.Model
{
    public class ModerationRuleViewModel
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Keyword { get; set; } = "";
        public string ResponseMessage { get; set; } = "";
        public bool IsActive { get; set; }
    }
}
