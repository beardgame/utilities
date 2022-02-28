using Bearded.Utilities.Geometry;
using Bearded.Utilities.Tests.Generators;
using Bearded.Utilities.Tilemaps.Rectangular;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using System;
using System.Linq;
using Xunit;

namespace Bearded.Utilities.Tests.Tilemaps;

public class ExtensionsTests
{
    public class DirectionTests
    {
        [Fact]
        public void DirectionsShouldContainAllDirections()
        {
            Extensions.Directions.Should().BeEquivalentTo(new[]
            {
                Direction.Right,
                Direction.UpRight,
                Direction.Up,
                Direction.UpLeft,
                Direction.Left,
                Direction.DownLeft,
                Direction.Down,
                Direction.DownRight,
            });
        }

        [Theory]
        [InlineData(Direction.Down, Direction.Up)]
        [InlineData(Direction.DownLeft, Direction.UpRight)]
        [InlineData(Direction.DownRight, Direction.UpLeft)]
        [InlineData(Direction.Left, Direction.Right)]
        [InlineData(Direction.Unknown, Direction.Unknown)]
        public void OppositesShouldMatch(Direction direction, Direction expected)
        {
            direction.Opposite().Should().Be(expected);
            expected.Opposite().Should().Be(direction);
        }

        [Theory]
        [InlineData(0, Direction.Right)]
        [InlineData(45, Direction.UpRight)]
        [InlineData(90, Direction.Up)]
        [InlineData(135, Direction.UpLeft)]
        [InlineData(180, Direction.Left)]
        [InlineData(225, Direction.DownLeft)]
        [InlineData(270, Direction.Down)]
        [InlineData(315, Direction.DownRight)]
        public void OrtogonalIsAsExpected(float degrees, Direction expected)
        {
            for (var delta = -22; delta < 22; delta++)
                Direction2.FromDegrees(degrees + delta).Octagonal().Should().Be(expected);
        }

        [Theory]
        [InlineData(0, Direction.Right)]
        [InlineData(90, Direction.Up)]
        [InlineData(180, Direction.Left)]
        [InlineData(270, Direction.Down)]
        public void QuadrogonalIsAsExpected(float degrees, Direction expected)
        {
            for (var delta = -45; delta < 45; delta++)
                Direction2.FromDegrees(degrees + delta).Quadrogonal().Should().Be(expected);
        }
    }
    public class DirectionsTests
    {
        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        [InlineData(Directions.All)]
        public void AnyIsTrueForAllButNone(Directions sut)
        {
            sut.Any().Should().BeTrue();
        }

        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        public void AnyMatchesForAll(Directions sut)
        {
            Directions.All.Any(sut).Should().BeTrue();
        }

        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        public void AllMatchesForAll(Directions sut)
        {
            Directions.All.All(sut).Should().BeTrue();
        }

        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        public void AllMatchesForSame(Directions sut)
        {
            sut.All(sut).Should().BeTrue();
        }

        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        public void AllDoesntMatchForNone(Directions sut)
        {
            Directions.None.All(sut).Should().BeFalse();
        }

        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        public void AnyDoesntMatchesForNone(Directions sut)
        {
            Directions.None.Any(sut).Should().BeFalse();
        }

        [Property]
        public void UnionMatchParts(Directions first, Directions second)
        {
            var union = Directions.All.Union(second);
            (union & first).Should().Be(first);
            (union & second).Should().Be(second);
        }

        [Property]
        public void ExceptDoesntMatchSame(Directions sut)
        {
            var except = Directions.All.Except(sut);
            (except & sut).Should().Be(Directions.None);
        }

        [Fact]
        public void AnyIsFalseOnNone()
        {
            Directions.None.Any().Should().BeFalse();
        }

        [Theory]
        [InlineData(Directions.Right)]
        [InlineData(Directions.UpRight)]
        [InlineData(Directions.Up)]
        [InlineData(Directions.UpLeft)]
        [InlineData(Directions.Left)]
        [InlineData(Directions.DownLeft)]
        [InlineData(Directions.Down)]
        [InlineData(Directions.DownRight)]
        [InlineData(Directions.None)]
        public void AllIsFalseForAllButAll(Directions sut)
        {
            sut.All().Should().BeFalse();
        }

        [Fact]
        public void AllIsTrueOnAll()
        {
            Directions.All.All().Should().BeTrue();
        }

    }
    public class DirectionAndDirectionsTests
    {
        [Fact]
        public void AllShouldEnumerateToAllDirections()
        {
            Directions.All.Enumerate().Should().BeEquivalentTo(Extensions.Directions);
        }

        [Fact]
        public void UnionShouldIncludeExpected()
        {
            var union = (Directions.Up | Directions.Down);
            union.Includes(Direction.Up).Should().BeTrue();
            union.Includes(Direction.Down).Should().BeTrue();
        }

        [Fact]
        public void AndShouldReturnExpected()
        {
            Directions and = Directions.Up.And(Direction.Down);
            (and & Directions.Up).Should().Be(Directions.Up);
            (and & Directions.Down).Should().Be(Directions.Down);
        }

        [Property]
        public void ExceptDoesntMatchSame(Direction sut)
        {
            Directions.All.Except(sut).Enumerate().Should().NotContain(sut);
        }
    }
}
