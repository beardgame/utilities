using System;
using System.Globalization;
using Bearded.Utilities.Geometry;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of an undirected signed distance or length.
/// </summary>
public readonly struct Unit : IEquatable<Unit>, IComparable<Unit>, IFormattable
{
    private readonly float value;

    #region construction

    public Unit(float value)
    {
        this.value = value;
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the unit value.
    /// </summary>
    public float NumericValue => value;

    /// <summary>
    /// Returns the type-safe square of the unit value.
    /// </summary>
    public Squared<Unit> Squared => Squared<Unit>.FromRoot(value);

    /// <summary>
    /// Returns a Unit type with value 0.
    /// </summary>
    public static Unit Zero => new Unit(0);

    /// <summary>
    /// Returns a Unit type with value 1.
    /// </summary>
    public static Unit One => new Unit(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Unit other) => value == other.value;

    public override bool Equals(object? obj) => obj is Unit unit && Equals(unit);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region compare

    public int CompareTo(Unit other) => value.CompareTo(other.value);

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} u";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two unit values.
    /// </summary>
    public static Unit operator +(Unit u0, Unit u1) => new Unit(u0.value + u1.value);

    /// <summary>
    /// Subtracts a unit value from another.
    /// </summary>
    public static Unit operator -(Unit u0, Unit u1) => new Unit(u0.value - u1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the unit value.
    /// </summary>
    public static Unit operator -(Unit u) => new Unit(-u.value);

    /// <summary>
    /// Multiples the unit value with a scalar.
    /// </summary>
    public static Unit operator *(Unit u, float scalar) => new Unit(u.value * scalar);

    /// <summary>
    /// Multiples the unit value with a scalar.
    /// </summary>
    public static Unit operator *(float scalar, Unit u) => new Unit(u.value * scalar);

    /// <summary>
    /// Divides the unit value by a divisor.
    /// </summary>
    public static Unit operator /(Unit u, float divisor) => new Unit(u.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Devides a unit value by another, returning a type-less fraction.
    /// </summary>
    public static float operator /(Unit dividend, Unit divisor) => dividend.value / divisor.value;

    #endregion

    #region differentiate

    /// <summary>
    /// Divides a unit value by a timespan, returning a speed.
    /// </summary>
    public static Speed operator /(Unit u, TimeSpan t) => new Speed(u.value / (float)t.NumericValue);

    public static Speed operator *(Unit u, Frequency f) => new Speed(u.value * (float)f.NumericValue);

    public static Speed operator *(Frequency f, Unit u) => new Speed(u.value * (float)f.NumericValue);

    #endregion

    #region add dimension

    /// <summary>
    /// Multiplies a direction with a unit value, returning a typed vector of the given direction and length.
    /// </summary>
    public static Difference2 operator *(Unit u, Direction2 d) => new Difference2(d.Vector * u.value);

    /// <summary>
    /// Multiplies a direction with a unit value, returning a typed vector of the given direction and length.
    /// </summary>
    public static Difference2 operator *(Direction2 d, Unit u) => new Difference2(d.Vector * u.value);

    /// <summary>
    /// Multiplies a unit value with an untyped vector, returning a typed vector.
    /// </summary>
    public static Difference2 operator *(Unit u, Vector2 v) => new Difference2(v * u.value);

    /// <summary>
    /// Multiplies a unit value with an untyped vector, returning a typed vector.
    /// </summary>
    public static Difference2 operator *(Vector2 v, Unit u) => new Difference2(v * u.value);

    #endregion

    #region add two dimensions

    /// <summary>
    /// Multiplies a unit value with an untyped vector, returning a typed vector.
    /// </summary>
    public static Difference3 operator *(Unit u, Vector3 v) => new Difference3(v * u.value);

    /// <summary>
    /// Multiplies a unit value with an untyped vector, returning a typed vector.
    /// </summary>
    public static Difference3 operator *(Vector3 v, Unit u) => new Difference3(v * u.value);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two unit values for equality.
    /// </summary>
    public static bool operator ==(Unit u0, Unit u1) => u0.Equals(u1);

    /// <summary>
    /// Compares two unit values for inequality.
    /// </summary>
    public static bool operator !=(Unit u0, Unit u1) => !(u0 == u1);

    /// <summary>
    /// Checks if one unit value is smaller than another.
    /// </summary>
    public static bool operator <(Unit u0, Unit u1) => u0.value < u1.value;

    /// <summary>
    /// Checks if one unit value is larger than another.
    /// </summary>
    public static bool operator >(Unit u0, Unit u1) => u0.value > u1.value;

    /// <summary>
    /// Checks if one unit value is smaller or equal to another.
    /// </summary>
    public static bool operator <=(Unit u0, Unit u1) => u0.value <= u1.value;

    /// <summary>
    /// Checks if one unit value is larger or equal to another.
    /// </summary>
    public static bool operator >=(Unit u0, Unit u1) => u0.value >= u1.value;

    #endregion

    #endregion

    #region static methods

    public static Unit Min(Unit u1, Unit u2)
        => new Unit(Math.Min(u1.NumericValue, u2.NumericValue));

    public static Unit Max(Unit u1, Unit u2)
        => new Unit(Math.Max(u1.NumericValue, u2.NumericValue));

    #endregion

}
