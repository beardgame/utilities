using System;

namespace Bearded.Utilities
{
    public static class Interpolation2<T, TScalar>
        where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, TScalar, T>
        where TScalar : INumber<TScalar>
    {
        public static IInterpolationMethod2<T, TScalar> Nearest { get; } = new ComposedInterpolation(Interpolation1<T, TScalar>.Nearest);
        public static IInterpolationMethod2<T, TScalar> BiLinear { get; } = new ComposedInterpolation(Interpolation1<T, TScalar>.Linear);

        private sealed class ComposedInterpolation : IInterpolationMethod2<T, TScalar>
        {
            private readonly IInterpolationMethod1<T, TScalar> inner;

            public ComposedInterpolation(IInterpolationMethod1<T, TScalar> inner)
            {
                this.inner = inner;
            }

            public T Interpolate(
                T value00, T value10, T value01, T value11, TScalar u, TScalar v)
            {
                var valueU0 = inner.Interpolate(value00, value10, u);
                var valueU1 = inner.Interpolate(value01, value11, u);
                return inner.Interpolate(valueU0, valueU1, v);
            }
        }
    }
}
