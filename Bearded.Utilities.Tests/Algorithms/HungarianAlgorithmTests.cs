using System;
using Bearded.Utilities.Algorithms;
using Bearded.Utilities.Tests.Generators;
using FsCheck.Xunit;
using OpenTK.Mathematics;
using Xunit;
using static System.Math;

namespace Bearded.Utilities.Tests.Algorithms;

public class HungarianAlgorithmTests
{
    [Fact]
    public void Run_EmptyCostMatrix_DoesNotFail()
    {
        HungarianAlgorithm.Run(new float[0, 0]);
    }

    [Fact]
    public void Run_EmptyCostMatrix_ReturnsEmptyResult()
    {
        Assert.Empty(HungarianAlgorithm.Run(new float[0, 0]));
    }

    [Property(Arbitrary = new [] { typeof(FloatGenerators.NonInfiniteNonNaN) })]
    public void Run_OneSource_AssignsTarget(float cost)
    {
        Assert.Equal(new[] { 0 }, HungarianAlgorithm.Run(new[,] { { cost } }));
    }

    [Fact]
    public void Run_InfiniteCost_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => HungarianAlgorithm.Run(new[,] { { float.PositiveInfinity } }));
        Assert.Throws<ArgumentException>(() => HungarianAlgorithm.Run(new[,] { { float.NegativeInfinity } }));
    }

    [Fact]
    public void Run_NaNCost_ThrowsException()
    {
        Assert.Throws<ArgumentException>(() => HungarianAlgorithm.Run(new[,] { { float.NaN } }));
    }

    [Property(Arbitrary = new[] { typeof(FloatGenerators.NonInfiniteNonNaN) })]
    public void Run_TwoSources_AssignsLowestCostTarget(float f, short s1, short s2, short s3, short s4)
    {
        // The behaviour for equal matching is not defined.
        if (Abs(s1 + s4 - s3 - s2) == 0 || Abs(f) < 1e-10 || Abs(f) > 1e10)
            return;
        var (f1, f2, f3, f4) = (f * s1, f * s2, f * s3, f * s4);

        var result = HungarianAlgorithm.Run(new[,] {{f1, f2}, {f3, f4}});

        Assert.Equal(f1 + f4 < f3 + f2 ? new[] {0, 1} : new[] {1, 0}, result);
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
