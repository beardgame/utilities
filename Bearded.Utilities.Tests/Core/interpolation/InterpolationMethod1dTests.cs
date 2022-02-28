using System;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests;

public abstract class InterpolationMethod1dTests
{
    private const double epsilon = 1e-6;

    protected abstract IInterpolationMethod1d Interpolation { get; }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
    public void ReturnsFromAtStart(double from, double to)
    {
        Interpolation.Interpolate(from, to, 0).Should().BeApproximately(from, epsilon);
    }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.NonInfiniteNonNaN)})]
    public void ReturnsToAtEnd(double from, double to)
    {
        from = 1;
        to = 0;
        Interpolation.Interpolate(from, to, 1).Should().BeApproximately(to, epsilon);
    }

    [Property(Arbitrary = new[] {typeof(DoubleGenerators.UnitIntervalBoundsInclusive)})]
    public void ReturnsValuesBetweenFromAndTo(double from, double to, double t)
    {
        Interpolation.Interpolate(from, to, t).Should().BeInRange(Math.Min(from, to), Math.Max(from, to));
    }
}
