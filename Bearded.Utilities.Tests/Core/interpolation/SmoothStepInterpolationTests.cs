namespace Bearded.Utilities.Tests
{
    public sealed class SmoothStepInterpolationTests : InterpolationMethod1Tests
    {
        protected override IInterpolationMethod1 Interpolation { get; } = Interpolation1.SmoothStep;
    }
}
