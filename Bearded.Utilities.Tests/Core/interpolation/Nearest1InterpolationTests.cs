namespace Bearded.Utilities.Tests
{
    public sealed class Nearest1InterpolationTests : InterpolationMethod1Tests
    {
        protected override IInterpolationMethod1 Interpolation { get; } = Interpolation1.Nearest;
    }
}
