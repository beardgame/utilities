using FsCheck;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Generators;

sealed class Vector3Generators
{
    public sealed class All
    {
        public static Arbitrary<Vector3> Vectors() =>
            Arb.From(FloatGenerators.ForArithmetic.Floats().Generator.Three()
                .Select(tuple => new Vector3(tuple.Item1, tuple.Item2, tuple.Item3)));
    }
}
