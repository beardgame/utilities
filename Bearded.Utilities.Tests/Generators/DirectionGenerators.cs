using Bearded.Utilities.Geometry;
using FsCheck;

namespace Bearded.Utilities.Tests.Generators;

static class DirectionGenerators
{
    public sealed class All
    {
        public static Arbitrary<Direction2> Directions() =>
            Arb.Generate<uint>().Select(i => Direction2.FromDegrees(i * 360f / (1L << 32))).ToArbitrary();
    }
}
