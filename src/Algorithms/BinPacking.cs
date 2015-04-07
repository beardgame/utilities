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
            private readonly T value;
            private readonly int width;
            private readonly int height;

            /// <summary>
            /// Custom user data associated with the rectangle.
            /// </summary>
            public T Value { get { return this.value; } }
            /// <summary>
            /// The width of the rectangle.
            /// </summary>
            public int Width { get { return this.width; } }
            /// <summary>
            /// The height of the rectangle.
            /// </summary>
            public int Height { get { return this.height; } }

            /// <summary>
            /// Creates a new instance of the rectangle class.
            /// </summary>
            /// <param name="value">Custom user data associated with the rectangle.</param>
            /// <param name="width">The width of the rectangle.</param>
            /// <param name="height">The height of the rectangle.</param>
            public Rectangle(T value, int width, int height)
            {
                this.value = value;
                this.width = width;
                this.height = height;
            }
        }

        /// <summary>
        /// Output rectangle type of the packing algorithm.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        public class PositionedRectangle<T>
        {
            private readonly Rectangle<T> rectangle;
            private readonly int x;
            private readonly int y;

            /// <summary>
            /// Lower X Coordinate of the rectangle.
            /// </summary>
            public int X { get { return this.x; } }
            /// <summary>
            /// Lower Y Coordinate of the rectangle.
            /// </summary>
            public int Y { get { return this.y; } }
            /// <summary>
            /// Custom user data associated with the rectangle.
            /// </summary>
            public T Value { get { return this.rectangle.Value; } }
            /// <summary>
            /// The width of the rectangle.
            /// </summary>
            public int Width { get { return this.rectangle.Width; } }
            /// <summary>
            /// The height of the rectangle.
            /// </summary>
            public int Height { get { return this.rectangle.Height; } }

            internal PositionedRectangle(Rectangle<T> rectangle, int x, int y)
            {
                this.rectangle = rectangle;
                this.x = x;
                this.y = y;
            }
        }

        /// <summary>
        /// Result container of the rectangle packing algorithm.
        /// </summary>
        /// <typeparam name="T">Type of custom user data associated with the rectangle.</typeparam>
        public class Result<T>
        {
            private readonly ReadOnlyCollection<PositionedRectangle<T>> rectangles;
            private readonly int width;
            private readonly int height;
            private readonly int emptyPixels;

            /// <summary>
            /// The list of packed rectangles.
            /// </summary>
            public ReadOnlyCollection<PositionedRectangle<T>> Rectangles { get { return this.rectangles; } }
            /// <summary>
            /// The width of the containing rectangle.
            /// </summary>
            public int Width { get { return this.width; } }
            /// <summary>
            /// The height of the containing rectangle.
            /// </summary>
            public int Height { get { return this.height; } }
            /// <summary>
            /// Number of pixels or grid cells of the containing rectangle wasted with this packing.
            /// </summary>
            public int EmptyPixels { get { return this.emptyPixels; } }

            /// <summary>
            /// The total area of the containing rectangle (Width * Height).
            /// </summary>
            public int Area { get { return this.Width * this.Height; } }
            /// <summary>
            /// The fraction of pixels or grid cells of the containing rectangle filled by the packed rectangles.
            /// Maximum is 1 for a perfect filling.
            /// </summary>
            public double Filled { get { return 1 - this.EmptyPixels / (double)this.Area; } }

            internal Result(ReadOnlyCollection<PositionedRectangle<T>> rectangles, int width, int height, int emptyPixels)
            {
                this.rectangles = rectangles;
                this.width = width;
                this.height = height;
                this.emptyPixels = emptyPixels;
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
        static public Result<T> Pack<T>(IEnumerable<Rectangle<T>> rectangles, bool tryMultipleHeuristics = true)
        {
            if(rectangles == null)
                throw new ArgumentNullException("rectangles");

            var asList = rectangles.ToList();

            if (asList.Count == 0)
            {
                return null;
            }

            asList.Sort((r1, r2) => (r2.Width * r2.Height).CompareTo(r1.Width * r1.Height));

            var result = fit(asList);

            if (!tryMultipleHeuristics)
            {
                return result;
            }

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

        /// <summary>
        /// Implementation of the algorithm.
        /// For details see http://codeincomplete.com/posts/2011/5/7/bin_packing/
        /// </summary>
        private class Tree<T>
        {

            private class Node
            {
                private Node down;
                private Node right;

                public int X { get; private set; }
                public int Y { get; private set; }
                public int W { get; private set; }
                public int H { get; private set; }

                public Node(int x, int y, int w, int h, Node down, Node right)
                {
                    this.down = down;
                    this.right = right;
                    this.X = x;
                    this.Y = y;
                    this.W = w;
                    this.H = h;
                }

                public Node(int x, int y, int w, int h)
                {
                    this.X = x;
                    this.Y = y;
                    this.W = w;
                    this.H = h;
                }

                public Node Find(int w, int h)
                {
                    if (this.right != null)
                    {
                        return this.right.Find(w, h) ?? this.down.Find(w, h);
                    }
                    if (w <= this.W && h <= this.H)
                    {
                        return this;
                    }
                    return null;
                }

                public void Split(int w, int h)
                {
                    this.down = new Node(this.X, this.Y + h, this.W, this.H - h);
                    this.right = new Node(this.X + w, this.Y, this.W - w, h);
                }

                public int GetEmptyPixels()
                {
                    if (this.right == null)
                        return this.W * this.H;
                    return this.down.GetEmptyPixels() + this.right.GetEmptyPixels();
                }
            }

            private Node root;

            public Result<T> Fit(IList<Rectangle<T>> blocks)
            {
                var first = blocks[0];

                this.root = new Node(0, 0, first.Width, first.Height);

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
                        node = this.growNode(block.Width, block.Height);
                    }
                    if (node == null)
                        throw new Exception("Oops.");
                    results.Add(new PositionedRectangle<T>(block, node.X, node.Y));
                }

                var emptyPixels = this.root.GetEmptyPixels();

                return new Result<T>(results.AsReadOnly(), this.root.W, this.root.H, emptyPixels);
            }

            private Node growNode(int w, int h)
            {
                var canGrowDown = w <= this.root.W;
                var canGrowRight = h <= this.root.H;

                var shouldGrowRight = canGrowRight && (this.root.H >= (this.root.W + w));
                var shouldGrowDown = canGrowDown && (this.root.W >= (this.root.H + h));

                if (shouldGrowRight)
                    return this.growRight(w, h);
                if (shouldGrowDown)
                    return this.growDown(w, h);
                if (canGrowRight)
                    return this.growRight(w, h);
                if (canGrowDown)
                    return this.growDown(w, h);
                return null;
            }

            private Node growRight(int w, int h)
            {
                var r = this.root;
                this.root = new Node(0, 0, r.W + w, r.H,
                    r, new Node(r.W, 0, w, r.H));
                var node = this.root.Find(w, h);
                if (node != null)
                    node.Split(w, h);
                return node;
            }

            private Node growDown(int w, int h)
            {
                var r = this.root;
                this.root = new Node(0, 0, r.W, r.H + h,
                    new Node(0, r.H, r.W, h), r);
                var node = this.root.Find(w, h);
                if (node != null)
                    node.Split(w, h);
                return node;
            }
        }
    }
}
