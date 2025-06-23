using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using ReflexCoreAgent.Domain.Entities;

namespace ReflexCoreAgent.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<UserInteraction> UserInteractions { get; set; }
        public DbSet<Agent> Agents { get; set; }
        public DbSet<ModerationRule> ModerationRules { get; set; }
        public DbSet<LlamaRequestConfig> LlamaRequestConfig { get; set; }
        public DbSet<KnowledgeEntry> KnowledgeEntries { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            var converter = new ValueConverter<float[], string>(
                v => string.Join(",", v),
                v => v.Split(',', StringSplitOptions.RemoveEmptyEntries).Select(float.Parse).ToArray()
            );

            var comparer = new ValueComparer<float[]>(
                (a, b) => a.SequenceEqual(b),
                a => a.Aggregate(0, (hash, x) => HashCode.Combine(hash, x.GetHashCode())),
                a => a.ToArray()
            );

            modelBuilder.Entity<KnowledgeEntry>()
                .Property(k => k.EmbeddingVector)
                .HasConversion(converter)
                .Metadata.SetValueComparer(comparer);
        }
    }
}
