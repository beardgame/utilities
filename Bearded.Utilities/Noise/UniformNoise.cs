using System;
using static Bearded.Utilities.Noise.NoiseUtils;

namespace Bearded.Utilities.Noise
{
    public static class UniformNoise
    {
        public static INoiseMap Generate(int width, int height, int? seed) =>
            Generate(width, height, Interpolation2.Nearest, seed);

        public static INoiseMap Generate(int width, int height, IInterpolationMethod2 interpolation, int? seed)
        {
            if (width == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            return NoiseMap.FromArray(GenerateRandomArray(width, height, r => r.NextDouble(), seed), interpolation);
        }
    }
}
