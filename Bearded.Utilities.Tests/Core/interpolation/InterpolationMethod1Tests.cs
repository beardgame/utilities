using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests
{
    public abstract class InterpolationMethod1Tests
    {
        protected abstract IInterpolationMethod1 Interpolation { get; }

        [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
        public void ReturnsFromAtStart(double from, double to)
        {
            Interpolation.Interpolate(from, to, 0).Should().BeApproximately(from, double.Epsilon);
        }

        [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
        public void ReturnsToAtEnd(double from, double to)
        {
            Interpolation.Interpolate(from, to, 1).Should().BeApproximately(to, double.Epsilon);
        }

        [Property(Arbitrary = new[] {typeof(DoubleGenerators.UnitIntervalBoundsInclusive)})]
        public void ReturnsValuesBetweenFromAndTo(double t)
        {
            Interpolation.Interpolate(0, 1, t).Should().BeInRange(0, 1);
        }
    }
}
