namespace Bearded.Utilities
{
    public interface IInterpolationMethod2<T, TScalar>
    {
        public T Interpolate(T value00, T value10, T value01, T value11, TScalar u, TScalar v);
    }
}
