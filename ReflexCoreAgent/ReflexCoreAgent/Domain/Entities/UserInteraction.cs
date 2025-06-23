using System.ComponentModel.DataAnnotations;

namespace ReflexCoreAgent.Domain.Entities
{
    public class UserInteraction
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string UserId { get; set; } = string.Empty;

        [Required]
        public string InputTh { get; set; } = string.Empty;

        [Required]
        public string ResponseTh { get; set; } = string.Empty;

        public DateTime Timestamp { get; set; } = DateTime.UtcNow;
    }
}
