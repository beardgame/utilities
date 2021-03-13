using System;
using Bearded.Utilities.Noise;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests.Noise
{
    public sealed class PerlinNoiseTests
    {
        [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
        public void GeneratesMapThatThrowsIfCoordinatesTooSmall(int seed, double x, double y)
        {
            var map = PerlinNoise.Generate(5, 5, seed);

            Func<double> action = () => map.ValueAt(x - 1, y - 1);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
        public void GeneratesMapThatThrowsIfCoordinatesTooBig(int seed, double x, double y)
        {
            var map = PerlinNoise.Generate(5, 5, seed);

            Func<double> action = () => map.ValueAt(x + 1, y + 1);
            action.Should().Throw<ArgumentOutOfRangeException>();
        }

        [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
        public void GeneratesMapWithValuesInUnitInterval(int seed, double x, double y)
        {
            var map = PerlinNoise.Generate(5, 5, seed);

            map.ValueAt(x, y).Should().BeInRange(0, 1);
        }

        [Property(Arbitrary = new[] { typeof(DoubleGenerators.UnitIntervalUpperBoundExclusive) })]
        public void GeneratesSameMapIfSeedUnchanged(int seed, double x, double y)
        {
            var map1 = PerlinNoise.Generate(5, 5, seed);
            var map2 = PerlinNoise.Generate(5, 5, seed);

            map1.ValueAt(x, y).Should().BeApproximately(map2.ValueAt(x, y), double.Epsilon);
        }
    }
}
