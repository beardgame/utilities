namespace Bearded.Utilities
{
    public interface IInterpolationMethod1<T, TScalar>
    {
        public T Interpolate(T from, T to, TScalar t);
    }
}
