namespace Bearded.Utilities;

public static class Interpolation2d
{
    public static IInterpolationMethod2d Nearest { get; } = new ComposedInterpolation(Interpolation1d.Nearest);
    public static IInterpolationMethod2d BiLinear { get; } = new ComposedInterpolation(Interpolation1d.Linear);

    private sealed class ComposedInterpolation : IInterpolationMethod2d
    {
        private readonly IInterpolationMethod1d inner;

        public ComposedInterpolation(IInterpolationMethod1d inner)
        {
            this.inner = inner;
        }

        public double Interpolate(
            double value00, double value10, double value01, double value11, double u, double v)
        {
            var valueU0 = inner.Interpolate(value00, value10, u);
            var valueU1 = inner.Interpolate(value01, value11, u);
            return inner.Interpolate(valueU0, valueU1, v);
        }
    }
}
