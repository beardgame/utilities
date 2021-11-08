namespace Bearded.Utilities.Tests
{
    public sealed class Nearest1InterpolationTests : InterpolationMethod1dTests
    {
        protected override IInterpolationMethod1<double, double> Interpolation => Interpolation1<double, double>.Nearest;
    }
}
