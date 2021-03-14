namespace Bearded.Utilities.Tests
{
    public sealed class BiLinearInterpolationTests : InterpolationMethod2Tests
    {
        protected override IInterpolationMethod2 Interpolation { get; } = Interpolation2.BiLinear;
    }
}
