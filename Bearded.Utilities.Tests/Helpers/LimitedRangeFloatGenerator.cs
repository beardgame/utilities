using System;
using FsCheck;

namespace Bearded.Utilities.Tests.Helpers;

public static class LimitedRangeFloatGenerator
{
    public static Arbitrary<float> Generate()
    {
        return Arb.Default.Float32()
            .Filter(f => Math.Abs(f) < 1e20 && Math.Abs(f) > 1e-20);
    }
}
