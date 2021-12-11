using System;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Testing.Geometry;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.Geometry;

public sealed class Bivector2Tests
{
    private const float epsilon = 1E-6f;

    public Bivector2Tests()
    {
        Arb.Register<FloatGenerators.ForArithmetic>();
        Arb.Register<Vector2Generators.All>();
    }

    [Fact]
    public void WedgeOfUnitVectorsIsUnitBivector()
    {
        var wedge = Bivector2.Wedge(Vector2.UnitX, Vector2.UnitY);

        wedge.Should().BeApproximately(Bivector2.Unit, epsilon);
    }

    [Property]
    public void WedgeOfVectorWithSelfIsZero(Vector2 vector)
    {
        var wedge = Bivector2.Wedge(vector, vector);

        wedge.Should().BeApproximately(Bivector2.Zero, epsilon);
    }

    [Property]
    public void WedgeOfCollinearVectorsIsZero(Vector2 vector, float scalar)
    {
        var wedge = Bivector2.Wedge(vector, vector * scalar);

        wedge.Should().BeApproximately(Bivector2.Zero, epsilon);
    }

    [Property]
    public void WedgeOfNonCollinearVectorsIsNonZero(Vector2 left, Vector2 right)
    {
        if (areCollinear(left, right)) return;

        var wedge = Bivector2.Wedge(left, right);

        wedge.Should().NotBe(Bivector2.Zero);
    }

    [Property]
    public void WedgeIsAntiSymmetric(Vector2 left, Vector2 right)
    {
        var wedge1 = Bivector2.Wedge(left, right);
        var wedge2 = Bivector2.Wedge(right, left);

        wedge1.Should().Be(-wedge2);
    }

    [Property]
    public void AddingBivectorsAddsMagnitudes(float f1, float f2)
    {
        var bivector1 = new Bivector2(f1);
        var bivector2 = new Bivector2(f2);
        var sum = bivector1 + bivector2;

        sum.Should().BeApproximately(new Bivector2(f1 + f2), epsilon);
    }

    [Property]
    public void SubtractingBivectorsSubtractsMagnitudes(float f1, float f2)
    {
        var bivector1 = new Bivector2(f1);
        var bivector2 = new Bivector2(f2);
        var difference = bivector1 - bivector2;

        difference.Should().BeApproximately(new Bivector2(f1 - f2), epsilon);
    }

    [Property]
    public void BivectorsWithSameMagnitudeAreEqual(float magnitude)
    {
        var bivector1 = new Bivector2(magnitude);
        var bivector2 = new Bivector2(magnitude);

        bivector1.Equals(bivector2).Should().BeTrue();
        (bivector1 == bivector2).Should().BeTrue();
        (bivector1 != bivector2).Should().BeFalse();
        bivector1.GetHashCode().Should().Be(bivector2.GetHashCode());
    }

    [Property]
    public void BivectorsWithDifferentMagnitudeAreNotEqual(float f1, float f2)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (f1 == f2) f2++;

        var bivector1 = new Bivector2(f1);
        var bivector2 = new Bivector2(f2);

        bivector1.Equals(bivector2).Should().BeFalse();
        (bivector1 == bivector2).Should().BeFalse();
        (bivector1 != bivector2).Should().BeTrue();
    }

    private static bool areCollinear(Vector2 v1, Vector2 v2)
    {
        if (v1.Y == 0 && v2.Y == 0) return true;
        if (v1 == Vector2.Zero || v2 == Vector2.Zero) return true;
        if (v1.Y == 0 || v2.Y == 0) return false;
        return Math.Abs(v1.X / v1.Y - v2.X / v2.Y) < epsilon;
    }
}
