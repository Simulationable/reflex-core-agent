using System.Text.Json.Serialization;

namespace ReflexCoreAgent.Domain.Entities
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Text.Json;
    using System.Text.Json.Serialization;

    public class LlamaRequestConfig
    {
        [Key]
        public Guid Id { get; set; }


        [JsonPropertyName("n_predict")]
        public int NPredict { get; set; } = 128;

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("top_p")]
        public double? TopP { get; set; }

        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }
        [JsonIgnore]
        public Guid? AgentId { get; set; }
        [JsonIgnore]
        public Agent? Agent { get; set; }

        [JsonPropertyName("prompt")]
        [NotMapped]
        public string Prompt { get; set; } = string.Empty;

        [NotMapped]
        [JsonPropertyName("stop")]
        public string[] Stop { get; set; } = Array.Empty<string>();

        [Column("StopSerialized")]
        public string StopSerialized
        {
            get => string.Join(",", Stop);
            set => Stop = string.IsNullOrWhiteSpace(value)
                ? Array.Empty<string>()
                : value.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
        }
    }
}
