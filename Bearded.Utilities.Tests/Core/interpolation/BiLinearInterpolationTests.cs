namespace Bearded.Utilities.Tests
{
    public sealed class BiLinearInterpolationTests : InterpolationMethod2dTests
    {
        protected override IInterpolationMethod2<double, double> Interpolation => Interpolation2<double, double>.BiLinear;
    }
}
