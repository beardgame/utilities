using System;

namespace Bearded.Utilities
{
    public static class Interpolation1<T, TScalar>
        where T : IAdditionOperators<T, T, T>, IMultiplyOperators<T, TScalar, T>
        where TScalar : INumber<TScalar>
    {
        private static readonly TScalar zero = TScalar.Zero;
        private static readonly TScalar one = TScalar.One;
        private static readonly TScalar two = TScalar.Create(2);
        private static readonly TScalar three = TScalar.Create(3);
        private static readonly TScalar oneHalf = TScalar.Create(0.5);

        public static IInterpolationMethod1<T, TScalar> Nearest { get; } = new NearestInterpolation();
        public static IInterpolationMethod1<T, TScalar> Linear { get; } = new LinearInterpolation();
        public static IInterpolationMethod1<T, TScalar> SmoothStep { get; } = new SmoothStepInterpolation();

        private sealed class NearestInterpolation : IInterpolationMethod1<T, TScalar>
        {
            public T Interpolate(T from, T to, TScalar t)
            {
                return t < oneHalf ? from : to;
            }
        }

        private sealed class LinearInterpolation : IInterpolationMethod1<T, TScalar>
        {
            public T Interpolate(T from, T to, TScalar t)
            {
                var clampedT = TScalar.Max(zero, TScalar.Min(t, one));
                return from * (one - clampedT) + to * clampedT;
            }
        }

        private sealed class SmoothStepInterpolation : IInterpolationMethod1<T, TScalar>
        {
            public T Interpolate(T from, T to, TScalar t)
            {
                var clampedT = TScalar.Max(zero, TScalar.Min(t, one));
                var smoothT = smoothStep(clampedT);
                return from * (one - smoothT) + to * smoothT;
            }

            private static TScalar smoothStep(TScalar t) => t * t * (three - two * t);
        }
    }
}
