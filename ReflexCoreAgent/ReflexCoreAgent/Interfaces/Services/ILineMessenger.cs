namespace ReflexCoreAgent.Interfaces.Services
{
    public interface ILineMessenger
    {
        Task ReplyAsync(string replyToken, string message);
    }
}
