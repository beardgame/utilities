using System;
using OpenTK.Mathematics;

namespace Bearded.Utilities.Geometry;

/// <summary>
/// Represents a bivector in three-dimensional Euclidean space.
/// </summary>
public readonly struct Bivector3 : IEquatable<Bivector3>
{
    public float Xy { get; }
    public float Yz { get; }
    public float Xz { get; }

    public static readonly Bivector3 Zero = new Bivector3(0, 0, 0);

    public static readonly Bivector3 UnitXy = new Bivector3(1, 0, 0);

    public static readonly Bivector3 UnitYz = new Bivector3(0, 1, 0);

    public static readonly Bivector3 UnitXz = new Bivector3(0, 0, 1);

    public static Bivector3 Wedge(Vector3 left, Vector3 right) =>
        new Bivector3(
            left.X * right.Y - right.X * left.Y,
            left.Y * right.Z - right.Y * left.Z,
            left.X * right.Z - right.X * left.Z);

    public Bivector3(float xy, float yz, float xz)
    {
        Xy = xy;
        Yz = yz;
        Xz = xz;
    }

    public static Bivector3 operator +(Bivector3 left, Bivector3 right) =>
        new Bivector3(left.Xy + right.Xy, left.Yz + right.Yz, left.Xz + right.Xz);

    public static Bivector3 operator -(Bivector3 left, Bivector3 right) =>
        new Bivector3(left.Xy - right.Xy, left.Yz - right.Yz, left.Xz - right.Xz);

    public static Bivector3 operator -(Bivector3 bivector) =>
        new Bivector3(-bivector.Xy, -bivector.Yz, -bivector.Xz);

    public bool Equals(Bivector3 other) => Xy.Equals(other.Xy) && Yz.Equals(other.Yz) && Xz.Equals(other.Xz);

    public override bool Equals(object? obj) => obj is Bivector3 other && Equals(other);

    public override int GetHashCode() => HashCode.Combine(Xy, Yz, Xz);

    public static bool operator ==(Bivector3 left, Bivector3 right) => left.Equals(right);

    public static bool operator !=(Bivector3 left, Bivector3 right) => !left.Equals(right);
}
