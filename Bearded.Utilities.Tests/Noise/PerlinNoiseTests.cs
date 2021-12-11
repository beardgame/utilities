using Bearded.Utilities.Noise;

namespace Bearded.Utilities.Tests.Noise;

public sealed class PerlinNoiseTests : NoiseTests
{
    protected override IProceduralTexture CreateProceduralTexture(int seed) => PerlinNoise.Generate(5, 5, new System.Random(seed));
}
