using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Algorithms;
using Xunit;

namespace Bearded.Utilities.Tests.Algorithms;

public class BinPackingTests
{
    [Fact]
    public void TestPackedRectangles_IncludesAll()
    {
        var random = new System.Random(0);
        var input = Enumerable.Range(0, 100)
            .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
            .ToList();

        var result = BinPacking.Pack(input);

        Assert.NotNull(result);
        Assert.Equal(100, result!.Rectangles.Count);

        var checkedIds = new HashSet<int>();

        foreach (var rectangle in result.Rectangles)
        {
            Assert.True(checkedIds.Add(rectangle.Value));

            Assert.Equal(rectangle.Width, input[rectangle.Value].Width);
            Assert.Equal(rectangle.Height, input[rectangle.Value].Height);
        }

        Assert.Equal(100, checkedIds.Count);
    }

    [Fact]
    public void TestPackedRectangles_NoOverlap()
    {
        var random = new System.Random(0);
        var input = Enumerable.Range(0, 100)
            .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
            .ToList();

        var result = BinPacking.Pack(input);

        Assert.NotNull(result);

        foreach (var r1 in result!.Rectangles)
        {
            foreach (var r2 in result.Rectangles)
            {
                if (r1 == r2)
                    continue;

                Assert.True(
                    r1.X + r1.Width <= r2.X ||
                    r2.X + r2.Width <= r1.X ||
                    r1.Y + r1.Height <= r2.Y ||
                    r2.Y + r2.Height <= r1.Y
                );
            }
        }
    }

    [Fact]
    public void TestPackedRectangles_CorrectResultStatistics()
    {
        var random = new System.Random(0);
        var input = Enumerable.Range(0, 100)
            .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
            .ToList();

        var result = BinPacking.Pack(input);

        Assert.NotNull(result);

        var totalArea = result!.Width * result.Height;

        Assert.Equal(totalArea, result.Area);

        var coveredPixels = 0;

        var maxX = 0;
        var maxY = 0;

        foreach (var r in result.Rectangles)
        {
            var area = r.Width * r.Height;
            coveredPixels += area;

            maxX = Math.Max(maxX, r.X + r.Width);
            maxY = Math.Max(maxY, r.Y + r.Height);
        }

        Assert.Equal(maxX, result.Width);
        Assert.Equal(maxY, result.Height);

        var emptyPixels = totalArea - coveredPixels;

        Assert.Equal(emptyPixels, result.EmptyPixels);
    }

    [Fact]
    public void TestPackedRectangles_MultipleHeuristicsEqualOrBetter()
    {
        var random = new System.Random(0);
        var input = Enumerable.Range(0, 100)
            .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
            .ToList();

        var resultSingle = BinPacking.Pack(input);
        var resultMultiple = BinPacking.PackWithMultipleHeuristics(input);

        Assert.NotNull(resultSingle);
        Assert.NotNull(resultMultiple);
        Assert.True(resultSingle!.Filled <= resultMultiple!.Filled);
    }

    [Fact]
    public void TestPackedRectangles_EmptyInputReturnsEmptyResult()
    {
        var result = BinPacking.Pack(new List<BinPacking.Rectangle<int>>());

        Assert.NotNull(result);
        Assert.Empty(result.Rectangles);
        Assert.Equal(0, result.Area);
        Assert.Equal(0, result.Width);
        Assert.Equal(0, result.Height);
        Assert.Equal(0, result.EmptyPixels);
        Assert.Equal(double.NaN, result.Filled);
    }
}
