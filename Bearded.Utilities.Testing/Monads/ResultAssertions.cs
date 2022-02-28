using Bearded.Utilities.Monads;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Bearded.Utilities.Testing.Monads;

public sealed class ResultAssertions<TResult, TError>
{
    private readonly Result<TResult, TError> subject;

    public ResultAssertions(Result<TResult, TError> subject)
    {
        this.subject = subject;
    }

    [CustomAssertion]
    public void BeSuccessWithResult(TResult result, string because = "", params object[] becauseArgs)
    {
        BeSuccess().Which.Should().Be(result, because, becauseArgs);
    }

    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions<TResult, TError>, TResult> BeSuccess(
        string because = "", params object[] becauseArgs)
    {
        var onResultCalled = false;
        var matched = default(TResult);

        subject.Match(actual =>
        {
            onResultCalled = true;
            matched = actual;
        });

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(onResultCalled)
            .FailWith("Expected result to be successful, but was not.");

        return new AndWhichConstraint<ResultAssertions<TResult, TError>, TResult>(this, matched);
    }

    [CustomAssertion]
    public void BeFailureWithError(TError error, string because = "", params object[] becauseArgs)
    {
        BeFailure().Which.Should().Be(error, because, becauseArgs);
    }

    [CustomAssertion]
    public AndWhichConstraint<ResultAssertions<TResult, TError>, TError> BeFailure(
        string because = "", params object[] becauseArgs)
    {
        var onErrorCalled = false;
        var matched = default(TError);

        subject.Match(_ => {}, actual =>
        {
            onErrorCalled = true;
            matched = actual;
        });

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(onErrorCalled)
            .FailWith("Expected result to be erroneous, but was not.");

        return new AndWhichConstraint<ResultAssertions<TResult, TError>, TError>(this, matched);
    }
}
