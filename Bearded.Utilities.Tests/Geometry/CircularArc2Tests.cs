using Bearded.Utilities.Geometry;
using FluentAssertions;
using FluentAssertions.Equivalency;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class CircularArc2Tests
    {
        public sealed class Opposite
        {
            [Fact]
            public void ReturnsMinorArcIfArcIsMajor()
            {
                var arc = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, -270.Degrees());

                var actual = arc.Opposite;

                var expected = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, 90.Degrees());
                actual.Should().BeEquivalentTo(expected, circularArc2Equivalency);
            }

            [Fact]
            public void ReturnsMajorArcIfArcIsMinor()
            {
                var arc = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, 90.Degrees());

                var actual = arc.Opposite;

                var expected = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, -270.Degrees());
                actual.Should().BeEquivalentTo(expected, circularArc2Equivalency);
            }
        }

        public sealed class Reversed
        {
            [Fact]
            public void ReturnsReversedArc()
            {
                var arc = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, 90.Degrees());

                var actual = arc.Reversed;

                var expected =
                    CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.FromDegrees(90), -90.Degrees());
                actual.Should().BeEquivalentTo(expected, circularArc2Equivalency);
            }
        }

        public sealed class ShortestArcBetweenDirections
        {
            [Fact]
            public void ReturnsMinorArc()
            {
                var actual = CircularArc2.ShortestArcBetweenDirections(
                    Vector2.Zero,
                    1,
                    Direction2.FromDegrees(0),
                    Direction2.FromDegrees(90));

                var expected = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, 90.Degrees());
                actual.Should().BeEquivalentTo(expected, circularArc2Equivalency);
            }
        }

        public sealed class LongestArcBetweenDirections
        {
            [Fact]
            public void ReturnsMajorArc()
            {
                var actual = CircularArc2.LongestArcBetweenDirections(
                    Vector2.Zero,
                    1,
                    Direction2.FromDegrees(0),
                    Direction2.FromDegrees(90));

                var expected = CircularArc2.FromStartAndAngle(Vector2.Zero, 1, Direction2.Zero, -270.Degrees());
                actual.Should().BeEquivalentTo(expected, circularArc2Equivalency);
            }
        }

        private static EquivalencyAssertionOptions<CircularArc2> circularArc2Equivalency(
            EquivalencyAssertionOptions<CircularArc2> options)
        {
            // Override the comparing of circular arcs to compare by member, so that we can get approximate comparisons
            // for the angle.
            return options
                .ComparingByMembers<CircularArc2>()
                .Including(a => a.Center)
                .Including(a => a.Radius)
                .Including(a => a.Start)
                .Including(a => a.Angle)
                .Using<Angle>(ctx => ctx.Subject.Radians.Should().BeApproximately(ctx.Expectation.Radians, 0.01f))
                .WhenTypeIs<Angle>();
        }
    }
}
