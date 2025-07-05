namespace ReflexCoreAgent.Domain.Entities
{
    public class Appointment
    {
        public Guid Id { get; set; }
        public Guid AgentId { get; set; }
        public string Title { get; set; } = default!;
        public string? Description { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
