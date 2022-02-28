namespace Bearded.Utilities.Tests;

public sealed class LinearInterpolationTests : InterpolationMethod1dTests
{
    protected override IInterpolationMethod1d Interpolation => Interpolation1d.Linear;
}
