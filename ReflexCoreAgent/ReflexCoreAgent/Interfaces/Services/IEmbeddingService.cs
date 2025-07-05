namespace ReflexCoreAgent.Interfaces.Services
{
    public interface IEmbeddingService
    {
        float[] Encode(string text);
    }
}
