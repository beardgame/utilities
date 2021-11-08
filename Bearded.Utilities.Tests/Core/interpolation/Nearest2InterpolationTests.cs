namespace Bearded.Utilities.Tests
{
    public sealed class Nearest2InterpolationTests : InterpolationMethod2dTests
    {
        protected override IInterpolationMethod2<double, double> Interpolation => Interpolation2<double, double>.Nearest;
    }
}
