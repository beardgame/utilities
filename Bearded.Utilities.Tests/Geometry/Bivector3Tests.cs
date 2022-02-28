using System.Collections.Generic;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Testing.Geometry;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.Geometry;

public sealed class Bivector3Tests
{
    private const float epsilon = 1E-3f;

    public Bivector3Tests()
    {
        Arb.Register<FloatGenerators.ForArithmetic>();
        Arb.Register<Vector3Generators.All>();
    }

    [Fact]
    public void WedgeOfUnitVectorsIsUnitBivector()
    {
        Bivector3.Wedge(Vector3.UnitX, Vector3.UnitY).Should().BeApproximately(Bivector3.UnitXy, epsilon);
        Bivector3.Wedge(Vector3.UnitY, Vector3.UnitZ).Should().BeApproximately(Bivector3.UnitYz, epsilon);
        Bivector3.Wedge(Vector3.UnitX, Vector3.UnitZ).Should().BeApproximately(Bivector3.UnitXz, epsilon);
    }

    [Property]
    public void WedgeOfVectorWithSelfIsZero(Vector3 vector)
    {
        var wedge = Bivector3.Wedge(vector, vector);

        wedge.Should().BeApproximately(Bivector3.Zero, epsilon);
    }

    [Property]
    public void WedgeOfCollinearVectorsIsZero(Vector3 vector, float scalar)
    {
        var wedge = Bivector3.Wedge(vector, vector * scalar);

        wedge.Should().BeApproximately(Bivector3.Zero, epsilon);
    }

    [Property]
    public void WedgeOfNonCollinearVectorsIsNonZero(Vector3 left, Vector3 right)
    {
        if (areCollinear(left, right)) return;

        var wedge = Bivector3.Wedge(left, right);

        wedge.Should().NotBe(Bivector3.Zero);
    }

    [Property]
    public void WedgeIsAntiSymmetric(Vector3 left, Vector3 right)
    {
        var wedge1 = Bivector3.Wedge(left, right);
        var wedge2 = Bivector3.Wedge(right, left);

        wedge1.Should().Be(-wedge2);
    }

    [Property]
    public void FromAxisCreatesBivectorWithSameMagnitudeAsVector(Vector3 axis)
    {
        var bivector = Bivector3.FromAxis(axis);

        bivector.Magnitude.Should().BeApproximately(axis.Length, epsilon);
    }

    [Property]
    public void FromAxisRetainsSign(Vector3 axis)
    {
        var bivector = Bivector3.FromAxis(axis);
        var reverseBivector = Bivector3.FromAxis(-axis);

        bivector.Should().BeApproximately(-reverseBivector, epsilon);
    }

    [Theory]
    [MemberData(nameof(UnitVectorsWithOrthogonalPlanes))]
    public void FromAxisReturnsOrthogonalPlaneToUnitVectors(Vector3 axis, Bivector3 expectedBivector)
    {
        var bivector = Bivector3.FromAxis(axis);

        bivector.Should().BeApproximately(expectedBivector, epsilon);
    }

    public static IEnumerable<object[]> UnitVectorsWithOrthogonalPlanes()
    {
        yield return new object[] { Vector3.UnitX, Bivector3.UnitYz };
        yield return new object[] { Vector3.UnitY, -Bivector3.UnitXz };
        yield return new object[] { Vector3.UnitZ, Bivector3.UnitXy };
    }

    [Fact]
    public void UnitBivectorsHaveMagnitudeOne()
    {
        Bivector3.UnitXy.Magnitude.Should().BeApproximately(1, epsilon);
        Bivector3.UnitXy.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        Bivector3.UnitYz.Magnitude.Should().BeApproximately(1, epsilon);
        Bivector3.UnitYz.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        Bivector3.UnitXz.Magnitude.Should().BeApproximately(1, epsilon);
        Bivector3.UnitXz.MagnitudeSquared.Should().BeApproximately(1, epsilon);
    }

    [Fact]
    public void ZeroBivectorHasMagnitudeZero()
    {
        Bivector3.Zero.Magnitude.Should().BeApproximately(0, epsilon);
        Bivector3.Zero.MagnitudeSquared.Should().BeApproximately(0, epsilon);
    }

    [Property]
    public void NormalizedNonZeroBivectorHasMagnitudeOne(float xy, float yz, float xz)
    {
        var bivector = new Bivector3(xy, yz, xz);
        if (bivector == Bivector3.Zero) return;

        var normalized = bivector.Normalized();

        normalized.Magnitude.Should().BeApproximately(1, epsilon);
        normalized.MagnitudeSquared.Should().BeApproximately(1, epsilon);
    }

    [Fact]
    public void NormalizedZeroBivectorIsZeroBivector()
    {
        var bivector = Bivector3.Zero;

        bivector.Normalized().Should().Be(bivector);
    }

    [Property]
    public void AddingBivectorsAddsComponents(float xy1, float xy2, float yz1, float yz2, float xz1, float xz2)
    {
        var bivector1 = new Bivector3(xy1, yz1, xz1);
        var bivector2 = new Bivector3(xy2, yz2, xz2);
        var sum = bivector1 + bivector2;

        sum.Should().BeApproximately(new Bivector3(xy1 + xy2, yz1 + yz2, xz1 + xz2), epsilon);
    }

    [Property]
    public void SubtractingBivectorsSubtractsComponents(
        float xy1, float xy2, float yz1, float yz2, float xz1, float xz2)
    {
        var bivector1 = new Bivector3(xy1, yz1, xz1);
        var bivector2 = new Bivector3(xy2, yz2, xz2);
        var sum = bivector1 - bivector2;

        sum.Should().BeApproximately(new Bivector3(xy1 - xy2, yz1 - yz2, xz1 - xz2), epsilon);
    }

    [Property]
    public void ScalingBivectorScalesItsComponents(float xy, float yz, float xz, float scalar)
    {
        var bivector = new Bivector3(xy, yz, xz);
        var scaled = scalar * bivector;

        scaled.Xy.Should().BeApproximately(xy * scalar, epsilon);
        scaled.Yz.Should().BeApproximately(yz * scalar, epsilon);
        scaled.Xz.Should().BeApproximately(xz * scalar, epsilon);
    }

    [Property]
    public void DividingBivectorByScalarDividesItsComponents(float xy, float yz, float xz, float divider)
    {
        if (divider == 0) return;
        var bivector = new Bivector3(xy, yz, xz);
        var scaled = bivector / divider;

        scaled.Xy.Should().BeApproximately(xy / divider, epsilon);
        scaled.Yz.Should().BeApproximately(yz / divider, epsilon);
        scaled.Xz.Should().BeApproximately(xz / divider, epsilon);
    }

    [Property]
    public void BivectorsWithSameComponentsAreEqual(float xy, float yz, float xz)
    {
        var bivector1 = new Bivector3(xy, yz, xz);
        var bivector2 = new Bivector3(xy, yz, xz);

        bivector1.Equals(bivector2).Should().BeTrue();
        (bivector1 == bivector2).Should().BeTrue();
        (bivector1 != bivector2).Should().BeFalse();
        bivector1.GetHashCode().Should().Be(bivector2.GetHashCode());
    }

    [Property]
    public void BivectorsWithDifferentXyComponentAreNotEqual(
        float xy1, float xy2, float yz, float xz)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (xy1 == xy2) xy2++;

        var bivector1 = new Bivector3(xy1, yz, xz);
        var bivector2 = new Bivector3(xy2, yz, xz);

        bivector1.Equals(bivector2).Should().BeFalse();
        (bivector1 == bivector2).Should().BeFalse();
        (bivector1 != bivector2).Should().BeTrue();
    }

    [Property]
    public void BivectorsWithDifferentYzComponentAreNotEqual(
        float xy, float yz1, float yz2, float xz)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (yz1 == yz2) yz2++;

        var bivector1 = new Bivector3(xy, yz1, xz);
        var bivector2 = new Bivector3(xy, yz2, xz);

        bivector1.Equals(bivector2).Should().BeFalse();
        (bivector1 == bivector2).Should().BeFalse();
        (bivector1 != bivector2).Should().BeTrue();
    }

    [Property]
    public void BivectorsWithDifferentXzComponentAreNotEqual(
        float xy, float yz, float xz1, float xz2)
    {
        // ReSharper disable once CompareOfFloatsByEqualityOperator
        if (xz1 == xz2) xz2++;

        var bivector1 = new Bivector3(xy, yz, xz1);
        var bivector2 = new Bivector3(xy, yz, xz2);

        bivector1.Equals(bivector2).Should().BeFalse();
        (bivector1 == bivector2).Should().BeFalse();
        (bivector1 != bivector2).Should().BeTrue();
    }

    private static bool areCollinear(Vector3 v1, Vector3 v2)
    {
        return Vector3.Cross(v1, v2).LengthSquared < epsilon;
    }
}
