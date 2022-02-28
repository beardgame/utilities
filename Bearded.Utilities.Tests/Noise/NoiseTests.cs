using System;
using Bearded.Utilities.Noise;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests.Noise;

public abstract class NoiseTests
{
    protected abstract IProceduralTexture CreateProceduralTexture(int seed);

    [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
    public void CannotGetValuesForNegativeXCoordinate(int seed, double x, double y)
    {
        var map = CreateProceduralTexture(seed);

        Func<double> getValueAtXMinusOne = () => map.ValueAt(x - 1.0, y);
        getValueAtXMinusOne.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
    public void CannotGetValuesForXCoordinateLargerThanOne(int seed, double x, double y)
    {
        var map = CreateProceduralTexture(seed);

        Func<double> getValueAtXPlusOne = () => map.ValueAt(x + 1.0, y);
        getValueAtXPlusOne.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
    public void CannotGetValuesForNegativeYCoordinate(int seed, double x, double y)
    {
        var map = CreateProceduralTexture(seed);

        Func<double> getValueAtYMinusOne = () => map.ValueAt(x, y - 1.0);
        getValueAtYMinusOne.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
    public void CannotGetValuesForYCoordinateLargerThanOne(int seed, double x, double y)
    {
        var map = CreateProceduralTexture(seed);

        Func<double> getValueAtYPlusOne = () => map.ValueAt(x, y + 1.0);
        getValueAtYPlusOne.Should().Throw<ArgumentOutOfRangeException>();
    }

    [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
    public void ReturnsValuesInUnitInterval(int seed, double x, double y)
    {
        var map = CreateProceduralTexture(seed);

        map.ValueAt(x, y).Should().BeInRange(0, 1);
    }

    [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
    public void IsDeterministicWithSameSeed(int seed, double x, double y)
    {
        var map1 = CreateProceduralTexture(seed);
        var map2 = CreateProceduralTexture(seed);

        map1.ValueAt(x, y).Should().BeApproximately(map2.ValueAt(x, y), double.Epsilon);
    }
}
