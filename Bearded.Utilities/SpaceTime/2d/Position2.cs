using System;
using System.Globalization;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of an absolute 2d position vector.
/// </summary>
public readonly struct Position2 : IEquatable<Position2>, IFormattable
{
    private readonly Vector2 value;

    #region construction

    public Position2(Vector2 value)
    {
        this.value = value;
    }

    public Position2(float x, float y)
        : this(new Vector2(x, y))
    {
    }

    public Position2(Unit x, Unit y)
        : this(new Vector2(x.NumericValue, y.NumericValue))
    {
    }

    #endregion

    /// <summary>
    /// Returns the numeric vector value of the position vector.
    /// </summary>
    public Vector2 NumericValue => value;

    /// <summary>
    /// Returns the X component of the position vector.
    /// </summary>
    public Unit X => new Unit(value.X);

    /// <summary>
    /// Returns the Y component of the position vector.
    /// </summary>
    public Unit Y => new Unit(value.Y);

    /// <summary>
    /// Returns a Position2 type with value 0.
    /// </summary>
    public static Position2 Zero => new Position2(0, 0);

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed position vectors.
    /// </summary>
    /// <param name="p0">The position vector at t = 0.</param>
    /// <param name="p1">The position vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Position2 Lerp(Position2 p0, Position2 p1, float t) => p0 + (p1 - p0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed position vector.
    /// </summary>
    /// <param name="p">The position vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Position2 LerpTo(Position2 p, float t) => Lerp(this, p, t);

    #endregion

    #region equality and hashcode

    public bool Equals(Position2 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Position2 position2 && Equals(position2);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"({value.X.ToString(format, formatProvider)}, {value.Y.ToString(format, formatProvider)}) u";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region Difference2 interaction

    /// <summary>
    /// Adds a difference vector to an absolute position.
    /// </summary>
    public static Position2 operator +(Position2 p, Difference2 d) => new Position2(p.value + d.NumericValue);

    /// <summary>
    /// Adds a difference vector to an absolute position.
    /// </summary>
    public static Position2 operator +(Difference2 d, Position2 p) => new Position2(p.value + d.NumericValue);

    /// <summary>
    /// Subtracts a difference vector from an absolute position.
    /// </summary>
    public static Position2 operator -(Position2 p, Difference2 d) => new Position2(p.value - d.NumericValue);

    /// <summary>
    /// Subtracts two absolute positions, returning a difference vector.
    /// </summary>
    public static Difference2 operator -(Position2 p0, Position2 p1) => new Difference2(p0.value - p1.value);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two position vectors for equality.
    /// </summary>
    public static bool operator ==(Position2 p0, Position2 p1) => p0.Equals(p1);

    /// <summary>
    /// Compares two position vectors for inequality.
    /// </summary>
    public static bool operator !=(Position2 p0, Position2 p1) => !(p0 == p1);

    #endregion

    #endregion

}
