using FsCheck;

namespace Bearded.Utilities.Tests.Generators;

internal static class IntGenerators
{
    public static class PositiveInt
    {
        public static Arbitrary<int> Integers()
            => Arb.Default.Int32().Generator
                .Where(i => i > 0)
                .ToArbitrary();
    }
}
