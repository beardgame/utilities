using Bearded.Utilities.Geometry;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class CircularArc2Tests
    {
        public sealed class Opposite
        {
            [Property(Arbitrary = new [] { typeof(DirectionGenerators.All) })]
            public void ReturnsShortArcIfArcIsLong(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Opposite;

                var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new [] { typeof(DirectionGenerators.All) })]
            public void ReturnsLongArcIfArcIsShort(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Opposite;

                var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);
                actual.Should().Be(expected);
            }
        }

        public sealed class Reversed
        {
            [Property(Arbitrary = new [] { typeof(DirectionGenerators.All) })]
            public void ReturnsReversedShortArcIfArcIsShort(Direction2 from, Direction2 to)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Reversed;

                var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, to, from);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new [] { typeof(DirectionGenerators.All) })]
            public void ReturnsReversedShortArcIfArcIsLong(Direction2 from, Direction2 to)
            {
                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Reversed;

                var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, to, from);
                actual.Should().Be(expected);
            }
        }
    }
}
