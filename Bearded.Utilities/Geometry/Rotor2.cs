using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry
{
    public readonly struct Rotor2 : IEquatable<Rotor2>
    {
        /// <summary>
        /// The scalar (0-dimensional) component of this rotor.
        /// </summary>
        public float Scalar { get; }

        /// <summary>
        /// The bivector (2-dimensional) component of this rotor.
        /// </summary>
        public Bivector2 Bivector { get; }

        /// <summary>
        /// The xy component of the bivector component of this rotor.
        /// </summary>
        public float Xy => Bivector.Xy;

        public float Magnitude => MagnitudeSquared.Sqrted();

        public float MagnitudeSquared => Scalar.Squared() + Bivector.MagnitudeSquared;

        public static Rotor2 Identity { get; } = new Rotor2(1, Bivector2.Zero);

        /// <summary>
        /// Creates a rotor that rotates the from vector to the to vector, assuming these vectors are normalized.
        /// The rotor between two opposite vectors is undefined.
        /// </summary>
        public static Rotor2 Between(Vector2 from, Vector2 to)
        {
            return new Rotor2(1 + Vector2.Dot(from, to), Bivector2.Wedge(to, from)).Normalized();
        }

        public Rotor2(float scalar, Bivector2 bivector)
        {
            Scalar = scalar;
            Bivector = bivector;
        }

        /// <summary>
        /// Returns the rotor with the same relative components, but magnitude one.
        /// </summary>
        public Rotor2 Normalized()
        {
            if (MagnitudeSquared == 0) return this;
            var l = Magnitude;
            return new Rotor2(Scalar / l, Bivector / l);
        }

        /// <summary>
        /// Rotates a vector according to the rotation defined by this rotor.
        /// May not induce a proper rotation if the rotor isn't normalized.
        /// </summary>
        public Vector2 Rotate(Vector2 from)
        {
            var (xx, yy) = (Scalar, Scalar);

            // intermediate = geometricProduct(rotor, from)
            var x = +from.X * xx + from.Y * Xy;
            var y = -from.X * Xy + from.Y * yy;

            // result = geometricProduct(intermediate, rotor*)
            return new Vector2(
                +x * xx + y * Xy,
                -x * Xy + y * yy
            );
        }

        /// <summary>
        /// Returns the rotor that does the reverse rotation.
        /// </summary>
        public Rotor2 Reversed() => new Rotor2(Scalar, -Bivector);

        public bool Equals(Rotor2 other) => Scalar.Equals(other.Scalar) && Bivector.Equals(other.Bivector);

        public override bool Equals(object? obj) => obj is Rotor2 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Scalar, Bivector);

        public static bool operator ==(Rotor2 left, Rotor2 right) => left.Equals(right);

        public static bool operator !=(Rotor2 left, Rotor2 right) => !left.Equals(right);
    }
}
