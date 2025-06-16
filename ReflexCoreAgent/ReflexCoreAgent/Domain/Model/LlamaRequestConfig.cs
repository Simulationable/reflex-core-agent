using System.Text.Json.Serialization;

namespace ReflexCoreAgent.Domain.Model
{
    public class LlamaRequestConfig
    {
        [JsonPropertyName("prompt")]
        public string Prompt { get; set; }

        [JsonPropertyName("n_predict")]
        public int N_Predict { get; set; } = 128;

        [JsonPropertyName("temperature")]
        public double Temperature { get; set; } = 0.7;

        [JsonPropertyName("top_p")]
        public double? TopP { get; set; }

        [JsonPropertyName("top_k")]
        public int? TopK { get; set; }

        [JsonPropertyName("stop")]
        public string[] Stop { get; set; }
    }
}
