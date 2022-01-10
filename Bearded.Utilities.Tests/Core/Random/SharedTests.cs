using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
// ReSharper disable CompareOfFloatsByEqualityOperator because we mean to test equality below

namespace Bearded.Utilities.Tests.Random;

public class SharedTests
{
    private const int sequenceCount = 100;
    private const int diviationOffset = 100;
        
    private static IEnumerable<T> sequence<T>(int count, Func<T> function)
        => Enumerable.Range(0, count).Select(_ => function());

    private static List<T> list<T>(int count, Func<T> function)
        => sequence(count, function).ToList();
        
    public abstract class TheSeedMethod : BaseTests.RandomMethodWithoutParameters<int>
    {
        [Property]
        public void LeadsToPredictableResultsWithSameSeed(int seed)
        {
            Seed(seed);
            var sequence1 = list(sequenceCount, CallMethod);

            Seed(seed);
            var sequence2 = list(sequenceCount, CallMethod);

            sequence1.Should().BeEquivalentTo(sequence2,
                options => options.WithStrictOrdering());
        }
    }
        
    public abstract class TheIntMethod : BaseTests.RandomMethodWithoutParameters<int>
    {
        [Property]
        public void ShouldReturnDifferentValues(int seed)
        {
            Seed(seed);
            var first = CallMethod();

            sequence(sequenceCount, CallMethod)
                .Should().Contain(i => i != first);
        }
    }

    public abstract class TheIntWithMaxMethod : BaseTests.RandomMethodWithOneParameter<int>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, PositiveInt max)
        {
            ResultOfCallingWith(seed, max.Get)
                .Should().BeInRange(0, max.Get);
        }

        [Property]
        public void ShouldThrowForNegativeMaximum(int seed, NegativeInt max)
        {
            CallingWith(seed, max.Get).Should().Throw<ArgumentException>();
        }

        [Property]
        public void ShouldReturnZeroForZeroMaximum(int seed)
        {
            ResultOfCallingWith(seed, 0).Should().Be(0);
        }
    }

    public abstract class TheIntWithMinAndMaxMethod : BaseTests.RandomMethodWithTwoParameters<int>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, int min, int max)
        {
            if (max < min)
                (min, max) = (max, min);

            ResultOfCallingWith(seed, min, max)
                .Should().BeInRange(min, max);
        }

        [Property]
        public void ShouldThrowIfMaximumLessThanMinimum(int seed, int min, int max)
        {
            if (max == min)
                max--;
            if (max > min)
                (min, max) = (max, min);

            CallingWith(seed, min, max)
                .Should().Throw<ArgumentException>();
        }

        [Property]
        public void ShouldReturnMinimumIfMinimumEqualsMaximum(int seed, int minMax)
        {
            ResultOfCallingWith(seed, minMax, minMax)
                .Should().Be(minMax);
        }
    }
        
    public abstract class TheLongMethod : BaseTests.RandomMethodWithoutParameters<long>
    {
        [Property]
        public void ShouldReturnDifferentValues(int seed)
        {
            Seed(seed);
            var first = CallMethod();

            sequence(sequenceCount, CallMethod)
                .Should().Contain(i => i != first);
        }
    }

    public abstract class TheLongWithMaxMethod : BaseTests.RandomMethodWithOneParameter<long>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, long max)
        {
            max = Math.Max(1, max);
            ResultOfCallingWith(seed, max)
                .Should().BeInRange(0, max);
        }

        [Property]
        public void ShouldThrowForNegativeMaximum(int seed, long max)
        {
            max = Math.Min(-1, max);
            CallingWith(seed, max).Should().Throw<ArgumentException>();
        }

        [Property]
        public void ShouldReturnZeroForZeroMaximum(int seed)
        {
            ResultOfCallingWith(seed, 0).Should().Be(0);
        }
    }

    public abstract class TheLongWithMinAndMaxMethod : BaseTests.RandomMethodWithTwoParameters<long>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, long min, long max)
        {
            if (max < min)
                (min, max) = (max, min);

            ResultOfCallingWith(seed, min, max)
                .Should().BeInRange(min, max);
        }

        [Property]
        public void ShouldThrowIfMaximumLessThanMinimum(int seed, long min, long max)
        {
            if (max == min)
                max--;
            if (max > min)
                (min, max) = (max, min);

            CallingWith(seed, min, max)
                .Should().Throw<ArgumentException>();
        }

        [Property]
        public void ShouldReturnMinimumIfMinimumEqualsMaximum(int seed, long minMax)
        {
            ResultOfCallingWith(seed, minMax, minMax)
                .Should().Be(minMax);
        }
    }
        
    public abstract class TheFloatMethod : BaseTests.RandomMethodWithoutParameters<float>
    {
        [Property]
        public void ShouldReturnDifferentValues(int seed)
        {
            Seed(seed);
            var first = CallMethod();

            sequence(sequenceCount, CallMethod)
                .Should().Contain(i => i != first);
        }
            
        [Property]
        public void ShouldReturnValuesInUnitRange(int seed)
        {
            ResultOfCallingWith(seed).Should().BeInRange(0, 1);
        }
    }
        
    public abstract class TheFloatWithOneBoundMethod : BaseTests.RandomMethodWithOneParameter<float>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, NormalFloat bound)
        {
            var b = (float) bound.Get;
            var (minimumValue, maximumValue) = b > 0
                ? (0f, b)
                : (b, 0f);
                
            ResultOfCallingWith(seed, b)
                .Should().BeInRange(minimumValue, maximumValue);
        }

        [Property]
        public void ShouldReturnZeroForZeroBound(int seed)
        {
            ResultOfCallingWith(seed, 0).Should().Be(0);
        }
    }

    public abstract class TheFloatWithTwoBoundsMethod : BaseTests.RandomMethodWithTwoParameters<float>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, NormalFloat bound1, NormalFloat bound2)
        {
            var (b1, b2) = ((float)bound1.Get, (float)bound2.Get);
            var (minimumValue, maximumValue) = b1 < b2
                ? (b1, b2)
                : (b2, b1);

            ResultOfCallingWith(seed, b1, b2)
                .Should().BeInRange(minimumValue, maximumValue);
        }

        [Property]
        public void ShouldReturnBoundIfBoundsAreEqual(int seed, NormalFloat bound)
        {
            var b = (float) bound.Get;
            ResultOfCallingWith(seed, b, b).Should().Be(b);
        }
    }
    public abstract class TheDoubleMethod : BaseTests.RandomMethodWithoutParameters<double>
    {
        [Property]
        public void ShouldReturnDifferentValues(int seed)
        {
            Seed(seed);
            var first = CallMethod();

            sequence(sequenceCount, CallMethod)
                .Should().Contain(i => i != first);
        }
            
        [Property]
        public void ShouldReturnValuesInUnitRange(int seed)
        {
            ResultOfCallingWith(seed).Should().BeInRange(0, 1);
        }
    }
        
    public abstract class TheDoubleWithOneBoundMethod : BaseTests.RandomMethodWithOneParameter<double>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, NormalFloat bound)
        {
            var b = bound.Get;
            var (minimumValue, maximumValue) = b > 0
                ? (0d, b)
                : (b, 0d);
                
            ResultOfCallingWith(seed, b)
                .Should().BeInRange(minimumValue, maximumValue);
        }

        [Property]
        public void ShouldReturnZeroForZeroBound(int seed)
        {
            ResultOfCallingWith(seed, 0).Should().Be(0);
        }
    }

    public abstract class TheDoubleWithTwoBoundsMethod : BaseTests.RandomMethodWithTwoParameters<double>
    {
        [Property]
        public void ShouldReturnValueInRange(int seed, NormalFloat bound1, NormalFloat bound2)
        {
            var (b1, b2) = (bound1.Get, bound2.Get);
            var (minimumValue, maximumValue) = b1 < b2
                ? (b1, b2)
                : (b2, b1);

            ResultOfCallingWith(seed, b1, b2)
                .Should().BeInRange(minimumValue, maximumValue);
        }

        [Property]
        public void ShouldReturnBoundIfBoundsAreEqual(int seed, NormalFloat bound)
        {
            var b = bound.Get;
            ResultOfCallingWith(seed, b, b).Should().Be(b);
        }
    }

    public abstract class TheSignMethod : BaseTests.RandomMethodWithoutParameters<int>
    {
        [Property]
        public void ReturnsPlusOrMinusOne(int seed)
        {
            ResultOfCallingWith(seed).Should().BeOneOf(-1, 1);
        }

        [Property]
        public void ReturnsBothValidValues(int seed)
        {
            Seed(seed);
            sequence(sequenceCount, CallMethod)
                .Should().Contain(new []{-1, 1});
        }
    }

    public abstract class TheBoolMethod : BaseTests.RandomMethodWithoutParameters<bool>
    {
        [Property]
        public void ReturnsBothPossibleValues(int seed)
        {
            Seed(seed);
            sequence(sequenceCount, CallMethod)
                .Should().Contain(new []{true, false});
        }
    }
        
    public abstract class TheBoolWithParameterMethod : BaseTests.RandomMethodWithOneParameter<bool, double>
    {
        [Property]
        public void ReturnsOnlyFalseForZeroParameter(int seed)
        {
            ResultOfCallingWith(seed, 0).Should().Be(false);
        }
            
        [Property]
        public void ReturnsOnlyTrueForOneParameter(int seed)
        {
            ResultOfCallingWith(seed, 1).Should().Be(true);
        }
            
        [Property]
        public void ReturnsOnlyFalseForNegativeParameter(int seed, NegativeInt negativeInt)
        {
            ResultOfCallingWith(seed, negativeInt.Get).Should().Be(false);
        }
            
        [Property]
        public void ReturnsOnlyTrueForParameterLargerThanOne(int seed, PositiveInt positiveInt)
        {
            ResultOfCallingWith(seed, positiveInt.Get).Should().Be(true);
        }
            
        [Property]
        public void ReturnsBothPossibleValues(int seed, byte parameter)
        {
            // ensure 0<<p<<1 to ensure the test is very likely to pass
            var p = 0.5f + (parameter - byte.MaxValue * 0.5f) / (3f * byte.MaxValue);
            Seed(seed);
            sequence(sequenceCount, () => CallMethod(p))
                .Should().Contain(new []{true, false});
        }
    }

    public abstract class TheDiscretiseMethod : BaseTests.RandomMethodWithOneParameter<int, float>
    {
        [Property]
        public void ReturnsTheSameValueForIntegerParameters(int seed, int parameter)
        {
            ResultOfCallingWith(seed, parameter).Should().Be(parameter);
        }

        [Property]
        public void ReturnsEitherFloorOrCeilOfParameter(int seed, NormalFloat parameter)
        {
            var f = (float)parameter.Get;
            ResultOfCallingWith(seed, f)
                .Should().BeOneOf((int)Math.Floor(f), (int)Math.Ceiling(f));
        }
    }

    public abstract class TheNormalFloatMethod : BaseTests.RandomMethodWithoutParameters<float>
    {
        [Property]
        public void ReturnsDifferentValues(int seed)
        {
            Seed(seed);
            var first = CallMethod();
                
            sequence(sequenceCount, CallMethod)
                .Should().Contain(f => f != first);
        }
    }
        
    public abstract class TheNormalFloatWithParametersMethod : BaseTests.RandomMethodWithTwoParameters<float>
    {
        [Property]
        public void ReturnsValuesOtherThanMeanForNonZeroDeviation(int seed, int mean, short deviation)
        {
            var m = (float) mean;
            var d = (float) deviation + deviation > 0 ? diviationOffset : -diviationOffset;
            Seed(seed);
            sequence(sequenceCount, () => CallMethod(m, d))
                .Should().Contain(f => f != m);
        }

        [Property]
        public void ReturnsMeanForZeroDeviation(int seed, float mean)
        {
            ResultOfCallingWith(seed, mean, 0).Should().Be(mean);
        }
    }
        
    public abstract class TheNormalDoubleMethod : BaseTests.RandomMethodWithoutParameters<double>
    {
        [Property]
        public void ReturnsDifferentValues(int seed)
        {
            Seed(seed);
            var first = CallMethod();
                
            sequence(sequenceCount, CallMethod)
                .Should().Contain(d => d != first);
        }
    }
        
    public abstract class TheNormalDoubleWithParametersMethod : BaseTests.RandomMethodWithTwoParameters<double>
    {
        [Property]
        public void ReturnsValuesOtherThanMeanForNonZeroDeviation(int seed, int mean, short deviation)
        {
            var m = (double) mean;
            var d = (double) deviation + deviation > 0 ? diviationOffset : -diviationOffset;
            Seed(seed);
            sequence(sequenceCount, () => CallMethod(m, d))
                .Should().Contain(f => f != m);
        }

        [Property]
        public void ReturnsMeanForZeroDeviation(int seed, double mean)
        {
            ResultOfCallingWith(seed, mean, 0).Should().Be(mean);
        }
    }
}
