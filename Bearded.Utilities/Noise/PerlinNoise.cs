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

        public static INoiseMap Generate(int width, int height, int? seed)
        {
            if (width == 0 || width == int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(width));
            }
            if (height == 0 || height == int.MaxValue)
            {
                throw new ArgumentOutOfRangeException(nameof(height));
            }

            var gradientArray = NoiseUtils.GenerateRandomArray(
                // Given the gradients are used as the corners of cells in the noise map, we need to generate one more
                // gradient in each direction.
                width + 1,
                height + 1,
                r => vectorSamples[r.Next(vectorSamples.Length)],
                seed);
            return new PerlinNoiseMap(gradientArray);
        }

        private sealed class PerlinNoiseMap : INoiseMap
        {
            private readonly Vector2d[,] gradientArray;

            public int Width { get; }
            public int Height { get; }

            public PerlinNoiseMap(Vector2d[,] gradientArray)
            {
                this.gradientArray = gradientArray;
                Width = gradientArray.GetLength(0) - 1;
                Height = gradientArray.GetLength(1) - 1;
            }

            public double ValueAt(double x, double y)
            {
                if (x < 0 || x >= Width)
                {
                    throw new ArgumentOutOfRangeException(nameof(x));
                }
                if (y < 0 || y >= Height)
                {
                    throw new ArgumentOutOfRangeException(nameof(y));
                }

                // Grid coordinates lower and upper
                var xLower = (int) x;
                var xUpper = xLower + 1;

                var yLower = (int) y;
                var yUpper = yLower + 1;

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
