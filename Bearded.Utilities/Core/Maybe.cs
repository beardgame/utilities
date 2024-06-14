using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Bearded.Utilities.Monads;

namespace Bearded.Utilities;

public abstract class Maybe<T> : IEquatable<Maybe<T>>
    where T : notnull
{

    public static Maybe<T> Nothing => new Nothing<T>();

    internal static Maybe<T> Just(T value) => new Just<T>(value);

    public abstract T ValueOrDefault(T @default);

    public abstract T ValueOrDefault(Func<T> defaultProvider);

    public abstract Result<T, TError> ValueOrFailure<TError>(TError error);

    public abstract Result<T, TError> ValueOrFailure<TError>(Func<TError> errorProvider);

    public abstract Maybe<TOut> Select<TOut>(Func<T, TOut> selector) where TOut : notnull;

    public abstract Maybe<TOut> SelectMany<TOut>(Func<T, Maybe<TOut>> selector) where TOut : notnull;

    public abstract Maybe<T> Where(Func<T, bool> predicate);

    public abstract void Match(Action<T> onValue);

    public abstract void Match(Action<T> onValue, Action onNothing);

    public abstract TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> onNothing);

    public abstract bool Equals(Maybe<T>? other);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Maybe<T>(NothingMaybe _) => Nothing;


}

public static class Maybe
{
    public static Maybe<T> FromNullable<T>(T? value) where T : class =>
        value == null ? Nothing<T>() : Maybe<T>.Just(value);

    public static Maybe<T> FromNullable<T>(T? value) where T : struct =>
        value.HasValue ? Maybe<T>.Just(value.Value) : Nothing<T>();

    public static Maybe<T> Just<T>(T value) where T : notnull  => Maybe<T>.Just(value);

    public static Maybe<T> Nothing<T>() where T : notnull => Maybe<T>.Nothing;
}

public readonly struct NothingMaybe
{
}


public class Nothing<T> : Maybe<T>
    where T : notnull
{
    internal Nothing()
    {
    }


    public override void Match(Action<T> _) { }

    public override void Match(Action<T> _, Action onNothing) => onNothing();
    public override TResult Match<TResult>(Func<T, TResult> _, Func<TResult> onNothing) => onNothing();


    public override T ValueOrDefault(T @default) => @default;

    public override T ValueOrDefault(Func<T> defaultProvider) => defaultProvider();

    public override Result<T, TError> ValueOrFailure<TError>(TError error) => Result.Failure(error);

    public override Result<T, TError> ValueOrFailure<TError>(Func<TError> errorProvider) => Result.Failure(errorProvider());

    public override Maybe<TOut> Select<TOut>(Func<T, TOut> selector) => Maybe<TOut>.Nothing;

    public override Maybe<TOut> SelectMany<TOut>(Func<T, Maybe<TOut>> selector) => Maybe<TOut>.Nothing;

    public override Maybe<T> Where(Func<T, bool> predicate) => Nothing;


    public override int GetHashCode() => 0;

    public override string ToString() => "nothing";
    public override bool Equals(Maybe<T>? other) => other is Nothing<T>;

    public override bool Equals(object? obj) => obj is Maybe<T> other && Equals(other);

}

public class Just<T> : Maybe<T> where T : notnull
{

    protected readonly T value;

    internal Just(T value)
    {
        this.value = value;
    }
    public override void Match(Action<T> onValue) => onValue(value!);
    public override void Match(Action<T> onValue, Action _) => onValue(value!);
    public override TResult Match<TResult>(Func<T, TResult> onValue, Func<TResult> _) => onValue(value!);

    public override T ValueOrDefault(T @default) => value!;

    public override T ValueOrDefault(Func<T> defaultProvider) => value!;

    public override Result<T, TError> ValueOrFailure<TError>(TError _) =>  Result.Success(value!);

    public override Result<T, TError> ValueOrFailure<TError>(Func<TError> _) => Result.Success(value!);

    public override Maybe<TOut> Select<TOut>(Func<T, TOut> selector) => Maybe.Just(selector(value!));

    public override Maybe<TOut> SelectMany<TOut>(Func<T, Maybe<TOut>> selector)  => selector(value!);

    public override Maybe<T> Where(Func<T, bool> predicate) => predicate(value!) ? this : Nothing;


    public override int GetHashCode() => EqualityComparer<T>.Default.GetHashCode(value!);

    public override string ToString() => $"just {value}";

    public bool Equals(Just<T> other) => EqualityComparer<T>.Default.Equals(value, other.value);
    public override bool Equals(Maybe<T>? other) => other is Just<T> just && Equals(just);
    public override bool Equals(object? obj) => obj is Maybe<T> other && Equals(other);
}
