namespace Bearded.Utilities.Tests;

public sealed class Nearest2InterpolationTests : InterpolationMethod2dTests
{
    protected override IInterpolationMethod2d Interpolation => Interpolation2d.Nearest;
}
