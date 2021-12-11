using FsCheck;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Generators;

sealed class Vector2Generators
{
    public sealed class All
    {
        public static Arbitrary<Vector2> Vectors() =>
            Arb.From(FloatGenerators.ForArithmetic.Floats().Generator.Two()
                .Select(tuple => new Vector2(tuple.Item1, tuple.Item2)));
    }
}
