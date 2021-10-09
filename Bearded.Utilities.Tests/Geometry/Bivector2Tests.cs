using System;
using Bearded.Utilities.Geometry;
using Bearded.Utilities.Testing.Geometry;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using OpenTK.Mathematics;
using Xunit;

namespace Bearded.Utilities.Tests.Geometry
{
    public sealed class Bivector2Tests
    {
        private const float epsilon = 1E-6f;

        public Bivector2Tests()
        {
            Arb.Register<FloatGenerators.ForArithmetic>();
            Arb.Register<Vector2Generators.All>();
        }

        [Fact]
        public void OuterProductOfUnitVectorsIsOne()
        {
            var outerProduct = Bivector2.OuterProduct(Vector2.UnitX, Vector2.UnitY);

            outerProduct.Should().BeApproximately(Bivector2.Unit, epsilon);
        }

        [Property]
        public void OuterProductOfVectorWithSelfIsZero(Vector2 vector)
        {
            var outerProduct = Bivector2.OuterProduct(vector, vector);

            outerProduct.Should().BeApproximately(Bivector2.Zero, epsilon);
        }

        [Property]
        public void OuterProductOfCollinearVectorsIsZero(Vector2 vector, float scalar)
        {
            var outerProduct = Bivector2.OuterProduct(vector, vector * scalar);

            outerProduct.Should().BeApproximately(Bivector2.Zero, epsilon);
        }

        [Property]
        public void OuterProductOfNonCollinearVectorsIsNonZero(Vector2 left, Vector2 right)
        {
            if (areCollinear(left, right)) return;

            var outerProduct = Bivector2.OuterProduct(left, right);

            outerProduct.Should().NotBe(Bivector2.Zero);
        }

        [Property]
        public void OuterProductIsAntiSymmetric(Vector2 left, Vector2 right)
        {
            var outerProduct1 = Bivector2.OuterProduct(left, right);
            var outerProduct2 = Bivector2.OuterProduct(right, left);

            outerProduct1.Should().Be(-outerProduct2);
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
}
