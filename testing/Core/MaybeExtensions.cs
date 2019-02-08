namespace Bearded.Utilities.Testing
{
    public static class MaybeExtensions
    {
        public static MaybeAssertions<T> Should<T>(Maybe<T> instance) => new MaybeAssertions<T>(instance);
    }
}
