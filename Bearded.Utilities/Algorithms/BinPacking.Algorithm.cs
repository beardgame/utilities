using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Bearded.Utilities.Algorithms;

public static partial class BinPacking
{
    private class Tree<T>
    {
        private readonly IList<Rectangle<T>> rectangles;
        private Node root;

        public Tree(IList<Rectangle<T>> rectangles)
        {
            this.rectangles = rectangles;
            var first = rectangles[0];
            root = new Node(0, 0, first.Width, first.Height);
        }

        public Result<T> Fit()
        {
            var results = new List<PositionedRectangle<T>>(rectangles.Count);

            foreach (var r in rectangles)
            {
                var (x, y) = fitRectangleIntoTree(r.Width, r.Height);
                results.Add(new PositionedRectangle<T>(r, x, y));
            }

            var emptyPixels = root.CountEmptyPixels();

            return new Result<T>(results.AsReadOnly(), root.W, root.H, emptyPixels);
        }

        // See http://codeincomplete.com/posts/2011/5/7/bin_packing/ for a full explanation of the algorithm
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
            // instead of growing linearly into only one direction.
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

            // Our own heuristics never violate the following constraint
            // so this exception should never be thrown.
            throw new InvalidOperationException(
                "Was not able to grow bin packing node because we tried inserting a rectangle " +
                "that was both wider and taller than the current packing. " +
                "Rectangles should never be supplied in such an order. " +
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
            extension.Split(w, h);
            root = new Node(0, 0, r.W + w, r.H, (r, extension));
            return extension;
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
            extension.Split(w, h);
            root = new Node(0, 0, r.W, r.H + h, (extension, r));
            return extension;
        }

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
    }
}
