using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry
{
    /// <summary>
    /// Represents a bivector in two-dimensional Euclidean space.
    /// </summary>
    public readonly struct Bivector2 : IEquatable<Bivector2>
    {
        public float Xy { get; }

        public static readonly Bivector2 Zero = new Bivector2(0);

        public static readonly Bivector2 Unit = new Bivector2(1);

        public static Bivector2 Wedge(Vector2 left, Vector2 right) =>
            new Bivector2(left.X * right.Y - left.Y * right.X);

        public Bivector2(float xy)
        {
            Xy = xy;
        }

        public float Magnitude() => MathF.Abs(Xy);

        public float MagnitudeSquared() => Xy.Squared();

        public Bivector2 Normalized() => new Bivector2(MathF.Sign(Xy));

        public static Bivector2 operator +(Bivector2 left, Bivector2 right) =>
            new Bivector2(left.Xy + right.Xy);

        public static Bivector2 operator -(Bivector2 left, Bivector2 right) =>
            new Bivector2(left.Xy - right.Xy);

        public static Bivector2 operator -(Bivector2 bivector) => new Bivector2(-bivector.Xy);

        public static Bivector2 operator *(float scalar, Bivector2 bivector) => new Bivector2(scalar * bivector.Xy);

        public static Bivector2 operator *(Bivector2 bivector, float scalar) => scalar * bivector;

        public static Bivector2 operator /(Bivector2 bivector, float divider) => 1 / divider * bivector;

        public bool Equals(Bivector2 other) => Xy.Equals(other.Xy);

        public override bool Equals(object? obj) => obj is Bivector2 other && Equals(other);

        public override int GetHashCode() => Xy.GetHashCode();

        public static bool operator ==(Bivector2 left, Bivector2 right) => left.Equals(right);

        public static bool operator !=(Bivector2 left, Bivector2 right) => !left.Equals(right);
    }
}
