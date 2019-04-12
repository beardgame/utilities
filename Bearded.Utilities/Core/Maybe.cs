using System;
using System.Collections.Generic;

namespace Bearded.Utilities
{
    public struct Maybe<T> : IEquatable<Maybe<T>>
    {
        private readonly bool hasValue;
        private readonly T value;

        private Maybe(T value)
        {
            hasValue = true;
            this.value = value;
        }

        internal static Maybe<T> Nothing() => new Maybe<T>();

        internal static Maybe<T> Just(T value) => new Maybe<T>(value);

        public T ValueOrDefault(T @default) => hasValue ? value : @default;

        public Maybe<TOut> Select<TOut>(Func<T, TOut> selector) =>
            hasValue ? Maybe.Just(selector(value)) : Maybe.Nothing<TOut>();

        public Maybe<TOut> SelectMany<TOut>(Func<T, Maybe<TOut>> selector) =>
            hasValue ? selector(value) : Maybe.Nothing<TOut>();

        public Maybe<T> Where(Func<T, bool> predicate) => hasValue && predicate(value) ? this : Nothing();

        public void Match(Action<T> onValue, Action onNothing)
        {
            if (hasValue)
            {
                onValue(value);
            }
            else
            {
                onNothing();
            }
        }

        public bool Equals(Maybe<T> other) =>
            hasValue == other.hasValue && EqualityComparer<T>.Default.Equals(value, other.value);

        public override bool Equals(object obj) => obj is Maybe<T> other && Equals(other);

        public override int GetHashCode() => hasValue ? EqualityComparer<T>.Default.GetHashCode(value) : 0;

        public override string ToString() => hasValue ? $"just {value}" : "nothing";
    }

    public static class Maybe
    {
        public static Maybe<T> FromNullable<T>(T value) where T : class =>
            value == null ? Maybe<T>.Nothing() : Maybe<T>.Just(value);

        public static Maybe<T> FromNullable<T>(T? value) where T : struct =>
            value.HasValue ? Maybe<T>.Just(value.Value) : Maybe<T>.Nothing();

        public static Maybe<T> Just<T>(T value) => Maybe<T>.Just(value);

        public static Maybe<T> Nothing<T>() => Maybe<T>.Nothing();
    }
}
