using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests.Core.Random
{
    public class SharedTests
    {
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
                var sequence1 = list(100, CallMethod);

                Seed(seed);
                var sequence2 = list(100, CallMethod);

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

                sequence(100, CallMethod)
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
        
        public abstract class TheFloatMethod : BaseTests.RandomMethodWithoutParameters<float>
        {
            [Property]
            public void ShouldReturnDifferentValues(int seed)
            {
                Seed(seed);
                var first = CallMethod();

                // ReSharper disable once CompareOfFloatsByEqualityOperator
                sequence(100, CallMethod)
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
            public void ShouldReturnValueInRange(int seed, float bound)
            {
                var (minimumValue, maximumValue) = bound > 0
                    ? (0f, bound)
                    : (bound, 0f);
                
                ResultOfCallingWith(seed, bound)
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
            public void ShouldReturnValueInRange(int seed, int bound1, int bound2)
            {
                var (minimumValue, maximumValue) = bound1 < bound2
                    ? (bound1, bound2)
                    : (bound2, bound1);

                ResultOfCallingWith(seed, bound1, bound2)
                    .Should().BeInRange(minimumValue, maximumValue);
            }

            [Property]
            public void ShouldReturnBoundIfBoundsAreEqual(int seed, int bound)
            {
                ResultOfCallingWith(seed, bound, bound)
                    .Should().Be(bound);
            }
        }
        public abstract class TheDoubleMethod : BaseTests.RandomMethodWithoutParameters<double>
        {
            [Property]
            public void ShouldReturnDifferentValues(int seed)
            {
                Seed(seed);
                var first = CallMethod();

                // ReSharper disable once CompareOfDoublesByEqualityOperator
                sequence(100, CallMethod)
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
            public void ShouldReturnValueInRange(int seed, double bound)
            {
                var (minimumValue, maximumValue) = bound > 0
                    ? (0d, bound)
                    : (bound, 0d);
                
                ResultOfCallingWith(seed, bound)
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
            public void ShouldReturnValueInRange(int seed, int bound1, int bound2)
            {
                var (minimumValue, maximumValue) = bound1 < bound2
                    ? (bound1, bound2)
                    : (bound2, bound1);

                ResultOfCallingWith(seed, bound1, bound2)
                    .Should().BeInRange(minimumValue, maximumValue);
            }

            [Property]
            public void ShouldReturnBoundIfBoundsAreEqual(int seed, int bound)
            {
                ResultOfCallingWith(seed, bound, bound)
                    .Should().Be(bound);
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
                sequence(100, CallMethod)
                    .Should().Contain(new []{-1, 1});
            }
        }

        public abstract class TheBoolMethod : BaseTests.RandomMethodWithoutParameters<bool>
        {
            [Property]
            public void ReturnsBothPossibleValues(int seed)
            {
                Seed(seed);
                sequence(100, CallMethod)
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
            public void ReturnsBothPossibleValues(int seed)
            {
                Seed(seed);
                sequence(100, () => CallMethod(0.5f))
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
            public void ReturnsEitherFloorOrCeilOfParameter(int seed, float parameter)
            {
                ResultOfCallingWith(seed, parameter)
                    .Should().BeOneOf((int)Math.Floor(parameter), (int)Math.Ceiling(parameter));
            }
        }
    }
}
