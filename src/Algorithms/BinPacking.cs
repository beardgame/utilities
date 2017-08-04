using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        /// <param name="tryMultipleHeuristics">
        /// If true, the algorithm is run for multiple heuristics and returns the best result.
        /// If false, it is only run for one heuristic, packing rectangles in order of decreasing area, which empirically works well.</param>
        /// <returns>
        /// Null, if the given list of rectangles is empty.
        /// Otherwise, an object containing the packed rectangles and some additional information.
        /// </returns>
        /// <exception cref="ArgumentNullException"><paramref name="rectangles"/> is null.</exception>
        public static Result<T> Pack<T>(IEnumerable<Rectangle<T>> rectangles, bool tryMultipleHeuristics = true)
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

            var heuristics = new List<Comparison<Rectangle<T>>>
            {
                (r1, r2) => r2.Width.CompareTo(r1.Width),
                (r1, r2) => r2.Height.CompareTo(r1.Height)
            };

            foreach (var h in heuristics)
            {
                asList.Sort(h);

                var r = fit(asList);

                if (result == null || result.Filled < r.Filled)
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
                private Node down;
                private Node right;

                public int X { get; }
                public int Y { get; }
                public int W { get; }
                public int H { get; }

                public Node(int x, int y, int w, int h, Node down, Node right)
                {
                    this.down = down;
                    this.right = right;
                    X = x;
                    Y = y;
                    W = w;
                    H = h;
                }

                public Node(int x, int y, int w, int h)
                {
                    X = x;
                    Y = y;
                    W = w;
                    H = h;
                }

                public Node Find(int w, int h)
                {
                    if (right != null)
                        return right.Find(w, h) ?? down.Find(w, h);
                    if (w <= W && h <= H)
                        return this;
                    return null;
                }

                public void Split(int w, int h)
                {
                    down = new Node(X, Y + h, W, H - h);
                    right = new Node(X + w, Y, W - w, h);
                }

                public int GetEmptyPixels()
                {
                    if (right == null)
                        return W * H;
                    return down.GetEmptyPixels() + right.GetEmptyPixels();
                }
            }

            private Node root;

            public Result<T> Fit(IList<Rectangle<T>> blocks)
            {
                var first = blocks[0];

                root = new Node(0, 0, first.Width, first.Height);

                var results = new List<PositionedRectangle<T>>(blocks.Count);

                foreach (var block in blocks)
                {
                    var node = root.Find(block.Width, block.Height);
                    if (node != null)
                    {
                        node.Split(block.Width, block.Height);
                    }
                    else
                    {
                        node = growNode(block.Width, block.Height);
                    }
                    if (node == null)
                        throw new InvalidOperationException("Encountered unexpected null node.");
                    results.Add(new PositionedRectangle<T>(block, node.X, node.Y));
                }

                var emptyPixels = root.GetEmptyPixels();

                return new Result<T>(results.AsReadOnly(), root.W, root.H, emptyPixels);
            }

            private Node growNode(int w, int h)
            {
                var canGrowDown = w <= root.W;
                var canGrowRight = h <= root.H;

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
                return null;
            }

            private Node growRight(int w, int h)
            {
                var r = root;
                root = new Node(0, 0, r.W + w, r.H, r, new Node(r.W, 0, w, r.H));
                var node = root.Find(w, h);
                node?.Split(w, h);
                return node;
            }

            private Node growDown(int w, int h)
            {
                var r = root;
                root = new Node(0, 0, r.W, r.H + h, new Node(0, r.H, r.W, h), r);
                var node = root.Find(w, h);
                node?.Split(w, h);
                return node;
            }
        }
    }
}
