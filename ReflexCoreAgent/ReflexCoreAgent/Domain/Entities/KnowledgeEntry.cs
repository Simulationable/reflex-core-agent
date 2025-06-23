using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;

namespace ReflexCoreAgent.Domain.Entities
{
    public class KnowledgeEntry
    {
        public Guid Id { get; set; }

        [Required, MaxLength(200)]
        public string Question { get; set; } = string.Empty;

        [Required]
        public string Answer { get; set; } = string.Empty;

        [MaxLength(100)]
        public string? Category { get; set; } 

        public List<string> Tags { get; set; } = new();
        public float[] EmbeddingVector { get; set; } = Array.Empty<float>();

        public string? Source { get; set; } 
        public float Confidence { get; set; } = 1.0f;

        public int Version { get; set; } = 1;
        public bool IsActive { get; set; } = true;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public DateTime? UpdatedAt { get; set; }

        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; }

        public Guid AgentId { get; set; }
        [ValidateNever]
        [JsonIgnore]
        public Agent Agent { get; set; } = null!;

        [NotMapped]
        public string TagsCsv
        {
            get => string.Join(",", Tags);
            set => Tags = value?.Split(',', StringSplitOptions.RemoveEmptyEntries)
                               .Select(t => t.Trim()).ToList() ?? new List<string>();
        }
    }
}
