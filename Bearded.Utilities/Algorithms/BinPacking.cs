using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;

namespace Bearded.Utilities.Algorithms
{
    /// <summary>
    /// This class contains the functionality to efficiently pack a list of rectangles into one larger rectangle.
    /// It tries to pack them tightly and minimizes wasted space in the containing rectangle.
    /// The result is not guaranteed to be optimal.
    ///
    /// The algorithm implemented is an almost 1:1 translation of the Binary Tree Bin Packing Algorithm by
    /// Jake Gordon: http://codeincomplete.com/posts/2011/5/7/bin_packing/
    /// </summary>
    public static class BinPacking
    {
        /// <summary>
        /// Input rectangle type for the packing algorithm.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        public class Rectangle<T>
        {
            public T Value { get; }
            public int Width { get; }
            public int Height { get; }

            public Rectangle(T value, int width, int height)
            {
                Value = value;
                Width = width;
                Height = height;
            }
        }

        /// <summary>
        /// Output rectangle type of the packing algorithm.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        public class PositionedRectangle<T>
        {
            private readonly Rectangle<T> rectangle;

            public int X { get; }
            public int Y { get; }
            public T Value => rectangle.Value;
            public int Width => rectangle.Width;
            public int Height => rectangle.Height;

            internal PositionedRectangle(Rectangle<T> rectangle, int x, int y)
            {
                this.rectangle = rectangle;
                X = x;
                Y = y;
            }
        }

        /// <summary>
        /// Result container of the rectangle packing algorithm.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        public class Result<T>
        {
            /// <summary>
            /// The list of packed rectangles.
            /// </summary>
            public ReadOnlyCollection<PositionedRectangle<T>> Rectangles { get; }

            /// <summary>
            /// The width of the containing rectangle.
            /// </summary>
            public int Width { get; }

            /// <summary>
            /// The height of the containing rectangle.
            /// </summary>
            public int Height { get; }

            /// <summary>
            /// Number of pixels or grid cells of the containing rectangle wasted with this packing.
            /// </summary>
            public int EmptyPixels { get; }

            /// <summary>
            /// The total area of the containing rectangle (Width * Height).
            /// </summary>
            public int Area => Width * Height;

            /// <summary>
            /// The fraction of pixels or grid cells of the containing rectangle filled by the packed rectangles.
            /// Maximum is 1 for a perfect filling.
            /// </summary>
            public double Filled => 1 - EmptyPixels / (double)Area;

            internal Result(ReadOnlyCollection<PositionedRectangle<T>> rectangles, int width, int height, int emptyPixels)
            {
                Rectangles = rectangles;
                Width = width;
                Height = height;
                EmptyPixels = emptyPixels;
            }
        }

        /// <summary>
        /// Packs a list of rectangles together trying to minimize the size of the containing rectangle.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        /// <param name="rectangles">The rectangles to pack.</param>
        /// <returns>
        /// A result object containing the packed rectangles and some additional information.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="rectangles"/> is null.</exception>
        public static Result<T>? Pack<T>(IEnumerable<Rectangle<T>> rectangles)
            => Pack(rectangles, true);

        /// <summary>
        /// Packs a list of rectangles together trying to minimize the size of the containing rectangle.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        /// <param name="rectangles">The rectangles to pack.</param>
        /// <param name="tryMultipleHeuristics">
        /// If true, the algorithm is run for multiple heuristics and returns the best result.
        /// If false, it is only run for one heuristic, packing rectangles in order of decreasing area, which empirically works well.</param>
        /// <returns>
        /// A result object containing the packed rectangles and some additional information.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="rectangles"/> is null.</exception>
        public static Result<T>? Pack<T>(IEnumerable<Rectangle<T>> rectangles, bool tryMultipleHeuristics)
        {
            if (rectangles == null)
                throw new ArgumentNullException(nameof(rectangles));

            var asList = rectangles.ToList();

            if (asList.Count == 0)
                return null;

            asList.Sort((r1, r2) => (r2.Width * r2.Height).CompareTo(r1.Width * r1.Height));

            var result = fit(asList);

            if (!tryMultipleHeuristics)
                return result;

            var otherHeuristics = new List<Comparison<Rectangle<T>>>
            {
                (r1, r2) => r2.Width.CompareTo(r1.Width),
                (r1, r2) => r2.Height.CompareTo(r1.Height)
            };

            foreach (var h in otherHeuristics)
            {
                asList.Sort(h);

                var r = fit(asList);

                if (result.Filled < r.Filled)
                    result = r;
            }
            return result;
        }

        private static Result<T> fit<T>(IList<Rectangle<T>> blocks)
        {
            return new Tree<T>().Fit(blocks);
        }

        // See http://codeincomplete.com/posts/2011/5/7/bin_packing/
        private class Tree<T>
        {

            private class Node
            {
                private (Node Child1, Node Child2)? split;

                public int X { get; }
                public int Y { get; }
                public int W { get; }
                public int H { get; }

                public Node(int x, int y, int w, int h, (Node, Node)? split = null)
                {
                    this.split = split;
                    X = x;
                    Y = y;
                    W = w;
                    H = h;
                }

                public Node? FindNodeWithSpaceFor(int w, int h)
                {
                    if (split is var (c1, c2))
                        return c1.FindNodeWithSpaceFor(w, h) ?? c2.FindNodeWithSpaceFor(w, h);
                    if (w <= W && h <= H)
                        return this;
                    return null;
                }

                public void Split(int w, int h)
                {
                    /*
                     Splitting a node semantically means reserving a (w, h) area and
                     divvying up the remaining space as empty nodes as indicated in this diagram.

                    X,Y-----+------+
                     |   w  |      |
                     |h     |child2|
                     |      |      |
                     +------+------+
                     |    child1   |
                     +------+------+
                     */
                    split = (
                        new Node(X, Y + h, W, H - h),
                        new Node(X + w, Y, W - w, h)
                        );
                }

                public int CountEmptyPixels()
                {
                    if (split is var (down, right))
                        return down.CountEmptyPixels() + right.CountEmptyPixels();
                    return W * H;
                }
            }

            private Node root = null!;

            public Result<T> Fit(IList<Rectangle<T>> blocks)
            {
                var first = blocks[0];

                root = new Node(0, 0, first.Width, first.Height);

                var results = new List<PositionedRectangle<T>>(blocks.Count);

                foreach (var block in blocks)
                {
                    var (x, y) = fitRectangleIntoTree(block.Width, block.Height);
                    results.Add(new PositionedRectangle<T>(block, x, y));
                }

                var emptyPixels = root.CountEmptyPixels();

                return new Result<T>(results.AsReadOnly(), root.W, root.H, emptyPixels);
            }

            private (int x, int y) fitRectangleIntoTree(int width, int height)
            {
                var node = root.FindNodeWithSpaceFor(width, height);
                if (node != null)
                {
                    node.Split(width, height);
                }
                else
                {
                    node = growNode(width, height);
                }

                return (node.X, node.Y);
            }

            private Node growNode(int w, int h)
            {
                var canGrowDown = w <= root.W;
                var canGrowRight = h <= root.H;

                // We try to approximate a square which helps keep the tree relatively balanced
                // instead of growing linearly into only onw direction.
                var shouldGrowRight = canGrowRight && root.H >= root.W + w;
                var shouldGrowDown = canGrowDown && root.W >= root.H + h;

                if (shouldGrowRight)
                    return growRight(w, h);
                if (shouldGrowDown)
                    return growDown(w, h);
                if (canGrowRight)
                    return growRight(w, h);
                if (canGrowDown)
                    return growDown(w, h);

                // Our own heuristics above never violate the following constraint
                // so this exception should never be thrown.
                throw new InvalidOperationException(
                    "Was not able to grow bin packing node because we tried inserting a rectangle " +
                    "that was both wider and taller than the current packing. " +
                    "Blocks should never be supplied in such an order. " +
                    "To guarantee this constraint, rectangles should be ordered so that each of them is larger " +
                    "in either width or height than all following ones."
                );
            }

            private Node growRight(int w, int h)
            {
                /*
                 Growing right creates a new node to the right of the current root
                 as well as a new root containing both.

                0,0-----+-----------+
                 | old  | extension |
                 | root | (width w) |
                 +------+-----------+

                 We then split that extension which inserts the (w, h) rectangle and
                 free up the remaining height.
                 */

                Debug.Assert(h <= root.H);

                var r = root;
                var extension = new Node(r.W, 0, w, r.H);
                root = new Node(0, 0, r.W + w, r.H, (r, extension));
                //var node = root.FindNodeWithSpaceFor(w, h);
                var node = extension;
                node.Split(w, h);
                return node;
            }

            private Node growDown(int w, int h)
            {
                /*
                 Growing right creates a new node to the bottom of the current root
                 as well as a new root containing both.

                0,0-----------+
                 |  old root  |
                 +------------+
                 | extension  |
                 | (height h) |
                 +------------+

                 We then split that extension which inserts the (w, h) rectangle and
                 free up the remaining width.
                 */

                Debug.Assert(w <= root.W);

                var r = root;
                var extension = new Node(0, r.H, r.W, h);
                root = new Node(0, 0, r.W, r.H + h, (extension, r));
                //var node = root.FindNodeWithSpaceFor(w, h);
                var node = extension;
                node.Split(w, h);
                return node;
            }
        }
    }
}
