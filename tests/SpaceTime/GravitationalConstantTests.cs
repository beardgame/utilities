using System;
using Bearded.Utilities.SpaceTime;
using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace Bearded.Utilities.Tests.SpaceTime
{
    public class GravitationalConstantTests
    {
        public class TheNumericValueProperty
        {
            [Property]
            public void ReturnsTheValuePassedToTheConstructor(float value)
            {
                var g = new GravitationalConstant(value);

                Assert.Equal(value, g.NumericValue);
            }
        }

        public class TheZeroProperty
        {
            [Fact]
            public void ReturnsAZeroValue()
            {
                var zero = GravitationalConstant.Zero;

                Assert.Equal(0, zero.NumericValue);
            }
        }

        public class TheOneProperty
        {
            [Fact]
            public void ReturnsAZeroValue()
            {
                var one = GravitationalConstant.One;

                Assert.Equal(0, one.NumericValue);
            }
        }

        public class TheAccelerationAtDistanceWithSquaredUnitMethod
        {
            [Property]
            public void CalculatesAccelerationCorrectly(
                float gValue, float massValue, float distanceSquaredValueWithSign)
            {
                var g = new GravitationalConstant(gValue);
                var mass = new Mass(massValue);
                var distanceSquaredValue = Math.Abs(distanceSquaredValueWithSign);
                var distanceSquared = new Squared<Unit>(Math.Abs(distanceSquaredValue));
                var expectedAcceleration = gValue * massValue / distanceSquaredValue;

                var acceleration = g.AccelerationAtDistance(mass, distanceSquared);

                Assert.Equal(expectedAcceleration, acceleration.NumericValue, 5);
            }
        }

        public class TheAccelerationAtDistanceWithUnitMethod
        {
            [Property]
            public void CalculatesAccelerationCorrectly(
                float gValue, float massValue, float distanceValue)
            {
                var g = new GravitationalConstant(gValue);
                var mass = new Mass(massValue);
                var distance = new Unit(distanceValue);
                var expectedAcceleration = gValue * massValue / distanceValue.Squared();

                var acceleration = g.AccelerationAtDistance(mass, distance);

                Assert.Equal(expectedAcceleration, acceleration.NumericValue, 5);
            }
        }

        public class TheAccelerationAtDistanceWithDifference2Method
        {
            [Property]
            public void CalculatesAccelerationCorrectly(
                float gValue, float massValue, float differenceXValue, float differenceYValue)
            {
                gValue = -14.9230766f;
                massValue = -3.40282347e+38f;
                differenceXValue = -16.181818f;
                differenceYValue = -8.33333302f;

                var g = new GravitationalConstant(gValue);
                var mass = new Mass(massValue);
                var difference = new Difference2(differenceXValue, differenceYValue);
                var expectedAcceleration = gValue * massValue
                    / (differenceXValue.Squared() + differenceYValue.Squared());

                var acceleration = g.AccelerationAtDistance(mass, difference);


                var expectedAsDouble = Math.Round((double)expectedAcceleration, 5);
                var actualAsDouble = Math.Round((double)acceleration.NumericValue, 5);
                Assert.Equal(expectedAsDouble, actualAsDouble, 5);
            }
        }
    }
}
