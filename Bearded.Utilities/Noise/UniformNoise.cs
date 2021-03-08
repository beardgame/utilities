using System;

namespace Bearded.Utilities.Noise
{
    public static class UniformNoise
    {
        public static INoiseMap Generate(int width, int height, int? seed) =>
            Generate(width, height, Interpolation2.Nearest, seed);

        public static INoiseMap Generate(int width, int height, IInterpolationMethod2 interpolation, int? seed) =>
            NoiseMap.FromArray(generateNoiseArray(width, height, seed), interpolation);

        private static double[,] generateNoiseArray(int width, int height, int? seed)
        {
            var r = seed == null ? new Random() : new Random(seed.Value);

            var array = new double[width, height];

            for (var y = 0; y < height; y++)
            {
                for (var x = 0; x < width; x++)
                {
                    array[x, y] = r.NextDouble();
                }
            }

            return array;
        }
    }
}
