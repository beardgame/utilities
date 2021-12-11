using Bearded.Utilities.Monads;

namespace Bearded.Utilities.Testing.Monads;

public static class ResultExtensions
{
    public static ResultAssertions<TResult, TError> Should<TResult, TError>(this Result<TResult, TError> subject) =>
        new ResultAssertions<TResult, TError>(subject);
}
