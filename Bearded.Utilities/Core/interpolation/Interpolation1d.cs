namespace Bearded.Utilities
{
    public static class Interpolation1d
    {
        public static IInterpolationMethod1d Nearest { get; } = new NearestInterpolation();
        public static IInterpolationMethod1d Linear { get; } = new LinearInterpolation();
        public static IInterpolationMethod1d SmoothStep { get; } = new SmoothStepInterpolation();

        private sealed class NearestInterpolation : IInterpolationMethod1d
        {
            public double Interpolate(double from, double to, double t)
            {
                return t < 0.5 ? from : to;
            }
        }

        private sealed class LinearInterpolation : IInterpolationMethod1d
        {
            public double Interpolate(double from, double to, double t)
            {
                if (t <= 0)
                {
                    return from;
                }
                if (t >= 1)
                {
                    return to;
                }

                return from + (to - from) * t;
            }
        }

        private sealed class SmoothStepInterpolation : IInterpolationMethod1d
        {
            public double Interpolate(double from, double to, double t)
            {
                if (t <= 0)
                {
                    return from;
                }
                if (t >= 1)
                {
                    return to;
                }

                return (2 * t - 3) * t * t * (from - to) + from;
            }
        }
    }
}
