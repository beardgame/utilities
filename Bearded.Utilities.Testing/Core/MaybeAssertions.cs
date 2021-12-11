using FluentAssertions;
using FluentAssertions.Execution;

namespace Bearded.Utilities.Testing;

public sealed class MaybeAssertions<T>
{
    private readonly Maybe<T> subject;

    public MaybeAssertions(Maybe<T> instance) => subject = instance;

    [CustomAssertion]
    public void BeJust(T value, string because = "", params object[] becauseArgs)
    {
        BeJust().Which.Should().Be(value, because, becauseArgs);
    }

    [CustomAssertion]
    public AndWhichConstraint<MaybeAssertions<T>, T> BeJust(string because = "", params object[] becauseArgs)
    {
        var onValueCalled = false;
        var matched = default(T);

        subject.Match(
            onValue: actual =>
            {
                onValueCalled = true;
                matched = actual;
            }, onNothing: () => { });

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(onValueCalled)
            .FailWith("Expected maybe to have value, but had none.");

        return new AndWhichConstraint<MaybeAssertions<T>, T>(this, matched);
    }

    [CustomAssertion]
    public void BeNothing(string because = "", params object[] becauseArgs)
    {
        var onNothingCalled = false;

        subject.Match(onValue: _ => { }, onNothing: () => onNothingCalled = true);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(onNothingCalled)
            .FailWith("Expected maybe to be nothing, but had value.");
    }
}
