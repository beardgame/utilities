namespace Bearded.Utilities.Tests;

public sealed class BiLinearInterpolationTests : InterpolationMethod2dTests
{
    protected override IInterpolationMethod2d Interpolation => Interpolation2d.BiLinear;
}
