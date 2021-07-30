using Bearded.Utilities.Geometry;
using FsCheck;

namespace Bearded.Utilities.Tests.Generators
{
    static class DirectionGenerators
    {
        public static class All
        {
            public static Arbitrary<Direction2> Directions() =>
                Arb.Generate<uint>().Select(i => Direction2.FromDegrees(360f / (1L << 32))).ToArbitrary();
        }
    }
}
