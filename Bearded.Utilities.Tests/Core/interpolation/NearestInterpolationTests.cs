namespace Bearded.Utilities.Tests
{
    public sealed class NearestInterpolationTests : InterpolationMethod1Tests
    {
        protected override IInterpolationMethod1 Interpolation { get; } = Interpolation1.Nearest;
    }
}
