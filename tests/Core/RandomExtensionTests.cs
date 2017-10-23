using System;
using FsCheck.Xunit;
using Xunit;

namespace Bearded.Utilities.Tests.Core
{
    public class RandomExtensionTests
    {
        [Property]
        public void NextLong(int seed)
        {
            var random = new Random(seed);

            random.NextLong();
        }

        [Property]
        public void NextLongWithMax(int seed, long max)
        {
            var random = new Random(seed);

            var number = random.NextLong(max);
            
            Assert.InRange(number, 0, max - 1);
        }

        [Property]
        public void NextLongWithMinAndMax(int seed, long min, long max)
        {
            var random = new Random(seed);

            var number = random.NextLong(min, max);

            Assert.InRange(number, min, max - 1);
        }

        [Property]
        public void NextDoubleWithMax(int seed, double max)
        {
            var random = new Random(seed);

            var number = random.NextDouble(max);

            Assert.InRange(number, 0, max);
            Assert.NotEqual(max, number);
        }

        [Property]
        public void NextDoubleWithMinAndMax(int seed, double min, double max)
        {
            var random = new Random(seed);

            var number = random.NextDouble(min, max);

            Assert.InRange(number, 0, max);
            Assert.NotEqual(max, number);
        }

        [Property]
        public void NextFloat(int seed)
        {
            var random = new Random(seed);

            random.NextFloat();
        }

        [Property]
        public void NextFloatWithMax(int seed, float max)
        {
            var random = new Random(seed);

            var number = random.NextFloat(max);

            Assert.InRange(number, 0, max);
            Assert.NotEqual(max, number);
        }

        [Property]
        public void NextFloatWithMinAndMax(int seed, float min, float max)
        {
            var random = new Random(seed);

            var number = random.NextFloat(min, max);

            Assert.InRange(number, 0, max);
            Assert.NotEqual(max, number);
        }

        [Property]
        public void NextSign(int seed)
        {
            var random = new Random(seed);

            var sign = random.NextSign();

            Assert.Equal(1, Math.Abs(sign));
        }

        [Property]
        public void NextBool(int seed)
        {
            var random = new Random(seed);

            random.NextBool();
        }

        [Property]
        public void NextBoolWithPropabilityZero(int seed)
        {
            var random = new Random(seed);

            var b = random.NextBool(0);
            
            Assert.Equal(false, b);
        }

        [Property]
        public void NextBoolWithPropabilityOne(int seed)
        {
            var random = new Random(seed);

            var b = random.NextBool(1);

            Assert.Equal(true, b);
        }

        [Property]
        public void DiscretiseWithArbitraryValue(int seed, float value)
        {
            var random = new Random(seed);

            var number = random.Discretise(value);
            
            Assert.InRange(number, (int)value, (int)value + 1);
        }

        [Property]
        public void DiscretiseWithRoundValue(int seed, int value)
        {
            var random = new Random(seed);

            var number = random.Discretise(value);

            Assert.Equal(value, number);
        }
    }
}
