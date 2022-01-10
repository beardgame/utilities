using FluentAssertions;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Tests.Assertions;

sealed class Vector2Assertions
{
    private readonly Vector2 subject;

    public Vector2Assertions(Vector2 subject)
    {
        this.subject = subject;
    }

    [CustomAssertion]
    public AndConstraint<Vector2Assertions> BeApproximately(
        Vector2 expectedValue,
        float precision,
        string because = "",
        params object[] becauseArgs)
    {
        subject.X.Should().BeApproximately(expectedValue.X, precision, because, becauseArgs);
        subject.Y.Should().BeApproximately(expectedValue.Y, precision, because, becauseArgs);
        return new AndConstraint<Vector2Assertions>(this);
    }
}
