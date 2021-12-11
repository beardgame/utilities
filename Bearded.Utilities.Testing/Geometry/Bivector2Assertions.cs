using Bearded.Utilities.Geometry;
using FluentAssertions;

namespace Bearded.Utilities.Testing.Geometry;

public sealed class Bivector2Assertions
{
    private readonly Bivector2 subject;

    public Bivector2Assertions(Bivector2 instance) => subject = instance;

    [CustomAssertion]
    public AndConstraint<Bivector2Assertions> Be(Bivector2 other, string because = "", params object[] becauseArgs)
    {
        AssertionExtensions.Should(subject).Be(other, because, becauseArgs);
        return new AndConstraint<Bivector2Assertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<Bivector2Assertions> NotBe(Bivector2 other, string because = "", params object[] becauseArgs)
    {
        AssertionExtensions.Should(subject).NotBe(other, because, becauseArgs);
        return new AndConstraint<Bivector2Assertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<Bivector2Assertions> BeApproximately(
        Bivector2 other, float precision, string because = "", params object[] becauseArgs)
    {
        subject.Magnitude.Should().BeApproximately(other.Magnitude, precision, because, becauseArgs);
        return new AndConstraint<Bivector2Assertions>(this);
    }

    [CustomAssertion]
    public AndConstraint<Bivector2Assertions> NotBeApproximately(
        Bivector2 other, float precision, string because = "", params object[] becauseArgs)
    {
        subject.Magnitude.Should().NotBeApproximately(other.Magnitude, precision, because, becauseArgs);
        return new AndConstraint<Bivector2Assertions>(this);
    }
}
