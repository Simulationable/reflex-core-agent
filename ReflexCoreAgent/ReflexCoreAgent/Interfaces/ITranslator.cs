namespace ReflexCoreAgent.Interfaces
{
    public interface ITranslator
    {
        Task<string> TranslateToEnglish(string thai);
        Task<string> TranslateToThai(string english);
        Task<string> Answer(string intent, string knowledge);
    }
}
