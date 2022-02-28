namespace Bearded.Utilities.Tests;

public sealed class SmoothStepInterpolationTests : InterpolationMethod1dTests
{
    protected override IInterpolationMethod1d Interpolation => Interpolation1d.SmoothStep;
}
