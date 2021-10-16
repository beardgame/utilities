using System;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Tests.Assertions;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class Rotor3Tests
    {
        private const float epsilon = 1E-3f;

        public Rotor3Tests()
        {
            Arb.Register<FloatGenerators.ForArithmetic>();
            Arb.Register<Vector3Generators.All>();
        }

        [Fact]
        public void IdentityRotorHasMagnitudeOne()
        {
            Rotor3.Identity.Magnitude.Should().BeApproximately(1, epsilon);
            Rotor3.Identity.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        }

        [Property]
        public void IdentityRotorDoesNotRotateVector(Vector3 vector)
        {
            AssertionExtensions.Should(Rotor3.Identity.Rotate(vector)).Be(vector);
        }

        [Property]
        public void NormalizedRotorHasMagnitudeOne(float scalar, Vector3 l, Vector3 r)
        {
            var rotor = new Rotor3(scalar, Bivector3.Wedge(l, r));
            if (rotor == new Rotor3()) return;
            var normalizedRotor = rotor.Normalized();

            normalizedRotor.Magnitude.Should().BeApproximately(1, epsilon);
            normalizedRotor.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        }

        [Fact]
        public void NormalizedZeroRotorIsZeroRotor()
        {
            var rotor = new Rotor3();

            rotor.Normalized().Should().Be(rotor);
        }

        [Property]
        public void NormalizedRotorDoesNotChangeVectorLengthOnRotation(float scalar, Vector3 l, Vector3 r, Vector3 v)
        {
            var rotor = new Rotor3(scalar, Bivector3.Wedge(l, r));
            // Zero rotors are special.
            if (rotor == new Rotor3()) return;
            var normalizedRotor = rotor.Normalized();

            var rotatedV = normalizedRotor.Rotate(v);
            rotatedV.Length.Should().BeApproximately(v.Length, epsilon);
        }

        [Property]
        public void ReversedKeepsMagnitudeInvariant(float scalar, Vector3 l, Vector3 r)
        {
            var rotor = new Rotor3(scalar, Bivector3.Wedge(l, r));

            var reversedRotor = rotor.Reversed();

            reversedRotor.Magnitude.Should().BeApproximately(rotor.Magnitude, epsilon);
        }

        [Property]
        public void ReversedRotorRotatesInTheOppositeDirection(Vector3 from, Vector3 to)
        {
            if (from == Vector3.Zero) from = Vector3.UnitX;
            if (to == Vector3.Zero) to = Vector3.UnitY;
            if (areCollinear(to, from)) return;

            var rotor = Rotor3.Between(from.Normalized(), to.Normalized());
            var reversedRotor = rotor.Reversed();

            var rotated = reversedRotor.Rotate(to);

            rotated.Normalized().Should().BeApproximately(from.Normalized(), epsilon);
        }

        [Property]
        public void RotorBetweenVectorsHasMagnitudeOne(Vector3 from, Vector3 to)
        {
            if (from == Vector3.Zero) from = Vector3.UnitX;
            if (to == Vector3.Zero) to = Vector3.UnitY;

            var rotor = Rotor3.Between(from, to);

            rotor.Magnitude.Should().BeApproximately(1, epsilon);
            rotor.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        }

        [Property]
        public void RotorBetweenSameVectorIsIdentity(Vector3 fromTo)
        {
            if (fromTo == Vector3.Zero) fromTo = Vector3.One;

            var rotor = Rotor3.Between(fromTo, fromTo);

            rotor.Should().Be(Rotor3.Identity);
        }

        [Property]
        public void RotorBetweenVectorsShouldRotateFromToDirectionOfTo(Vector3 from, Vector3 to)
        {
            if (from == Vector3.Zero) from = Vector3.UnitX;
            if (to == Vector3.Zero) to = Vector3.UnitY;
            if (to == -from) return;

            var rotor = Rotor3.Between(from.Normalized(), to.Normalized());

            var rotated = rotor.Rotate(from);

            rotated.Normalized().Should().BeApproximately(to.Normalized(), epsilon);
        }

        [Property]
        public void RotorFromPlaneAngleKeepsAngleWithPlaneInvariant(Vector3 l, Vector3 r, float rads, Vector3 toRotate)
        {
            if (areCollinear(l, r)) return;
            var plane = Bivector3.Wedge(l, r);
            var angle = Angle.FromRadians(rads);

            var rotor = Rotor3.FromPlaneAngle(plane, angle);
            var rotated = rotor.Rotate(toRotate);

            // Calculating the angle with the perpendicular axis is easier than calculating the angle with the plane
            // itself, but the invariant still holds.
            var axis = new Vector3(plane.Yz, -plane.Xz, plane.Xy);
            var toRotateDotAxis = Vector3.Dot(toRotate, axis);
            var rotatedDotAxis = Vector3.Dot(rotated, axis);

            rotatedDotAxis.Should().BeApproximately(toRotateDotAxis, epsilon);
        }

        [Property]
        public void RotorFromPlaneAngleRotatesByAngle(Vector3 l, Vector3 r, float rads, Vector3 toRotate)
        {
            if (areCollinear(l, r)) return;
            var plane = Bivector3.Wedge(l, r);
            var angle = Angle.FromRadians(rads);

            var rotor = Rotor3.FromPlaneAngle(plane, angle);
            var rotated = rotor.Rotate(toRotate);

            Vector3.Dot(toRotate.Normalized(), rotated.Normalized()).Should().BeApproximately(MathF.Cos(rads), epsilon);
        }

        [Property]
        public void RotorFromAxisAngleKeepsAngleWithAxisInvariant(Vector3 axis, float rads, Vector3 toRotate)
        {
            if (axis == Vector3.Zero) axis = Vector3.UnitX;
            if (toRotate == Vector3.Zero) toRotate = Vector3.UnitY;
            var angle = Angle.FromRadians(rads);

            var rotor = Rotor3.FromAxisAngle(axis, angle);
            var rotated = rotor.Rotate(toRotate);

            var toRotateDotAxis = Vector3.Dot(toRotate, axis);
            var rotatedDotAxis = Vector3.Dot(rotated, axis);

            rotatedDotAxis.Should().BeApproximately(toRotateDotAxis, epsilon);
        }

        [Property]
        public void RotorFromAxisAngleRotatesByAngle(Vector3 axis, float rads, Vector3 toRotate)
        {
            if (axis == Vector3.Zero) axis = Vector3.UnitX;
            if (toRotate == Vector3.Zero) toRotate = Vector3.UnitY;
            var angle = Angle.FromRadians(rads);

            var rotor = Rotor3.FromAxisAngle(axis, angle);
            var rotated = rotor.Rotate(toRotate);

            Vector3.Dot(toRotate.Normalized(), rotated.Normalized()).Should().BeApproximately(MathF.Cos(rads), epsilon);
        }

        [Property]
        public void RotorsWithSameComponentsAreEqual(float scalar, Vector3 l, Vector3 r)
        {
            var bivector = Bivector3.Wedge(l, r);
            var rotor1 = new Rotor3(scalar, bivector);
            var rotor2 = new Rotor3(scalar, bivector);

            rotor1.Equals(rotor2).Should().BeTrue();
            (rotor1 == rotor2).Should().BeTrue();
            (rotor1 != rotor2).Should().BeFalse();
            rotor1.GetHashCode().Should().Be(rotor2.GetHashCode());
        }

        [Property]
        public void RotorsWithDifferentScalarComponentAreNotEqual(float scalar1, float scalar2, Vector3 l, Vector3 r)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (scalar1 == scalar2) scalar2++;

            var bivector = Bivector3.Wedge(l, r);
            var rotor1 = new Rotor3(scalar1, bivector);
            var rotor2 = new Rotor3(scalar2, bivector);

            rotor1.Equals(rotor2).Should().BeFalse();
            (rotor1 == rotor2).Should().BeFalse();
            (rotor1 != rotor2).Should().BeTrue();
        }

        [Property]
        public void RotorsWithDifferentBivectorComponentAreNotEqual(float scalar, Vector3 l1, Vector3 r1, Vector3 l2, Vector3 r2)
        {
            var bivector1 = Bivector3.Wedge(l1, r1);
            var bivector2 = Bivector3.Wedge(l2, r2);
            if (bivector1 == bivector2)
                bivector2 = Bivector3.Wedge(l2 + Vector3.UnitX, r2 + Vector3.UnitY);

            var rotor1 = new Rotor3(scalar, bivector1);
            var rotor2 = new Rotor3(scalar, bivector2);

            rotor1.Equals(rotor2).Should().BeFalse();
            (rotor1 == rotor2).Should().BeFalse();
            (rotor1 != rotor2).Should().BeTrue();
        }

        private static bool areCollinear(Vector3 v1, Vector3 v2)
        {
            return Vector3.Cross(v1, v2).LengthSquared < epsilon;
        }
    }
}
