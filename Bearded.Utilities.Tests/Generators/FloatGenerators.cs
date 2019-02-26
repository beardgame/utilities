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
    }
}