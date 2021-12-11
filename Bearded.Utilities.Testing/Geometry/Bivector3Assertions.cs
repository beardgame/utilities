using System;
using Bearded.Utilities.Geometry;
using FluentAssertions;
using FluentAssertions.Execution;

namespace Bearded.Utilities.Testing.Geometry;

public sealed class Bivector3Assertions
{
    private readonly Bivector3 subject;

    public Bivector3Assertions(Bivector3 instance) => subject = instance;

    [CustomAssertion]
    public AndConstraint<Bivector3Assertions> Be(Bivector3 other, string because = "", params object[] becauseArgs)
    {
        AssertionExtensions.Should(subject).Be(other, because, becauseArgs);
        return new AndConstraint<Bivector3Assertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<Bivector3Assertions> NotBe(Bivector3 other, string because = "", params object[] becauseArgs)
    {
        AssertionExtensions.Should(subject).NotBe(other, because, becauseArgs);
        return new AndConstraint<Bivector3Assertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<Bivector3Assertions> BeApproximately(
        Bivector3 other, float precision, string because = "", params object[] becauseArgs)
    {
        using (new AssertionScope())
        {
            subject.Xy.Should().BeApproximately(other.Xy, precision, because, becauseArgs);
            subject.Yz.Should().BeApproximately(other.Yz, precision, because, becauseArgs);
            subject.Xz.Should().BeApproximately(other.Xz, precision, because, becauseArgs);
        }
        return new AndConstraint<Bivector3Assertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<Bivector3Assertions> NotBeApproximately(
        Bivector3 other, float precision, string because = "", params object[] becauseArgs)
    {
        var xyDifference = Math.Abs(subject.Xy - other.Xy);
        var yzDifference = Math.Abs(subject.Yz - other.Yz);
        var xzDifference = Math.Abs(subject.Xz - other.Xz);

        Execute.Assertion
            .BecauseOf(because, becauseArgs)
            .ForCondition(xyDifference > precision || yzDifference > precision || xzDifference > precision)
            .FailWith(
                "Expected {context:value} to not approximate {1} +/- {2}{reason}, " +
                "but {0}'s coordinates only differed by {3} (xy), {4} (yz), and {5} (xz).",
                subject, other, precision, xyDifference, yzDifference, xzDifference);

        return new AndConstraint<Bivector3Assertions>(this);
    }
}
