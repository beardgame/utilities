using System;
using System.Linq;
using Bearded.Utilities.Geometry;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Noise
{
    public static class PerlinNoise
    {
        // Use uniformly distributed vectors to force better gradient distribution.
        private const int numVectorSamples = 12;
        private static readonly Vector2d[] vectorSamples = vectorSamples = Enumerable.Range(0, numVectorSamples)
            .Select(i => Direction2.FromRadians(MathConstants.TwoPi * i / numVectorSamples).Vector)
            .Select(vector => new Vector2d(vector.X, vector.Y))
            .ToArray();

        public static IProceduralTexture Generate(int numCellsX, int numCellsY, int? seed)
        {
            if (numCellsX <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numCellsX));
            }
            if (numCellsY <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(numCellsY));
            }

            var gradientArray = NoiseUtils.GenerateRandomArray(
                // We generate the corners, but to make the noise map wrap along both dimensions, we reuse the same
                // values as the left and top boundaries, so we don't have to generate the right and bottom boundaries.
                numCellsX,
                numCellsY,
                r => vectorSamples[r.Next(vectorSamples.Length)],
                seed);
            return new PerlinProceduralTexture(gradientArray);
        }

        private sealed class PerlinProceduralTexture : IProceduralTexture
        {
            private readonly Vector2d[,] gradientArray;
            private readonly int width;
            private readonly int height;

            public PerlinProceduralTexture(Vector2d[,] gradientArray)
            {
                this.gradientArray = gradientArray;
                width = gradientArray.GetLength(0);
                height = gradientArray.GetLength(1);
            }

            public double ValueAt(double x, double y)
            {
                if (x < 0 || x >= 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
                if (y < 0 || y >= 1)
                {
                    throw new ArgumentOutOfRangeException(nameof(y));
                }

                // First we transform x and y from [0, 1) x [0, 1) to [0, width) x [0, height) for easier math.
                x *= width;
                y *= height;

                // We take the floor and ceil to get the corners of the grid cell that surround the current point.
                // Note that given the wrapping of this map, xUpper and yUpper may be 0 (i.e. smaller than xLower and
                // yLower respectively). This is why xUpper and yUpper are only used as array access coordinates below.
                var xLower = (int) x;
                var xUpper = (xLower + 1) % width;

                var yLower = (int) y;
                var yUpper = (yLower + 1) % height;

                // Calculate dot products between distance and gradient for each of the grid corners
                var topLeft = dotProductWithGridDirection(xLower, yUpper, x, y);
                var topRight = dotProductWithGridDirection(xUpper, yUpper, x, y);
                var bottomLeft = dotProductWithGridDirection(xLower, yLower, x, y);
                var bottomRight = dotProductWithGridDirection(xUpper, yLower, x, y);

                // Interpolation weights
                var tx = Interpolation1.SmoothStep.Interpolate(0, 1, x - xLower);
                // TODO: why is this 1-?
                var ty = 1 - Interpolation1.SmoothStep.Interpolate(0, 1, y - yLower);

                // Dot products can be between -1 and 1, so we need to normalize before returning.
                var r = Interpolation2.BiLinear.Interpolate(bottomLeft, bottomRight, topLeft, topRight, tx, ty);
                return (r + 1) * 0.5;
            }

            private double dotProductWithGridDirection(int gridX, int gridY, double x, double y)
            {
                var distance = new Vector2d(x, y) - new Vector2d(gridX, gridY);
                return Vector2d.Dot(distance, gradientArray[gridX, gridY]);
            }
        }
    }
}
