using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace Bearded.Utilities.Monads;

public readonly struct Result<TResult, TError> : IEquatable<Result<TResult, TError>>
{
    private readonly bool isSuccess;
    [AllowNull]
    private readonly TResult result;
    [AllowNull]
    private readonly TError error;

    private Result(bool isSuccess, [AllowNull] TResult result, [AllowNull] TError error)
    {
        this.isSuccess = isSuccess;
        this.result = result;
        this.error = error;
    }

    public static Result<TResult, TError> Success(TResult result) =>
        new Result<TResult, TError>(true, result, default);

    public static Result<TResult, TError> Failure(TError error) =>
        new Result<TResult, TError>(false, default, error);

    public TResult ResultOrDefault(TResult @default) => isSuccess ? result : @default;

    public TResult ResultOrDefault(Func<TResult> defaultProvider) => isSuccess ? result : defaultProvider();

    public TResult ResultOrThrow(Func<TError, Exception> exceptionProvider) =>
        isSuccess ? result : throw exceptionProvider(error);

    public Maybe<TResult> AsMaybe() => isSuccess ? Maybe.Just(result) : Maybe.Nothing;

    public Result<TOut, TError> Select<TOut>(Func<TResult, TOut> selector) =>
        isSuccess ? (Result<TOut, TError>) Result.Success(selector(result)) : Result.Failure(error);

    public Result<TOut, TError> SelectMany<TOut>(Func<TResult, Result<TOut, TError>> selector) =>
        isSuccess ? selector(result) : Result.Failure(error);

    public void Match(Action<TResult> onSuccess)
    {
        if (isSuccess)
        {
            onSuccess(result);
        }
    }

    public void Match(Action<TResult> onSuccess, FailureResultCallback<TError> onFailure)
    {
        if (isSuccess)
        {
            onSuccess(result);
        }
        else
        {
            onFailure(error);
        }
    }

    public TOut Match<TOut>(Func<TResult, TOut> onSuccess, FailureResultTransformation<TError, TOut> onFailure) =>
        isSuccess ? onSuccess(result) : onFailure(error);

    public bool Equals(Result<TResult, TError> other) =>
        isSuccess == other.isSuccess
        && EqualityComparer<TResult>.Default.Equals(result, other.result)
        && EqualityComparer<TError>.Default.Equals(error, other.error);

    public override bool Equals(object? obj) => obj is Result<TResult, TError> other && Equals(other);

    public static bool operator ==(Result<TResult, TError> left, Result<TResult, TError> right) =>
        left.Equals(right);

    public static bool operator !=(Result<TResult, TError> left, Result<TResult, TError> right) =>
        !left.Equals(right);

    public override int GetHashCode() => HashCode.Combine(isSuccess, result, error);

    public override string ToString() => isSuccess ? $"success {result}" : $"error {error}";

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TResult, TError>(Success<TResult> success) => Success(success.Result);

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public static implicit operator Result<TResult, TError>(Failure<TError> failure) => Failure(failure.Error);
}

public static class Result
{
    public static Result<TResult, TError> Success<TResult, TError>(TResult result) =>
        Result<TResult, TError>.Success(result);

    public static Success<T> Success<T>(T result) => new Success<T>(result);

    public static Result<TResult, TError> Failure<TResult, TError>(TError error) =>
        Result<TResult, TError>.Failure(error);

    public static Failure<T> Failure<T>(T error) => new Failure<T>(error);

    public static TResult ResultOrThrow<TResult, TError>(this Result<TResult, TError> result)
        where TError : Exception
    {
        return result.ResultOrThrow(e => e);
    }
}

public readonly struct Success<T>
{
    public T Result { get; }

    internal Success(T result)
    {
        Result = result;
    }
}

public readonly struct Failure<T>
{
    public T Error { get; }

    internal Failure(T error)
    {
        Error = error;
    }
}

public delegate void FailureResultCallback<in TError>([MaybeNull] TError error);

public delegate TOut FailureResultTransformation<in TError, out TOut>([MaybeNull] TError error);