using System;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Tests.Assertions;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class CircularArc2Tests
    {
        private const float epsilon = 0.001f;

        public CircularArc2Tests()
        {
            Arb.Register<DirectionGenerators.All>();
        }

        [Property]
        public void CreatingShortArcBetweenIdenticalDirections_ReturnsZeroLengthArc(Direction2 fromTo)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

            arc.Angle.Radians.Should().BeApproximately(0, epsilon);
        }

        [Property]
        public void CreatingShortArcBetweenOppositeDirections_ReturnsNegativeDirectionArc(Direction2 from)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, -from);

            arc.Angle.Radians.Should().BeApproximately(-MathConstants.Pi, epsilon);
        }

        [Property]
        public void CreatingShortArc_ReturnsArcWithAngleSmallerThanPiRad(Direction2 from, Direction2 to)
        {
            if (from == to) return;

            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

            arc.Angle.MagnitudeInRadians.Should().BeInRange(0, MathConstants.Pi);
        }

        [Property]
        public void CreatingLongArcBetweenIdenticalDirections_ReturnsFullLengthArc(Direction2 fromTo)
        {
            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

            arc.Angle.Radians.Should().BeApproximately(MathConstants.TwoPi, epsilon);
        }

        [Property]
        public void CreatingLongArcBetweenOppositeDirections_ReturnsPositiveDirectionArc(Direction2 from)
        {
            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, -from);

            arc.Angle.Radians.Should().BeApproximately(MathConstants.Pi, epsilon);
        }

        [Property]
        public void CreatingLongArc_ReturnsArcWithAngleLargerThanPiRad(Direction2 from, Direction2 to)
        {
            if (from == to) return;

            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

            arc.Angle.MagnitudeInRadians.Should().BeInRange(MathConstants.Pi, MathConstants.TwoPi);
        }

        [Property]
        public void CreatingArcWithAngleSmallerThanMinusTwoPi_Throws(Direction2 from)
        {
            Action action = () => CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(-7));

            action.Should().Throw<ArgumentException>();
        }

        [Property]
        public void CreatingArcWithAngleEqualsMinusTwoPi_DoesNotThrow(Direction2 from)
        {
            Action action = () => CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(-MathConstants.TwoPi));

            action.Should().NotThrow();
        }

        [Property]
        public void CreatingArcWithAngleSmallerThanMinusPi_ReturnsALongArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(-4));

            arc.IsLongArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithAngleEqualsMinusPi_ReturnsAShortArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(-MathConstants.Pi));

            arc.IsShortArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithAngleLargerThanMinusPi_ReturnsAShortArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(-2));

            arc.IsShortArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithZeroAngle_ReturnsAShortArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.Zero);

            arc.IsShortArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithAngleSmallerThanPi_ReturnsAShortArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(2));

            arc.IsShortArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithAngleEqualsPi_ReturnsALongArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(MathConstants.Pi));

            arc.IsLongArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithAngleLargerThanPi_ReturnsALongArc(Direction2 from)
        {
            var arc = CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(4));

            arc.IsLongArc.Should().BeTrue();
        }

        [Property]
        public void CreatingArcWithAngleEqualsTwoPi_Throws(Direction2 from)
        {
            Action action = () => CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(MathConstants.TwoPi));

            action.Should().Throw<ArgumentException>();
        }

        [Property]
        public void CreatingArcWithAngleLargerThanTwoPi_Throws(Direction2 from)
        {
            Action action = () => CircularArc2.FromStartAndAngle(
                Vector2.Zero, 1, from, Angle.FromRadians(7));

            action.Should().Throw<ArgumentException>();
        }

        [Property]
        public void OppositeOfALongArcIsAShortArc(Direction2 from, Direction2 to)
        {
            if (from == to) return;

            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

            var actual = arc.Opposite;

            var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);
            actual.Should().Be(expected);
        }

        [Property]
        public void OppositeOfAShortArcIsALongArc(Direction2 from, Direction2 to)
        {
            if (from == to) return;

            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

            var actual = arc.Opposite;

            var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);
            actual.Should().Be(expected);
        }

        [Property]
        public void OppositeOfAZeroLengthArcIsAFullLengthArc(Direction2 fromTo)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

            var actual = arc.Opposite;

            var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);
            actual.Should().Be(expected);
        }

        [Property]
        public void OppositeOfAFullLengthArcIsAZeroLengthArc(Direction2 fromTo)
        {
            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

            var actual = arc.Opposite;

            var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);
            actual.Should().Be(expected);
        }

        [Property]
        public void ReverseOfAShortArcIsTheShortArcWithDirectionsSwitched(Direction2 from, Direction2 to)
        {
            if (from == to) return;

            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, from, to);

            var actual = arc.Reversed;

            var expected = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, to, from);
            actual.Should().Be(expected);
        }

        [Property]
        public void ReverseOfALongArcIsTheLongArcWithDirectionsSwitched(Direction2 from, Direction2 to)
        {
            if (from == to) return;

            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, from, to);

            var actual = arc.Reversed;

            var expected = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, to, from);
            actual.Should().Be(expected);
        }

        [Property]
        public void ReverseOfAZeroLengthArcIsTheOriginalArc(Direction2 fromTo)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

            var actual = arc.Reversed;

            actual.Should().Be(arc);
        }

        [Property]
        public void ReverseOfAFullCircleArcIsTheOriginalArc(Direction2 fromTo)
        {
            var arc = CircularArc2.LongArcBetweenDirections(Vector2.Zero, 1, fromTo, fromTo);

            var actual = arc.Reversed;

            actual.Should().Be(arc);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void StartPointIsPointOnArc(Direction2 from, Direction2 to, float radius)
        {
            if (from == to) return;

            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, to);

            arc.StartPoint.LengthSquared.Should().BeApproximately(radius * radius, epsilon);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void ArcStartingAtDirectionZeroHasStartPointOnXAxis(Direction2 to, float radius)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, Direction2.Zero, to);

            arc.StartPoint.Should().BeApproximately(radius * Vector2.UnitX, epsilon);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void ArcStartingAtDirection90DegHasStartPointOnYAxis(Direction2 to, float radius)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, Direction2.FromDegrees(90), to);

            arc.StartPoint.Should().BeApproximately(radius * Vector2.UnitY, epsilon);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void ArcStartingAtDirection135HasStartPointInThirdQuadrant(Direction2 to, float radius)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(
                Vector2.Zero, radius, Direction2.FromDegrees(-135), to);

            arc.StartPoint.X.Should().BeNegative();
            arc.StartPoint.Y.Should().BeNegative();
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void EndPointIsPointOnArc(Direction2 from, Direction2 to, float radius)
        {
            if (from == to) return;

            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, to);

            arc.EndPoint.LengthSquared.Should().BeApproximately(radius * radius, epsilon);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void ArcEndingAtDirectionZeroHasEndPointOnXAxis(Direction2 from, float radius)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, Direction2.Zero);

            arc.EndPoint.Should().BeApproximately(radius * Vector2.UnitX, epsilon);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void ArcEndingAtDirection90DegHasEndPointOnYAxis(Direction2 from, float radius)
        {
            var arc =
                CircularArc2.ShortArcBetweenDirections(Vector2.Zero, radius, from, Direction2.FromDegrees(90));

            arc.EndPoint.Should().BeApproximately(radius * Vector2.UnitY, epsilon);
        }

        [Property(Arbitrary = new[] { typeof(FloatGenerators.PositiveCircleRadius) })]
        public void ArcEndingAtDirection135DegHasEndPointInThirdQuadrant(Direction2 from, float radius)
        {
            var arc = CircularArc2.ShortArcBetweenDirections(
                Vector2.Zero, radius, from, Direction2.FromDegrees(-135));

            arc.EndPoint.X.Should().BeNegative();
            arc.EndPoint.Y.Should().BeNegative();
        }
    }
}
