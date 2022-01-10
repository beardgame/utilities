using System.Linq;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests;

public abstract class InterpolationMethod2dTests
{
    private const double epsilon = 1e-6;

    protected abstract IInterpolationMethod2d Interpolation { get; }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
    public void ReturnsValue00At00(double value00, double value10, double value01, double value11)
    {
        Interpolation.Interpolate(value00, value10, value01, value11, 0, 0)
            .Should().BeApproximately(value00, epsilon);
    }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
    public void ReturnsValue10At10(double value00, double value10, double value01, double value11)
    {
        Interpolation.Interpolate(value00, value10, value01, value11, 1, 0)
            .Should().BeApproximately(value10, epsilon);
    }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
    public void ReturnsValue01At01(double value00, double value10, double value01, double value11)
    {
        Interpolation.Interpolate(value00, value10, value01, value11, 0, 1)
            .Should().BeApproximately(value01, epsilon);
    }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
    public void ReturnsValue11At11(double value00, double value10, double value01, double value11)
    {
        Interpolation.Interpolate(value00, value10, value01, value11, 1, 1)
            .Should().BeApproximately(value11, epsilon);
    }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.UnitIntervalBoundsInclusive)})]
    public void ReturnsValuesBetweenValues(double value00, double value10, double value01, double value11, double u, double v)
    {
        var min = new[] { value00, value10, value01, value11 }.Min();
        var max = new[] { value00, value10, value01, value11 }.Max();

        Interpolation.Interpolate(value00, value10, value01, value11, u, v).Should().BeInRange(min, max);
    }
}
