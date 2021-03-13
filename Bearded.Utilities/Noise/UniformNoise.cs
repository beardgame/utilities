using System;
using static Bearded.Utilities.Noise.NoiseUtils;

namespace Bearded.Utilities.Noise
{
    public static class UniformNoise
    {
        public static INoiseMap Generate(int numCellsX, int numCellsY, int? seed) =>
            Generate(numCellsX, numCellsY, Interpolation2.Nearest, seed);

        public static INoiseMap Generate(int numCellsX, int numCellsY, IInterpolationMethod2 interpolation, int? seed)
        {
            if (numCellsX == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numCellsX));
            }
            if (numCellsY == 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numCellsY));
            }

            return NoiseMap.FromArray(GenerateRandomArray(numCellsX, numCellsY, r => r.NextDouble(), seed), interpolation);
        }
    }
}
