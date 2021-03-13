using FsCheck;

namespace Bearded.Utilities.Tests.Generators
{
    static class DoubleGenerators
    {
        public static class UnitIntervalUpperBoundExclusive
        {
            public static Arbitrary<double> Doubles()
            {
                var gen = Gen.Frequency(
                    // Make "0" return more often to ensure it's likely to be covered by each test run.
                    new WeightAndValue<Gen<double>>(1, Gen.Constant(0.0)),
                    new WeightAndValue<Gen<double>>(9,
                        Arb.Generate<uint>()
                            .Where(u => u > 0 && u < uint.MaxValue)
                            // We can't precalculate 1.0 / uint.MaxValue because the double precision is too small
                            .Select(i => (double) i / uint.MaxValue)));
                return gen.ToArbitrary();
            }
        }
    }
}
