using System;
using static Bearded.Utilities.Noise.NoiseUtils;

namespace Bearded.Utilities.Noise;

public static class UniformNoise
{
    public static IProceduralTexture Generate(int numCellsX, int numCellsY, Random? random) =>
        Generate(numCellsX, numCellsY, Interpolation2d.Nearest, random);

    public static IProceduralTexture Generate(int numCellsX, int numCellsY, IInterpolationMethod2d interpolation, Random? random)
    {
        if (numCellsX <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numCellsX));
        }
        if (numCellsY <= 0)
        {
            throw new ArgumentOutOfRangeException(nameof(numCellsY));
        }

        return ProceduralTexture.FromArray(GenerateRandomArray(numCellsX, numCellsY, r => r.NextDouble(), random), interpolation);
    }
}
