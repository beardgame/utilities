using System;
using Bearded.Utilities.SpaceTime;
using Bearded.Utilities.Tests.Helpers;
using FluentAssertions;
using FsCheck.Xunit;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.SpaceTime;

public class GravitationalConstantTests
{
    public class TheNumericValueProperty
    {
        [Property]
        public void ReturnsTheValuePassedToTheConstructor(float value)
        {
            var g = new GravitationalConstant(value);

            g.NumericValue.Should().Be(value);
        }
    }

    public class TheZeroProperty
    {
        [Fact]
        public void ReturnsAZeroValue()
        {
            var zero = GravitationalConstant.Zero;

            zero.NumericValue.Should().Be(0);
        }
    }

    public class TheOneProperty
    {
        [Fact]
        public void ReturnsAZeroValue()
        {
            var one = GravitationalConstant.One;

            one.NumericValue.Should().Be(1);
        }
    }

    public class TheAccelerationTowardsWithDifference2Method
    {
        [Property(Arbitrary = new []{typeof(LimitedRangeFloatGenerator)})]
        public void CalculatesAccelerationCorrectly(
            float gValue, float massValue, float differenceXValue, float differenceYValue)
        {
            var g = new GravitationalConstant(gValue);
            var mass = new Mass(massValue);
            var difference = new Difference2(differenceXValue, differenceYValue);
            var expectedAcceleration = gValue * massValue
                / new Vector2(differenceXValue, differenceYValue).LengthSquared
                * new Vector2(differenceXValue, differenceYValue).Normalized();

            var acceleration = g.AccelerationTowards(mass, difference);

            acceleration.NumericValue.X.Should()
                .BeApproximatelyOrBothNaNOrInfinity(expectedAcceleration.X);
            acceleration.NumericValue.Y.Should()
                .BeApproximatelyOrBothNaNOrInfinity(expectedAcceleration.Y);
        }
    }

    public class TheAccelerationTowardsWithUnitMethod
    {
        [Property(Arbitrary = new []{typeof(LimitedRangeFloatGenerator)})]
        public void CalculatesAccelerationCorrectly(
            float gValue, float massValue, float differenceValue)
        {
            var g = new GravitationalConstant(gValue);
            var mass = new Mass(massValue);
            var difference = new Unit(differenceValue);
            var expectedAcceleration = gValue * massValue
                / differenceValue.Squared()
                * Math.Sign(differenceValue);

            var acceleration = g.AccelerationTowards(mass, difference);

            acceleration.NumericValue.Should()
                .BeApproximatelyOrBothNaNOrInfinity(expectedAcceleration);
        }
    }

    public class TheAccelerationAtDistanceWithSquaredUnitMethod
    {
        [Property(Arbitrary = new []{typeof(LimitedRangeFloatGenerator)})]
        public void CalculatesAccelerationCorrectly(
            float gValue, float massValue, float distanceSquaredValueWithSign)
        {
            var g = new GravitationalConstant(gValue);
            var mass = new Mass(massValue);
            var distanceSquaredValue = Math.Abs(distanceSquaredValueWithSign);
            var distanceSquared = new Squared<Unit>(distanceSquaredValue);
            var expectedAcceleration = gValue * massValue / distanceSquaredValue;

            var acceleration = g.AccelerationAtDistance(mass, distanceSquared);

            acceleration.NumericValue.Should()
                .BeApproximatelyOrBothNaNOrInfinity(expectedAcceleration);
        }
    }

    public class TheAccelerationAtDistanceWithUnitMethod
    {
        [Property(Arbitrary = new []{typeof(LimitedRangeFloatGenerator)})]
        public void CalculatesAccelerationCorrectly(
            float gValue, float massValue, float distanceValue)
        {
            var g = new GravitationalConstant(gValue);
            var mass = new Mass(massValue);
            var distance = new Unit(distanceValue);
            var expectedAcceleration = gValue * massValue / distanceValue.Squared();

            var acceleration = g.AccelerationAtDistance(mass, distance);

            acceleration.NumericValue.Should()
                .BeApproximatelyOrBothNaNOrInfinity(expectedAcceleration);
        }
    }

    public class TheAccelerationAtDistanceWithDifference2Method
    {
        [Property(Arbitrary = new []{typeof(LimitedRangeFloatGenerator)})]
        public void CalculatesAccelerationCorrectly(
            float gValue, float massValue, float differenceXValue, float differenceYValue)
        {
            var g = new GravitationalConstant(gValue);
            var mass = new Mass(massValue);
            var difference = new Difference2(differenceXValue, differenceYValue);
            var expectedAcceleration = gValue * massValue
                / new Vector2(differenceXValue, differenceYValue).LengthSquared;

            var acceleration = g.AccelerationAtDistance(mass, difference);

            acceleration.NumericValue.Should()
                .BeApproximatelyOrBothNaNOrInfinity(expectedAcceleration);
        }
    }
}
