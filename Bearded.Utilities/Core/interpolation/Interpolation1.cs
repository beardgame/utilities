namespace Bearded.Utilities
{
    public static class Interpolation1
    {
        public static IInterpolationMethod1 Nearest { get; } = new NearestInterpolation();
        public static IInterpolationMethod1 Linear { get; } = new LinearInterpolation();
        public static IInterpolationMethod1 SmoothStep { get; } = new SmoothStepInterpolation();

        private sealed class NearestInterpolation : IInterpolationMethod1
        {
            public double Interpolate(double from, double to, double t)
            {
                return t < 0.5 ? from : to;
            }
        }

        private sealed class LinearInterpolation : IInterpolationMethod1
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

        private sealed class SmoothStepInterpolation : IInterpolationMethod1
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

    public interface IInterpolationMethod1
    {
        public double Interpolate(double from, double to, double t);
    }
}
