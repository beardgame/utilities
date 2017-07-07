using System;
using Bearded.Utilities.Algorithms;
using FsCheck.Xunit;
using OpenTK;
using Xunit;
using static System.Math;

namespace Bearded.Utilities.Tests.Algorithms
{
    public class HungarianAlgorithmTests
    {
        [Fact]
        public void Run_EmptyCostMatrix_DoesntFail()
        {
            Assert.Empty(HungarianAlgorithm.Run(new float[0, 0]));
        }

        [Property]
        public void Run_OneSource_AssignsTarget(float cost)
        {
            if (float.IsInfinity(cost) || float.IsNaN(cost))
                Assert.Throws<ArgumentException>(() => HungarianAlgorithm.Run(new[,] {{cost}}));
            else
                Assert.Equal(new[] { 0 }, HungarianAlgorithm.Run(new[,] { { cost } }));
        }

        [Fact]
        public void Run_TwoSources_AssignsLowestCostTarget()
        {
            Assert.Equal(new[] { 0, 1 }, HungarianAlgorithm.Run(new[,] {{1f, 5f}, {5f, 1f}}));
        }

        [Fact]
        public void Run_MoreSourcesThanTargets_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() => HungarianAlgorithm.Run(
                new float[,]
                {
                    {1, 3},
                    {9, 9},
                    {3, 1}
                }
            ));
        }

        [Fact]
        public void Run_MoreTargetsThanSources_AssignsSourcesToCheapest()
        {
            Assert.Throws<ArgumentException>(() => HungarianAlgorithm.Run(
                new float[,]
                {
                    {1, 9, 3},
                    {3, 9, 1}
                }
            ));
        }

        [Fact]
        public void Run_Metric_MatchClosest()
        {
            Assert.Equal(new[] { 1, 2, 0 },
                HungarianAlgorithm.Run(new[] { -100, 0, 100 }, new[] { 111, -111, 42 }, (i1, i2) => Abs(i2 - i1)));
        }

        [Fact]
        public void Run_Vector2s_MatchClosest()
        {
            Assert.Equal(new[] {1, 0},
                HungarianAlgorithm.Run(new[] {Vector2.Zero, Vector2.UnitY}, new[] {Vector2.One, Vector2.UnitX}));
        }

        [Fact]
        public void Run_Vector3s_MathClosest()
        {
            Assert.Equal(new[] { 1, 0 },
                HungarianAlgorithm.Run(new[] { Vector3.Zero, Vector3.UnitY }, new[] { Vector3.One, Vector3.Zero }));
        }
    }
}
