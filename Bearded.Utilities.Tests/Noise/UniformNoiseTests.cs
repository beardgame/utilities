using Bearded.Utilities.Noise;

namespace Bearded.Utilities.Tests.Noise;

public sealed class UniformNoiseTests : NoiseTests
{
    protected override IProceduralTexture CreateProceduralTexture(int seed) => UniformNoise.Generate(5, 5, new System.Random(seed));
}
