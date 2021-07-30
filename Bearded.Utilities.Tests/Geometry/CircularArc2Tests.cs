using Bearded.Utilities.Geometry;
using FluentAssertions;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class CircularArc2Tests
    {
        public sealed class Opposite
        {
            [Fact]
            public void ReturnsShortArcIfArcIsLong()
            {
                var arc = CircularArc2.LongArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(0), Direction2.FromDegrees(90));

                var actual = arc.Opposite;

                var expected = CircularArc2.ShortArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(0), Direction2.FromDegrees(90));
                actual.Should().Be(expected);
            }

            [Fact]
            public void ReturnsLongArcIfArcIsShort()
            {
                var arc = CircularArc2.ShortArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(0), Direction2.FromDegrees(90));

                var actual = arc.Opposite;

                var expected = CircularArc2.LongArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(0), Direction2.FromDegrees(90));
                actual.Should().Be(expected);
            }
        }

        public sealed class Reversed
        {
            [Fact]
            public void ReturnsReversedShortArcIfArcIsShort()
            {
                var arc = CircularArc2.ShortArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(0), Direction2.FromDegrees(90));

                var actual = arc.Reversed;

                var expected = CircularArc2.ShortArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(90), Direction2.FromDegrees(0));
                actual.Should().Be(expected);
            }

            [Fact]
            public void ReturnsReversedShortArcIfArcIsLong()
            {
                var arc = CircularArc2.LongArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(0), Direction2.FromDegrees(90));

                var actual = arc.Reversed;

                var expected = CircularArc2.LongArcBetweenDirections(
                    Vector2.Zero, 1, Direction2.FromDegrees(90), Direction2.FromDegrees(0));
                actual.Should().Be(expected);
            }
        }
    }
}
