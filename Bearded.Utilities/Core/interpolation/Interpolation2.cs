namespace Bearded.Utilities
{
    public static class Interpolation2
    {
        public static IInterpolationMethod2 Nearest { get; } = new ExtendedInterpolation(Interpolation1.Nearest);
        public static IInterpolationMethod2 BiLinear { get; } = new ExtendedInterpolation(Interpolation1.Linear);

        private sealed class ExtendedInterpolation : IInterpolationMethod2
        {
            private readonly IInterpolationMethod1 inner;

            public ExtendedInterpolation(IInterpolationMethod1 inner)
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

    public interface IInterpolationMethod2
    {
        public double Interpolate(double value00, double value10, double value01, double value11, double u, double v);
    }
}
