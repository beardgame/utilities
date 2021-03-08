using System;

namespace Bearded.Utilities.Noise
{
    public static class NoiseMap
    {
        public static INoiseMap FromArray(double[,] array, IInterpolationMethod2 interpolation) =>
            new ArrayNoiseMap(array, interpolation);

        private sealed class ArrayNoiseMap : INoiseMap
        {
            private readonly double[,] array;
            private readonly IInterpolationMethod2 interpolation;

            public int Width { get; }
            public int Height { get; }

            public ArrayNoiseMap(double[,] array, IInterpolationMethod2 interpolation)
            {
                this.array = array;
                this.interpolation = interpolation;
                Width = array.GetLength(0);
                Height = array.GetLength(1);
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

                xBelow = (xBelow + Width) % Width;
                xAbove %= Width;
                yBelow = (yBelow + Height) % Height;
                yAbove %= Height;

                return interpolation.Interpolate(
                    array[xBelow, yBelow], array[xAbove, yBelow], array[xBelow, yAbove], array[xAbove, yAbove], u, v);
            }
        }
    }
}
