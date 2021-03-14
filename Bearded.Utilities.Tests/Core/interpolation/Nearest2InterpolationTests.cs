namespace Bearded.Utilities.Tests
{
    public sealed class Nearest2InterpolationTests : InterpolationMethod2Tests
    {
        protected override IInterpolationMethod2 Interpolation { get; } = Interpolation2.Nearest;
    }
}
