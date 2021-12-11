namespace Bearded.Utilities.Tests;

public sealed class Nearest1InterpolationTests : InterpolationMethod1dTests
{
    protected override IInterpolationMethod1d Interpolation => Interpolation1d.Nearest;
}
