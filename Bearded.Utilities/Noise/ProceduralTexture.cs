using System;

namespace Bearded.Utilities.Noise
{
    public static class ProceduralTexture
    {
        public static IProceduralTexture FromArray(double[,] array, IInterpolationMethod2d interpolation) =>
            new ArrayProceduralTexture(array, interpolation);

        private sealed class ArrayProceduralTexture : IProceduralTexture
        {
            private readonly int width;
            private readonly int height;
            private readonly double[,] array;
            private readonly IInterpolationMethod2d interpolation;

            public ArrayProceduralTexture(double[,] array, IInterpolationMethod2d interpolation)
            {
                width = array.GetLength(0);
                height = array.GetLength(1);
                this.array = new double[width, height];
                Array.Copy(array, this.array, array.Length);

                this.interpolation = interpolation;
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

                // The noise array keeps track of the values at the center of each "pixel", so for the interpolation, we
                // need to move the array coordinates by (0.5, 0.5), which is the same as moving the x, y by
                // (-0.5, -0.5).
                x -= 0.5;
                y -= 0.5;

                var xBelow = MoreMath.FloorToInt(x);
                var xAbove = MoreMath.CeilToInt(x);
                var yBelow = MoreMath.FloorToInt(y);
                var yAbove = MoreMath.CeilToInt(y);
                var u = x - xBelow;
                var v = y - yBelow;

                xBelow = (xBelow + width) % width;
                xAbove %= width;
                yBelow = (yBelow + height) % height;
                yAbove %= height;

                return interpolation.Interpolate(
                    array[xBelow, yBelow], array[xAbove, yBelow], array[xBelow, yAbove], array[xAbove, yAbove], u, v);
            }
        }
    }
}
