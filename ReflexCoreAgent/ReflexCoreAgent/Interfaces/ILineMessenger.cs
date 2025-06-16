namespace ReflexCoreAgent.Interfaces
{
    public interface ILineMessenger
    {
        Task ReplyAsync(string replyToken, string message);
    }
}
