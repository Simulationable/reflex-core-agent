using ReflexCoreAgent.Interfaces;
using System.Security.Cryptography;
using System.Text;

namespace ReflexCoreAgent.Applications
{
    public class DeterministicEmbeddingService : IEmbeddingService
    {
        private const int Dimension = 384;

        public float[] Encode(string text)
        {
            var vector = new float[Dimension];

            var tokens = text.Split(' ', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            foreach (var token in tokens)
            {
                var hash = SHA256.HashData(Encoding.UTF8.GetBytes(token));
                for (int i = 0; i < Dimension; i++)
                {
                    int safeIdx = (i * 7) % (hash.Length - 4);
                    int seed = BitConverter.ToInt32(hash, safeIdx) ^ i;

                    var rand = new Random(seed);
                    vector[i] += (float)(rand.NextDouble() * 2.0 - 1.0);
                }
            }

            Normalize(vector);
            return vector;
        }

        private void Normalize(float[] vector)
        {
            float mag = (float)Math.Sqrt(vector.Sum(v => v * v));
            if (mag > 0)
            {
                for (int i = 0; i < vector.Length; i++)
                    vector[i] /= mag;
            }
        }
    }
}
