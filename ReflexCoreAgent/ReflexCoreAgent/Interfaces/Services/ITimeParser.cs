using ReflexCoreAgent.Domain.Model;

namespace ReflexCoreAgent.Interfaces.Services
{
    public interface ITimeParser
    {
        ParsedTime? Parse(string userInput);
    }
}
