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
    public sealed class Rotor2Tests
    {
        private const float epsilon = 1E-3f;

        public Rotor2Tests()
        {
            Arb.Register<FloatGenerators.ForArithmetic>();
            Arb.Register<Vector2Generators.All>();
        }

        [Fact]
        public void IdentityRotorHasMagnitudeOne()
        {
            Rotor2.Identity.Magnitude.Should().BeApproximately(1, epsilon);
            Rotor2.Identity.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        }

        [Property]
        public void IdentityRotorDoesNotRotateVector(Vector2 vector)
        {
            AssertionExtensions.Should(Rotor2.Identity.Rotate(vector)).Be(vector);
        }

        [Property]
        public void NormalizedRotorHasMagnitudeOne(float scalar, Vector2 l, Vector2 r)
        {
            var rotor = new Rotor2(scalar, Bivector2.Wedge(l, r));
            if (rotor == new Rotor2()) return;
            var normalizedRotor = rotor.Normalized();

            normalizedRotor.Magnitude.Should().BeApproximately(1, epsilon);
            normalizedRotor.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        }

        [Fact]
        public void NormalizedZeroRotorIsZeroRotor()
        {
            var rotor = new Rotor2();

            rotor.Normalized().Should().Be(rotor);
        }

        [Property]
        public void NormalizedRotorDoesNotChangeVectorLengthOnRotation(float scalar, Vector2 l, Vector2 r, Vector2 v)
        {
            var rotor = new Rotor2(scalar, Bivector2.Wedge(l, r));
            // Zero rotors are special.
            if (rotor == new Rotor2()) return;
            var normalizedRotor = rotor.Normalized();

            var rotatedV = normalizedRotor.Rotate(v);
            rotatedV.Length.Should().BeApproximately(v.Length, epsilon);
        }

        [Property]
        public void ReversedKeepsMagnitudeInvariant(float scalar, Vector2 l, Vector2 r)
        {
            var rotor = new Rotor2(scalar, Bivector2.Wedge(l, r));

            var reversedRotor = rotor.Reversed();

            reversedRotor.Magnitude.Should().BeApproximately(rotor.Magnitude, epsilon);
        }

        [Property]
        public void ReversedRotorRotatesInTheOppositeDirection(Vector2 from, Vector2 to)
        {
            if (from == Vector2.Zero) from = Vector2.UnitX;
            if (to == Vector2.Zero) to = Vector2.UnitY;
            if (to == -from) return;

            var rotor = Rotor2.Between(from.Normalized(), to.Normalized());
            var reversedRotor = rotor.Reversed();

            var rotated = reversedRotor.Rotate(to);

            rotated.Normalized().Should().BeApproximately(from.Normalized(), epsilon);
        }

        [Property]
        public void RotorBetweenVectorsHasMagnitudeOne(Vector2 from, Vector2 to)
        {
            if (from == Vector2.Zero) from = Vector2.UnitX;
            if (to == Vector2.Zero) to = Vector2.UnitY;

            var rotor = Rotor2.Between(from, to);

            rotor.Magnitude.Should().BeApproximately(1, epsilon);
            rotor.MagnitudeSquared.Should().BeApproximately(1, epsilon);
        }

        [Property]
        public void RotorBetweenSameVectorIsIdentity(Vector2 fromTo)
        {
            if (fromTo == Vector2.Zero) fromTo = Vector2.One;

            var rotor = Rotor2.Between(fromTo, fromTo);

            rotor.Should().Be(Rotor2.Identity);
        }

        [Property]
        public void RotorBetweenVectorsShouldRotateFromToDirectionOfTo(Vector2 from, Vector2 to)
        {
            if (from == Vector2.Zero) from = Vector2.UnitX;
            if (to == Vector2.Zero) to = Vector2.UnitY;
            if (areCollinear(to, from)) return;

            var rotor = Rotor2.Between(from.Normalized(), to.Normalized());

            var rotated = rotor.Rotate(from);

            rotated.Normalized().Should().BeApproximately(to.Normalized(), epsilon);
        }

        [Property]
        public void RotorsWithSameComponentsAreEqual(float scalar, Vector2 l, Vector2 r)
        {
            var bivector = Bivector2.Wedge(l, r);
            var rotor1 = new Rotor2(scalar, bivector);
            var rotor2 = new Rotor2(scalar, bivector);

            rotor1.Equals(rotor2).Should().BeTrue();
            (rotor1 == rotor2).Should().BeTrue();
            (rotor1 != rotor2).Should().BeFalse();
            rotor1.GetHashCode().Should().Be(rotor2.GetHashCode());
        }

        [Property]
        public void RotorsWithDifferentScalarComponentAreNotEqual(float scalar1, float scalar2, Vector2 l, Vector2 r)
        {
            // ReSharper disable once CompareOfFloatsByEqualityOperator
            if (scalar1 == scalar2) scalar2++;

            var bivector = Bivector2.Wedge(l, r);
            var rotor1 = new Rotor2(scalar1, bivector);
            var rotor2 = new Rotor2(scalar2, bivector);

            rotor1.Equals(rotor2).Should().BeFalse();
            (rotor1 == rotor2).Should().BeFalse();
            (rotor1 != rotor2).Should().BeTrue();
        }

        [Property]
        public void RotorsWithDifferentBivectorComponentAreNotEqual(
            float scalar, Vector2 l1, Vector2 r1, Vector2 l2, Vector2 r2)
        {
            var bivector1 = Bivector2.Wedge(l1, r1);
            var bivector2 = Bivector2.Wedge(l2, r2);
            if (bivector1 == bivector2)
                bivector2 = Bivector2.Wedge(l2 + Vector2.UnitX, r2 + Vector2.UnitY);

            var rotor1 = new Rotor2(scalar, bivector1);
            var rotor2 = new Rotor2(scalar, bivector2);

            rotor1.Equals(rotor2).Should().BeFalse();
            (rotor1 == rotor2).Should().BeFalse();
            (rotor1 != rotor2).Should().BeTrue();
        }

        private static bool areCollinear(Vector2 v1, Vector2 v2)
        {
            if (v1.Y == 0 && v2.Y == 0) return true;
            if (v1 == Vector2.Zero || v2 == Vector2.Zero) return true;
            if (v1.Y == 0 || v2.Y == 0) return false;
            return Math.Abs(v1.X / v1.Y - v2.X / v2.Y) < epsilon;
        }
    }
}
