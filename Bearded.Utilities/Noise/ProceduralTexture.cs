using System;

namespace Bearded.Utilities.Noise;

public static class ProceduralTexture
{
    public static IProceduralTexture FromArray(double[,] array, IInterpolationMethod2d interpolation)
    {
        var arrayCopy = new double[array.GetLength(0), array.GetLength(1)];
        Array.Copy(array, arrayCopy, array.Length);
        return new ArrayProceduralTexture(arrayCopy, interpolation);
    }

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
            this.array = array;

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

            scaleToWidthHeight(ref x, ref y);
            alignCoordinatesWithPixelCenters(ref x, ref y);

            var (xBelow, xAbove, yBelow, yAbove) = getGridCorners(x, y);
            var u = x - xBelow;
            var v = y - yBelow;

            xBelow = (xBelow + width) % width;
            xAbove %= width;
            yBelow = (yBelow + height) % height;
            yAbove %= height;

            return interpolation.Interpolate(
                array[xBelow, yBelow], array[xAbove, yBelow], array[xBelow, yAbove], array[xAbove, yAbove], u, v);
        }

        private void scaleToWidthHeight(ref double x, ref double y)
        {
            x *= width;
            y *= height;
        }

        private void alignCoordinatesWithPixelCenters(ref double x, ref double y)
        {
            // The texture keeps track of the values at the center of each "pixel", so for the interpolation, we
            // need to move the array coordinates by (0.5, 0.5), which is the same as moving the x, y by
            // (-0.5, -0.5).
            x -= 0.5;
            y -= 0.5;
        }

        private static (int xLower, int xUpper, int yLower, int yUpper) getGridCorners(double x, double y)
        {
            var xLower = (int) x;
            var xUpper = xLower + 1;

            var yLower = (int) y;
            var yUpper = yLower + 1;

            return (xLower, xUpper, yLower, yUpper);
        }
    }
}
