using Bearded.Utilities.Noise;
using FluentAssertions;
using FsCheck.Xunit;

namespace Bearded.Utilities.Tests.Noise
{
    public sealed class PerlinNoiseTests
    {
        [Property]
        public void GeneratesMapWithRightDimensions(int w, int h, int seed)
        {
            ensurePositive(ref w);
            ensurePositive(ref h);

            var map = PerlinNoise.Generate(w, h, seed);

            map.Width.Should().Be(w);
            map.Height.Should().Be(h);
        }

        [Property]
        public void GeneratesMapWithValuesInUnitInterval(int seed)
        {
            var map = PerlinNoise.Generate(5, 5, seed);

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    map.ValueAt(i, j).Should().BeInRange(0, 1);
                }
            }
        }

        [Property]
        public void GeneratesSameMapIfSeedUnchanged(int seed)
        {
            var map1 = PerlinNoise.Generate(5, 5, seed);
            var map2 = PerlinNoise.Generate(5, 5, seed);

            for (var i = 0; i < 5; i++)
            {
                for (var j = 0; j < 5; j++)
                {
                    map1.ValueAt(i, j).Should().BeApproximately(map2.ValueAt(i, j), double.Epsilon);
                }
            }
        }

        private static void ensurePositive(ref int i)
        {
            if (i < 0)
            {
                i = -i;
            }
            else if (i == 0)
            {
                i++;
            }
        }
    }
}
