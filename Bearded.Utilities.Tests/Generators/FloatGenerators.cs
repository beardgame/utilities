using System;
using FsCheck;

namespace Bearded.Utilities.Tests.Generators;

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

    /// <summary>
    /// Generates a positive (specifically, non-zero) circle radius with a reasonable maximum radius to avoid
    /// positive infinities when doing geometric arithmetic with them.
    /// </summary>
    public static class PositiveCircleRadius
    {
        public static Arbitrary<float> Floats
            => Arb.Default.UInt32().Generator
                .Select(i => ((float) i + 1) / 1000)
                .ToArbitrary();
    }

    /// <summary>
    /// Generates floats with reasonable maximum magnitude to avoid positive infinities when doing arithmetic.
    /// </summary>
    public sealed class ForArithmetic
    {
        public static Arbitrary<float> Floats()
            => Arb.Default.Int32().Generator
                .Select(i => 0.001f * i)
                .ToArbitrary();
    }
}
