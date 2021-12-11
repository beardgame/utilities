using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bearded.Utilities.Monads;

namespace Bearded.Utilities;

public readonly struct Maybe<T> : IEquatable<Maybe<T>>
{
    private readonly bool hasValue;
    private readonly T value;

    private Maybe(T value)
    {
        hasValue = true;
        this.value = value;
    }

    public static Maybe<T> Nothing => default;

    internal static Maybe<T> Just(T value) => new Maybe<T>(value);

    public T ValueOrDefault(T @default) => hasValue ? value : @default;

    public T ValueOrDefault(Func<T> defaultProvider) => hasValue ? value : defaultProvider();

    public Result<T, TError> ValueOrFailure<TError>(TError error) =>
        hasValue ? (Result<T, TError>) Result.Success(value) : Result.Failure(error);

    public Result<T, TError> ValueOrFailure<TError>(Func<TError> errorProvider) =>
        hasValue ? (Result<T, TError>) Result.Success(value) : Result.Failure(errorProvider());

    public Maybe<TOut> Select<TOut>(Func<T, TOut> selector) =>
        hasValue ? Maybe.Just(selector(value)) : Maybe<TOut>.Nothing;

    public Maybe<TOut> SelectMany<TOut>(Func<T, Maybe<TOut>> selector) =>
        hasValue ? selector(value) : Maybe<TOut>.Nothing;

    public Maybe<T> Where(Func<T, bool> predicate) => hasValue && predicate(value) ? this : Nothing;

    public void Match(Action<T> onValue)
    {
        if (hasValue)
            onValue(value);
    }

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

    public TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> onNothing)
    {
        return hasValue ? onValue(value) : onNothing();
    }

    public bool Equals(Maybe<T> other) =>
        hasValue == other.hasValue && EqualityComparer<T>.Default.Equals(value, other.value);

    public override bool Equals(object? obj) => obj is Maybe<T> other && Equals(other);

    public override int GetHashCode() => hasValue ? EqualityComparer<T>.Default.GetHashCode(value) : 0;

    public override string ToString() => hasValue ? $"just {value}" : "nothing";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Maybe<T>(NothingMaybe _) => Nothing;
}

public static class Maybe
{
    public static Maybe<T> FromNullable<T>(T? value) where T : class =>
        value == null ? Nothing : Maybe<T>.Just(value);

    public static Maybe<T> FromNullable<T>(T? value) where T : struct =>
        value.HasValue ? Maybe<T>.Just(value.Value) : Nothing;

    public static Maybe<T> Just<T>(T value) => Maybe<T>.Just(value);

    public static NothingMaybe Nothing => default;
}

public readonly struct NothingMaybe
{
}
