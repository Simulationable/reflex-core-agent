using System.ComponentModel.DataAnnotations;

namespace ReflexCoreAgent.Domain.Entities
{
    public class Agent
    {
        public Guid Id { get; set; }
        [Required(ErrorMessage = "กรุณาระบุชื่อ Agent")]
        public string Name { get; set; } = string.Empty;

        [Required(ErrorMessage = "กรุณาระบุจุดประสงค์")]
        public string Purpose { get; set; } = string.Empty;
        public string? PromptTemplate { get; set; }
        public bool EnableModeration { get; set; } = true;
        public List<ModerationRule> ModerationRules { get; set; } = new();
        public LlamaRequestConfig Config { get; set; } = new();
    }
}
