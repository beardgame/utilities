using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Bearded.Utilities.Monads
{
    public readonly struct Result<TResult, TError> : IEquatable<Result<TResult, TError>>
    {
        private readonly bool isSuccess;
        private readonly TResult result;
        private readonly TError error;

        private Result(bool isSuccess, TResult result, TError error)
        {
            this.isSuccess = isSuccess;
            this.result = result;
            this.error = error;
        }

        public TResult ResultOrDefault(TResult @default) => isSuccess ? result : @default;

        public TResult ResultOrDefault(Func<TResult> defaultProvider) => isSuccess ? result : defaultProvider();

        public TResult ResultOrThrow(Func<TError, Exception> exceptionProvider) =>
            isSuccess ? result : throw exceptionProvider(error);

        public Maybe<TResult> ResultMaybe() => isSuccess ? Maybe.Just(result) : Maybe.Nothing;

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

        public void Match(Action<TResult> onSuccess, Action<TError> onFailure)
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

        public TOut Match<TOut>(Func<TResult, TOut> onSuccess, Func<TError, TOut> onFailure) =>
            isSuccess ? onSuccess(result) : onFailure(error);

        public bool Equals(Result<TResult, TError> other) =>
            isSuccess == other.isSuccess
            && EqualityComparer<TResult>.Default.Equals(result, other.result)
            && EqualityComparer<TError>.Default.Equals(error, other.error);

        public override bool Equals(object obj) => obj is Result<TResult, TError> other && Equals(other);

        public static bool operator ==(Result<TResult, TError> left, Result<TResult, TError> right) =>
            left.Equals(right);

        public static bool operator !=(Result<TResult, TError> left, Result<TResult, TError> right) =>
            !left.Equals(right);

        public override int GetHashCode()
        {
            var hashCode = isSuccess.GetHashCode();
            hashCode = (hashCode * 397) ^ EqualityComparer<TResult>.Default.GetHashCode(result);
            hashCode = (hashCode * 397) ^ EqualityComparer<TError>.Default.GetHashCode(error);
            return hashCode;
        }

        public override string ToString() => isSuccess ? $"success {result}" : $"error {error}";

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TResult, TError>(Success<TResult> success)
        {
            return new Result<TResult, TError>(true, success.Result, default);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static implicit operator Result<TResult, TError>(Failure<TError> failure)
        {
            return new Result<TResult, TError>(false, default, failure.Error);
        }
    }

    public static class Result
    {
        public static Result<TResult, TError> Success<TResult, TError>(TResult result) => Success(result);

        public static Success<T> Success<T>(T result) => new Success<T>(result);

        public static Result<TResult, TError> Failure<TResult, TError>(TError error) => Failure(error);

        public static Failure<T> Failure<T>(T error) => new Failure<T>(error);

        public static TResult ResultOrThrow<TResult, TError>(this Result<TResult, TError> result)
            where TError : Exception
        {
            return result.ResultOrThrow(e => e);
        }
    }

    public readonly struct Success<T>
    {
        public readonly T Result;

        internal Success(T result)
        {
            Result = result;
        }
    }

    public readonly struct Failure<T>
    {
        public readonly T Error;

        internal Failure(T error)
        {
            Error = error;
        }
    }
}
