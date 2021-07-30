using System;
using FsCheck;

namespace Bearded.Utilities.Tests.Generators
{
    static class FloatGenerators
    {
        public static class NonInfiniteNonNaN
        {
            public static Arbitrary<float> Floats()
                => Arb.Default.Float32().Filter(f => !float.IsInfinity(f) && !float.IsNaN(f));
        }

        public static class PositiveNonInfiniteNonNaN
        {
            public static Arbitrary<float> Floats()
                => NonInfiniteNonNaN.Floats().Generator
                    .Where(f => f != 0)
                    .Select(Math.Abs)
                    .ToArbitrary();
        }
    }
}
