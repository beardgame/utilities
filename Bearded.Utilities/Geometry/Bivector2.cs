using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// Represents a bivector in two-dimensional Euclidean space.
/// </summary>
public readonly struct Bivector2 : IEquatable<Bivector2>
{
    public float Magnitude { get; }

    public static readonly Bivector2 Zero = new Bivector2(0);

    public static readonly Bivector2 Unit = new Bivector2(1);

    public static Bivector2 Wedge(Vector2 left, Vector2 right) =>
        new Bivector2(left.X * right.Y - left.Y * right.X);

    public Bivector2(float magnitude)
    {
        Magnitude = magnitude;
    }

    public static Bivector2 operator +(Bivector2 left, Bivector2 right) =>
        new Bivector2(left.Magnitude + right.Magnitude);

    public static Bivector2 operator -(Bivector2 left, Bivector2 right) =>
        new Bivector2(left.Magnitude - right.Magnitude);

    public static Bivector2 operator -(Bivector2 bivector) => new Bivector2(-bivector.Magnitude);

    public bool Equals(Bivector2 other) => Magnitude.Equals(other.Magnitude);

    public override bool Equals(object? obj) => obj is Bivector2 other && Equals(other);

    public override int GetHashCode() => Magnitude.GetHashCode();

    public static bool operator ==(Bivector2 left, Bivector2 right) => left.Equals(right);

    public static bool operator !=(Bivector2 left, Bivector2 right) => !left.Equals(right);
}
