using Microsoft.EntityFrameworkCore;
using ReflexCoreAgent.Domain.Entities;
using ReflexCoreAgent.Infrastructure.Data;
using ReflexCoreAgent.Interfaces.Repositories;
using ReflexCoreAgent.Interfaces.Services;
using ReflexCoreAgent.Utils;

namespace ReflexCoreAgent.Infrastructure.Repositories
{
    public class KnowledgeRepository : IKnowledgeRepository
    {
        IEmbeddingService _embeddingService;
        private readonly AppDbContext _db;

        public KnowledgeRepository(AppDbContext db,
            IEmbeddingService embeddingService)
        {
            _db = db;
            _embeddingService = embeddingService;
        }

        public async Task<List<KnowledgeEntry>> GetActiveByAgentIdAsync(Guid agentId)
        {
            return await _db.KnowledgeEntries
                .Where(k => k.AgentId == agentId && k.IsActive)
                .ToListAsync();
        }

        public async Task<KnowledgeEntry?> SearchBestMatchAsync(Guid agentId, string question)
        {
            var cleaned = Preprocess(question);
            var questionEmbedding = _embeddingService.Encode(cleaned);

            var candidates = await _db.KnowledgeEntries
                .Where(k => k.AgentId == agentId && k.IsActive)
                .ToListAsync();

            if (candidates.Count == 0)
                return null;

            return candidates
                .Select(k =>
                {
                    var cleanedCandidate = Preprocess(k.Question);

                    // Cosine similarity
                    var simEmbedding = VectorUtils.CosineSimilarity(questionEmbedding, k.EmbeddingVector);

                    // Substring / keyword matching
                    var simExact = cleanedCandidate.Contains(cleaned) || cleaned.Contains(cleanedCandidate) ? 1f : 0f;

                    // Levenshtein distance score (scaled)
                    var lev = Levenshtein(cleaned, cleanedCandidate);
                    var maxLen = Math.Max(cleaned.Length, cleanedCandidate.Length);
                    var simLevenshtein = maxLen == 0 ? 1f : 1f - ((float)lev / maxLen);

                    // Combined score with weights
                    var score = (simEmbedding * 0.6f) + (simLevenshtein * 0.3f) + (simExact * 0.1f);

                    return new { Entry = k, Score = score };
                })
                .OrderByDescending(x => x.Score)
                .FirstOrDefault(x => x.Score >= 0.3f)
                ?.Entry;
        }

        public async Task<KnowledgeEntry?> GetByIdAsync(Guid id)
        {
            return await _db.KnowledgeEntries.FindAsync(id);
        }

        public async Task<List<KnowledgeEntry>> GetAllAsync()
        {
            return await _db.KnowledgeEntries.ToListAsync();
        }

        public async Task AddAsync(KnowledgeEntry entry)
        {
            entry.EmbeddingVector = _embeddingService.Encode(entry.Question);

            await _db.KnowledgeEntries.AddAsync(entry);
        }

        public void Update(KnowledgeEntry entry)
        {
            _db.KnowledgeEntries.Update(entry);
        }

        public void Delete(KnowledgeEntry entry)
        {
            _db.KnowledgeEntries.Remove(entry);
        }
        private string Preprocess(string input)
        {
            var stopwords = new[] { "ครับ", "ค่ะ", "คุณ", "ขอ", "ช่วย", "หน่อย", "ได้ไหม", "ได้มั้ย", "รบกวน", "หน่อยครับ", "ครับผม", "ค่ะคุณ" };
            var lowered = input.ToLowerInvariant();
            foreach (var stop in stopwords)
                lowered = lowered.Replace(stop, "", StringComparison.OrdinalIgnoreCase);
            return lowered.Trim();
        }

        private int Levenshtein(string s, string t)
        {
            var n = s.Length;
            var m = t.Length;
            var d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; i++) d[i, 0] = i;
            for (int j = 0; j <= m; j++) d[0, j] = j;

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (s[i - 1] == t[j - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            return d[n, m];
        }

    }
}
