using FsCheck;

namespace Bearded.Utilities.Tests.Generators;

static class DoubleGenerators
{
    public static class NonInfiniteNonNaN
    {
        public static Arbitrary<double> Doubles()
            => Arb.Default.Float().Filter(d => !double.IsInfinity(d) && !double.IsNaN(d));
    }

    public static class UnitIntervalBoundsInclusive
    {
        public static Arbitrary<double> Doubles()
        {
            var gen = Gen.Frequency(
                // Make "0" and "1" return more often to ensure it's likely to be covered by each test run.
                new WeightAndValue<Gen<double>>(1, Gen.OneOf(Gen.Constant(0.0), Gen.Constant(1.0))),
                new WeightAndValue<Gen<double>>(9,
                    Gen.Choose(0, int.MaxValue).Select(i => (double) i / int.MaxValue)));
            return gen.ToArbitrary();
        }
    }

    public static class UnitIntervalUpperBoundExclusive
    {
        public static Arbitrary<double> Doubles()
        {
            var gen = Gen.Frequency(
                // Make "0" return more often to ensure it's likely to be covered by each test run.
                new WeightAndValue<Gen<double>>(1, Gen.Constant(0.0)),
                new WeightAndValue<Gen<double>>(9,
                    Gen.Choose(0, int.MaxValue - 1).Select(i => (double) i / int.MaxValue)));
            return gen.ToArbitrary();
        }
    }
}
