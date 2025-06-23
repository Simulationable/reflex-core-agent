namespace ReflexCoreAgent.Interfaces
{
    public interface IEmbeddingService
    {
        float[] Encode(string text);
    }
}
