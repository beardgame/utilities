using System;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Tests.Assertions;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck.Xunit;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class CircularArc2Tests
    {
        public sealed class ShortArcBetweenDirections
        {
            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsZeroLengthArcIfDirectionsAreEqual(Direction2 fromTo)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

                arc.Angle.Radians.Should().BeApproximately(0, 0.01f);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsNegativeDirectionArcIfDirectionsAreOpposite(Direction2 from)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, -from);

                arc.Angle.Radians.Should().BeApproximately(-MathConstants.Pi, 0.01f);
            }
        }

        public sealed class LongArcBetweenDirections
        {
            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsFullLengthArcInPositiveDirectionIfDirectionsAreEqual(Direction2 fromTo)
            {
                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

                arc.Angle.Radians.Should().BeApproximately(MathConstants.TwoPi, 0.01f);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsPositiveDirectionArcIfDirectionsAreOpposite(Direction2 from)
            {
                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, -from);

                arc.Angle.Radians.Should().BeApproximately(MathConstants.Pi, 0.01f);
            }
        }

        public sealed class FromStartAndAngle
        {
            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ThrowsIfAngleIsSmallerThanMinusTwoPi(Direction2 from)
            {
                Action action = () => CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(-7));

                action.Should().Throw<ArgumentException>();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void DoesNotThrowIfAngleIsMinusTwoPi(Direction2 from)
            {
                Action action = () => CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(-MathConstants.TwoPi));

                action.Should().NotThrow();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsLongArcIfAngleSmallerThanMinusPi(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(-4));

                arc.IsLongArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsShortArcIfAngleEqualsMinusPi(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(-MathConstants.Pi));

                arc.IsShortArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsShortArcIfAngleIsLargerThanMinusPi(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(-2));

                arc.IsShortArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsShortArcIfAngleIsZero(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.Zero);

                arc.IsShortArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsShortArcIfAngleIsSmallerThanPi(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(2));

                arc.IsShortArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsLongArcIfAngleEqualsPi(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(MathConstants.Pi));

                arc.IsLongArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsLongArcIfAngleIsLargerThanPi(Direction2 from)
            {
                var arc = CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(4));

                arc.IsLongArc.Should().BeTrue();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ThrowsIfAngleIsTwoPi(Direction2 from)
            {
                Action action = () => CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(MathConstants.TwoPi));

                action.Should().Throw<ArgumentException>();
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void DoesNotThrowIfAngleIsLargerThanTwoPi(Direction2 from)
            {
                Action action = () => CircularArc2.FromStartAndAngle(
                    Vector2.Zero, 1, from, Bearded.Utilities.Geometry.Angle.FromRadians(7));

                action.Should().Throw<ArgumentException>();
            }
        }

        public sealed class Opposite
        {
            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsShortArcIfArcIsLong(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Opposite;

                var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsLongArcIfArcIsShort(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Opposite;

                var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsFullCircleInPositiveDirectionIfArcIsZeroLength(Direction2 fromTo)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

                var actual = arc.Opposite;

                var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsZeroLengthArcIfArcIsFullCircle(Direction2 fromTo)
            {
                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

                var actual = arc.Opposite;

                var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);
                actual.Should().Be(expected);
            }
        }

        public sealed class Reversed
        {
            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsReversedShortArcIfArcIsShort(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Reversed;

                var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, to, from);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void ReturnsReversedShortArcIfArcIsLong(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

                var actual = arc.Reversed;

                var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, to, from);
                actual.Should().Be(expected);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void IsANoOpWhenArcIsZeroLength(Direction2 fromTo)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

                var actual = arc.Reversed;

                actual.Should().Be(arc);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void IsANoOpWhenArcIsFullCircle(Direction2 fromTo)
            {
                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

                var actual = arc.Reversed;

                actual.Should().Be(arc);
            }
        }

        public sealed class Angle
        {
            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void IsSmallerThanPiRadForShortArcs(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

                arc.Angle.MagnitudeInRadians.Should().BeInRange(0, MathConstants.Pi);
            }

            [Property(Arbitrary = new[] {typeof(DirectionGenerators.All)})]
            public void IsLargerThanPiRadForShortArcs(Direction2 from, Direction2 to)
            {
                if (from == to) return;

                var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

                arc.Angle.MagnitudeInRadians.Should().BeInRange(MathConstants.Pi, MathConstants.TwoPi);
            }
        }

        public sealed class StartPoint
        {
            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsPointOnArc(Direction2 from, Direction2 to, float radius)
            {
                if (from == to) return;

                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, to);

                arc.StartPoint.LengthSquared.Should().BeApproximately(radius * radius, 0.01f);
            }

            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsCorrectPointOnXAxis(Direction2 to, float radius)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, Direction2.Zero, to);

                arc.StartPoint.Should().BeApproximately(radius * Vector2.UnitX, 0.01f);
            }

            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsCorrectPointOnYAxis(Direction2 to, float radius)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, Direction2.FromDegrees(90), to);

                arc.StartPoint.Should().BeApproximately(radius * Vector2.UnitY, 0.01f);
            }

            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsPointInCorrectQuadrant(Direction2 to, float radius)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(
                    Vector2.Zero, radius, Direction2.FromDegrees(-135), to);

                arc.StartPoint.X.Should().BeNegative();
                arc.StartPoint.Y.Should().BeNegative();
            }
        }

        public sealed class EndPoint
        {
            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsPointOnArc(Direction2 from, Direction2 to, float radius)
            {
                if (from == to) return;

                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, to);

                arc.EndPoint.LengthSquared.Should().BeApproximately(radius * radius, 0.01f);
            }

            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsCorrectPointOnXAxis(Direction2 from, float radius)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, Direction2.Zero);

                arc.EndPoint.Should().BeApproximately(radius * Vector2.UnitX, 0.01f);
            }

            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsCorrectPointOnYAxis(Direction2 from, float radius)
            {
                var arc =
                    CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, Direction2.FromDegrees(90));

                arc.EndPoint.Should().BeApproximately(radius * Vector2.UnitY, 0.01f);
            }

            [Property(Arbitrary = new[]
                {typeof(DirectionGenerators.All), typeof(FloatGenerators.PositiveCircleRadius)})]
            public void ReturnsPointInCorrectQuadrant(Direction2 from, float radius)
            {
                var arc = CircularArc2.ShortArcBetweenDirections(
                    Vector2.Zero, radius, from, Direction2.FromDegrees(-135));

                arc.EndPoint.X.Should().BeNegative();
                arc.EndPoint.Y.Should().BeNegative();
            }
        }
    }
}
