using System;

namespace Bearded.Utilities;

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
            var clampedT = Math.Max(0, Math.Min(t, 1));
            return from * (1 - clampedT) + to * clampedT;
        }
    }

    private sealed class SmoothStepInterpolation : IInterpolationMethod1d
    {
        public double Interpolate(double from, double to, double t)
        {
            var clampedT = Math.Max(0, Math.Min(t, 1));
            var smoothT = smoothStep(clampedT);
            return from * (1 - smoothT) + to * smoothT;
        }

        private static double smoothStep(double t) => t * t * (3 - 2 * t);
    }
}
