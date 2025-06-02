using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry
{
    /// <summary>
    /// A representation of a rotation in three-dimensional space.
    /// Rotors are the analog to quaternions in geometric algebra. The underlying maths is isomorphic to the maths used
    /// in quaternions, but the underlying geometric concepts are more geometrically intuitive.
    /// Normalized rotors represent a rotation only. Non-normalized rotors may also scale the vector.
    /// </summary>
    public readonly struct Rotor3 : IEquatable<Rotor3>
    {
        /// <summary>
        /// The scalar (0-dimensional) component of this rotor.
        /// </summary>
        public float Scalar { get; }

        /// <summary>
        /// The bivector (2-dimensional) component of this rotor.
        /// </summary>
        public Bivector3 Bivector { get; }

        /// <summary>
        /// The xy component of the bivector component of this rotor.
        /// </summary>
        public float Xy => Bivector.Xy;

        /// <summary>
        /// The yz component of the bivector component of this rotor.
        /// </summary>
        public float Yz => Bivector.Yz;

        /// <summary>
        /// The xz component of the bivector component of this rotor.
        /// </summary>
        public float Xz => Bivector.Xz;

        public float Magnitude => MagnitudeSquared.Sqrted();

        public float MagnitudeSquared => Scalar.Squared() + Bivector.MagnitudeSquared;

        /// <summary>
        /// The rotor that acts as an identity transformation on all vectors.
        /// </summary>
        public static Rotor3 Identity { get; } = new Rotor3(1, Bivector3.Zero);

        /// <summary>
        /// Creates a rotor that rotates the from vector to the to vector, assuming these vectors are normalized.
        /// The rotor between two opposite vectors is undefined, as there are infinitely many possible rotations.
        /// </summary>
        public static Rotor3 Between(Vector3 from, Vector3 to)
        {
            return new Rotor3(1 + Vector3.Dot(from, to), Bivector3.Wedge(to, from)).Normalized();
        }

        /// <summary>
        /// Creates a rotor that rotates in the (orientation-aware) plane defined by the bivector by the specified
        /// angle.
        /// </summary>
        public static Rotor3 FromPlaneAngle(Bivector3 plane, Angle angle)
        {
            var halfAngle = 0.5f * angle;
            return new Rotor3(MathF.Cos(halfAngle.Radians), -MathF.Sin(halfAngle.Radians) * plane.Normalized());
        }

        /// <summary>
        /// Creates a rotor that rotates in the xy plane (i.e. around the z axis).
        /// </summary>
        public static Rotor3 FromXyAngle(Angle angle) => FromPlaneAngle(Bivector3.UnitXy, angle);

        /// <summary>
        /// Creates a rotor that rotates in the xy plane (i.e. around the x axis).
        /// </summary>
        public static Rotor3 FromYzAngle(Angle angle) => FromPlaneAngle(Bivector3.UnitYz, angle);

        /// <summary>
        /// Creates a rotor that rotates in the xz plane (i.e. around the y axis).
        /// </summary>
        public static Rotor3 FromXzAngle(Angle angle) => FromPlaneAngle(Bivector3.UnitXz, angle);

        /// <summary>
        /// Creates a rotor that rotates around the axis by the specified angle.
        /// </summary>
        public static Rotor3 FromAxisAngle(Vector3 axis, Angle angle) =>
            FromPlaneAngle(Bivector3.FromAxis(axis), angle);

        public Rotor3(float scalar, Bivector3 bivector)
        {
            Scalar = scalar;
            Bivector = bivector;
        }

        /// <summary>
        /// Returns the rotor with the same relative components, but magnitude one.
        /// </summary>
        public Rotor3 Normalized()
        {
            if (MagnitudeSquared == 0) return this;
            var l = Magnitude;
            return new Rotor3(Scalar / l, Bivector / l);
        }

        /// <summary>
        /// Returns the rotor that does the reverse rotation.
        /// </summary>
        public Rotor3 Reversed() => new Rotor3(Scalar, -Bivector);

        /// <summary>
        /// Rotates a vector according to the rotation defined by this rotor.
        /// May not induce a proper rotation if the rotor isn't normalized.
        /// </summary>
        public Vector3 Rotate(Vector3 from)
        {
            var (xx, yy, zz) = (Scalar, Scalar, Scalar);

            // intermediate = geometricProduct(rotor, from)
            var x = +from.X * xx + from.Y * Xy + from.Z * Xz;
            var y = -from.X * Xy + from.Y * yy + from.Z * Yz;
            var z = -from.X * Xz - from.Y * Yz + from.Z * zz;
            var xyz = from.X * Yz - from.Y * Xz + from.Z * Xy;

            // result = geometricProduct(intermediate, rotor*)
            return new Vector3(
                +x * xx + y * Xy + z * Xz + xyz * Yz,
                -x * Xy + y * yy + z * Yz - xyz * Xz,
                -x * Xz - y * Yz + z * zz + xyz * Xy
            );
        }

        public bool Equals(Rotor3 other) => Scalar.Equals(other.Scalar) && Bivector.Equals(other.Bivector);

        public override bool Equals(object? obj) => obj is Rotor3 other && Equals(other);

        public override int GetHashCode() => HashCode.Combine(Scalar, Bivector);

        public static bool operator ==(Rotor3 left, Rotor3 right) => left.Equals(right);

        public static bool operator !=(Rotor3 left, Rotor3 right) => !left.Equals(right);
    }
}
