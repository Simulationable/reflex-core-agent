namespace ReflexCoreAgent.Domain.Model
{
    public class LineWebhookPayload
    {
        public string Destination { get; set; }
        public List<LineWebhookEvent> Events { get; set; }
    }

    public class LineWebhookEvent
    {
        public string ReplyToken { get; set; }
        public string Type { get; set; }
        public long Timestamp { get; set; }
        public LineSource Source { get; set; }
        public LineMessage Message { get; set; }
    }

    public class LineSource
    {
        public string UserId { get; set; }
        public string Type { get; set; }
    }

    public class LineMessage
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public string Text { get; set; }
    }
}
