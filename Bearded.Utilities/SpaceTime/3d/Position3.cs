using System;
using System.Globalization;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of an absolute 3d position vector.
/// </summary>
public readonly struct Position3 : IEquatable<Position3>, IFormattable
{
    private readonly Vector3 value;

    #region construction

    public Position3(Vector3 value)
    {
        this.value = value;
    }

    public Position3(float x, float y, float z)
        : this(new Vector3(x, y, z))
    {
    }

    public Position3(Unit x, Unit y, Unit z)
        : this(new Vector3(x.NumericValue, y.NumericValue, z.NumericValue))
    {
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric vector value of the position vector.
    /// </summary>
    public Vector3 NumericValue => value;

    /// <summary>
    /// Returns the X component of the position vector.
    /// </summary>
    public Unit X => new Unit(value.X);

    /// <summary>
    /// Returns the Y component of the position vector.
    /// </summary>
    public Unit Y => new Unit(value.Y);

    /// <summary>
    /// Returns the Z component of the position vector.
    /// </summary>
    public Unit Z => new Unit(value.Z);

    /// <summary>
    /// Returns a Position2 type with value 0.
    /// </summary>
    public static Position3 Zero => new Position3(0, 0, 0);

    #endregion

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed position vectors.
    /// </summary>
    /// <param name="p0">The position vector at t = 0.</param>
    /// <param name="p1">The position vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Position3 Lerp(Position3 p0, Position3 p1, float t) => p0 + (p1 - p0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed position vector.
    /// </summary>
    /// <param name="p">The position vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Position3 LerpTo(Position3 p, float t) => Lerp(this, p, t);

    #endregion

    #region equality and hashcode

    public bool Equals(Position3 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Position3 pos && Equals(pos);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) => "(" +
        $"{value.X.ToString(format, formatProvider)}, " +
        $"{value.Y.ToString(format, formatProvider)}, " +
        $"{value.Z.ToString(format, formatProvider)}) u";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region Difference2 interaction

    /// <summary>
    /// Adds a difference vector to an absolute position.
    /// </summary>
    public static Position3 operator +(Position3 p, Difference3 d) => new Position3(p.value + d.NumericValue);

    /// <summary>
    /// Adds a difference vector to an absolute position.
    /// </summary>
    public static Position3 operator +(Difference3 d, Position3 p) => new Position3(p.value + d.NumericValue);

    /// <summary>
    /// Subtracts a difference vector from an absolute position.
    /// </summary>
    public static Position3 operator -(Position3 p, Difference3 d) => new Position3(p.value - d.NumericValue);

    /// <summary>
    /// Subtracts two absolute positions, returning a difference vector.
    /// </summary>
    public static Difference3 operator -(Position3 p0, Position3 p1) => new Difference3(p0.value - p1.value);

    #endregion

    #region comparison

    /// <summary>
    /// Compares two position vectors for equality.
    /// </summary>
    public static bool operator ==(Position3 p0, Position3 p1) => p0.Equals(p1);

    /// <summary>
    /// Compares two position vectors for inequality.
    /// </summary>
    public static bool operator !=(Position3 p0, Position3 p1) => !(p0 == p1);

    #endregion

    #endregion

}
