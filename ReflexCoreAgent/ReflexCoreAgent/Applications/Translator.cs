using ReflexCoreAgent.Domain.Response;
using ReflexCoreAgent.Helpers;
using ReflexCoreAgent.Interfaces.Services;
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

        public async Task<string> Answer(string intent, string knowledge, Guid agentId, string? context = null)
        {
            intent ??= string.Empty;
            knowledge ??= string.Empty;
            context ??= string.Empty;

            var (isBlocked, message, agent) = await _moderationFilter.CheckAsync(intent, agentId);
            if (isBlocked)
                return message;

            if (agent == null)
            {
                return string.IsNullOrWhiteSpace(knowledge)
                    ? "ขออภัย ระบบยังไม่มีข้อมูลสำหรับคำถามนี้"
                    : knowledge;
            }

            var promptBody = agent.PromptTemplate
                    ?.Replace("{knowledge}", string.IsNullOrWhiteSpace(knowledge) ? "ไม่มีข้อมูล" : knowledge)
                    ?.Replace("{intent}", intent) ?? $"{intent}\n{knowledge}";

            var fullPrompt = $"{context}\n{promptBody}";

            var response = await LlamaCppHelper.RunAsync("thaigpt", fullPrompt, agent.Config);
            var parsed = JsonSerializer.Deserialize<LlamaCppResponse>(response);

            return string.IsNullOrWhiteSpace(parsed?.content)
                ? "ขออภัย ระบบไม่สามารถสร้างคำตอบได้ในตอนนี้"
                : Regex.Replace(parsed.content, @"^\s+|\s+$", "");
        }
    }
}
