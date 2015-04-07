using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Algorithms;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bearded.Utilities.Tests.Algorithms
{
    [TestClass]
    public sealed class BinPackingTests
    {
        [TestMethod]
        public void TestPackedRectangles_IncludesAll()
        {
            var random = new Random(0);
            var input = Enumerable.Range(0, 100)
                .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
                .ToList();

            var result = BinPacking.Pack(input);

            Assert.AreEqual(100, result.Rectangles.Count);

            var checkedIds = new HashSet<int>();

            foreach (var rectangle in result.Rectangles)
            {
                Assert.IsTrue(checkedIds.Add(rectangle.Value));

                Assert.AreEqual(rectangle.Width, input[rectangle.Value].Width);
                Assert.AreEqual(rectangle.Height, input[rectangle.Value].Height);
            }

            Assert.AreEqual(100, checkedIds.Count);
        }

        [TestMethod]
        public void TestPackedRectangles_NoOverlap()
        {
            var random = new Random(0);
            var input = Enumerable.Range(0, 100)
                .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
                .ToList();

            var result = BinPacking.Pack(input);

            foreach (var r1 in result.Rectangles)
            {
                foreach (var r2 in result.Rectangles)
                {
                    if (r1 == r2)
                        continue;

                    Assert.IsTrue(
                        r1.X + r1.Width <= r2.X ||
                        r2.X + r2.Width <= r1.X ||
                        r1.Y + r1.Height <= r2.Y ||
                        r2.Y + r2.Height <= r1.Y
                        );
                }
            }
        }

        [TestMethod]
        public void TestPackedRectangles_CorrectResultStatistics()
        {
            var random = new Random(0);
            var input = Enumerable.Range(0, 100)
                .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
                .ToList();

            var result = BinPacking.Pack(input);

            var totalArea = result.Width * result.Height;

            Assert.AreEqual(totalArea, result.Area);

            var coveredPixels = 0;

            var maxX = 0;
            var maxY = 0;
            
            foreach (var r in result.Rectangles)
            {
                var area = r.Width * r.Height;
                coveredPixels += area;

                maxX = System.Math.Max(maxX, r.X + r.Width);
                maxY = System.Math.Max(maxY, r.Y + r.Height);
            }

            Assert.AreEqual(maxX, result.Width);
            Assert.AreEqual(maxY, result.Height);

            var emptyPixels = totalArea - coveredPixels;

            Assert.AreEqual(emptyPixels, result.EmptyPixels);
        }

        [TestMethod]
        public void TestPackedRectangles_MultipleHeuristicsEqualOrBetter()
        {
            var random = new Random(0);
            var input = Enumerable.Range(0, 100)
                .Select(i => new BinPacking.Rectangle<int>(i, random.Next(5, 20), random.Next(5, 20)))
                .ToList();

            var resultSingle = BinPacking.Pack(input, false);
            var resultMultiple = BinPacking.Pack(input, true);

            Assert.IsTrue(resultSingle.Filled <= resultMultiple.Filled);
        }

        [TestMethod]
        public void TestPackedRectangles_EmptyInputReturnsNull()
        {
            var result = BinPacking.Pack(new List<BinPacking.Rectangle<int>>());
            
            Assert.IsNull(result);
        }
    }
}
