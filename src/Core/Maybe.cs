using System;

namespace Bearded.Utilities
{
    public struct Maybe<T>
    {
        private bool hasValue;
        private T value;

        private Maybe(T value)
        {
            hasValue = true;
            this.value = value;
        }

        public static Maybe<T> Nothing() => new Maybe<T>();

        public static Maybe<T> Just(T value) => new Maybe<T>(value);

        public T OrElse(T @default) => hasValue ? value : @default;

        public T OrThrow(Func<Exception> exceptionProvider) => hasValue ? value : throw exceptionProvider();

        public Maybe<TOut> Map<TOut>(Func<T, TOut> map) =>
            hasValue ? new Maybe<TOut>(map(value)) : Maybe<TOut>.Nothing();

        public Maybe<TOut> Map<TOut>(Func<T, Maybe<TOut>> map) => hasValue ? map(value) : Maybe<TOut>.Nothing();
    }
}
