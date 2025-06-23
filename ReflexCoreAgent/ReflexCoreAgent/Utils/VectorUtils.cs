namespace ReflexCoreAgent.Utils
{
    public static class VectorUtils
    {
        public static float CosineSimilarity(float[] v1, float[] v2)
        {
            if (v1.Length != v2.Length || v1.Length == 0) return 0;

            float dot = 0, mag1 = 0, mag2 = 0;
            for (int i = 0; i < v1.Length; i++)
            {
                dot += v1[i] * v2[i];
                mag1 += v1[i] * v1[i];
                mag2 += v2[i] * v2[i];
            }

            return dot / (float)(Math.Sqrt(mag1) * Math.Sqrt(mag2));
        }
    }
}
