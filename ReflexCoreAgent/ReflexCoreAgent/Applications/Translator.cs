using ReflexCoreAgent.Domain.Response;
using ReflexCoreAgent.Helpers;
using ReflexCoreAgent.Interfaces;
using System.Text.Json;
using System.Text.RegularExpressions;

namespace ReflexCoreAgent.Applications
{
    public class Translator : ITranslator
    {
        private readonly IModerationFilter _moderationFilter;

        public Translator(IModerationFilter moderationFilter)
        {
            _moderationFilter = moderationFilter;
        }

        public async Task<string> Answer(string intent, string knowledge, Guid agentId)
        {
            var (isBlocked, message, agent) = await _moderationFilter.CheckAsync(intent, agentId);
            if (isBlocked)
            {
                return message;
            }

            string prompt = agent.PromptTemplate
                .Replace("{knowledge}", string.IsNullOrWhiteSpace(knowledge) ? "ไม่มีข้อมูล" : knowledge)
                .Replace("{intent}", intent);

            var response = await LlamaCppHelper.RunAsync("thaigpt", prompt, agent!.Config);
            var parsed = JsonSerializer.Deserialize<LlamaCppResponse>(response);
            var result = Regex.Replace(parsed.content, @"^\s+|\s+$", "");
            return result;
        }
    }
}
