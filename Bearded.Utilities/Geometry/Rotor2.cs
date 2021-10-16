using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry
{
    /// <summary>
    /// A representation of a rotation in two-dimensional space.
    /// Rotors are the analog to complex numbers in geometric algebra. The underlying maths is isomorphic to the maths
    /// used in complex numbers, but the underlying geometric concepts are more geometrically intuitive.
    /// Normalized rotors represent a rotation only. Non-normalized rotors may also scale the vector.
    /// </summary>
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

        /// <summary>
        /// The rotor that acts as an identity transformation on all vectors.
        /// </summary>
        public static Rotor2 Identity { get; } = new Rotor2(1, Bivector2.Zero);

        /// <summary>
        /// Creates a rotor that rotates the from vector to the to vector, assuming these vectors are normalized.
        /// The rotor between two opposite vectors is undefined, as there are multiple possible rotations.
        /// </summary>
        public static Rotor2 Between(Vector2 from, Vector2 to)
        {
            return new Rotor2(1 + Vector2.Dot(from, to), Bivector2.Wedge(to, from)).Normalized();
        }

        /// <summary>
        /// Creates a rotor that rotates the from direction to the to direction.
        /// The rotor between two opposite directions is undefined, as there are multiple possible rotations.
        /// </summary>
        public static Rotor2 Between(Direction2 from, Direction2 to) => Between(from.Vector, to.Vector);

        /// <summary>
        /// Creates a rotor that rotates in the (orientation-aware) plane defined by the bivector by the specified
        /// angle.
        /// </summary>
        public static Rotor2 FromPlaneAngle(Bivector2 plane, Angle angle)
        {
            var halfAngle = 0.5f * angle;
            return new Rotor2(MathF.Cos(halfAngle.Radians), -MathF.Sin(halfAngle.Radians) * plane.Normalized());
        }

        /// <summary>
        /// Creates a rotor that rotates by the specified angle.
        /// This is possible in 2D, because there is only one axis to rotate around.
        /// </summary>
        public static Rotor2 FromAngle(Angle angle) => FromPlaneAngle(Bivector2.Unit, angle);

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
