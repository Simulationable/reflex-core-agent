namespace ReflexCoreAgent.Domain.Model
{
    public class UserProfile
    {
        public string UserId { get; set; } = string.Empty;
        public List<string> RecentTopics { get; set; } = new();
        public string ContextSummary => string.Join(", ", RecentTopics.TakeLast(3));
    }
}
