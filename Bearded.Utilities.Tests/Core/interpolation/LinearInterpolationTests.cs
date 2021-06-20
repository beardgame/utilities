namespace Bearded.Utilities.Tests
{
    public sealed class LinearInterpolationTests : InterpolationMethod1Tests
    {
        protected override IInterpolationMethod1 Interpolation { get; } = Interpolation1.Linear;
    }
}
