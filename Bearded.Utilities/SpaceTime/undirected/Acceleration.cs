using System;
using System.Globalization;
using Bearded.Utilities.Geometry;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of an undirected signed acceleration.
/// </summary>
public readonly struct Acceleration : IEquatable<Acceleration>, IComparable<Acceleration>, IFormattable
{
    private readonly float value;

    #region construction

    public Acceleration(float value)
    {
        this.value = value;
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the acceleration value.
    /// </summary>
    public float NumericValue => value;

    /// <summary>
    /// Returns the type-safe square of the acceleration value.
    /// </summary>
    public Squared<Acceleration> Squared => Squared<Acceleration>.FromRoot(value);

    /// <summary>
    /// Returns an Acceleration type with value 0.
    /// </summary>
    public static Acceleration Zero => new Acceleration(0);

    /// <summary>
    /// Returns an Acceleration type with value 1.
    /// </summary>
    public static Acceleration One => new Acceleration(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Acceleration other) => value == other.value;

    public override bool Equals(object? obj) => obj is Acceleration acceleration && Equals(acceleration);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} u/t²";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region compare

    public int CompareTo(Acceleration other) => value.CompareTo(other.value);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two acceleration values.
    /// </summary>
    public static Acceleration operator +(Acceleration a0, Acceleration a1) => new Acceleration(a0.value + a1.value);

    /// <summary>
    /// Subtracts an acceleration value from another.
    /// </summary>
    public static Acceleration operator -(Acceleration a0, Acceleration a1) => new Acceleration(a0.value - a1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the acceleration value.
    /// </summary>
    public static Acceleration operator -(Acceleration a) => new Acceleration(-a.value);

    /// <summary>
    /// Multiples the acceleration value with a scalar.
    /// </summary>
    public static Acceleration operator *(Acceleration a, float scalar) => new Acceleration(a.value * scalar);

    /// <summary>
    /// Multiples the acceleration value with a scalar.
    /// </summary>
    public static Acceleration operator *(float scalar, Acceleration a) => new Acceleration(a.value * scalar);

    /// <summary>
    /// Divides the acceleration value by a divisor.
    /// </summary>
    public static Acceleration operator /(Acceleration a, float divisor) => new Acceleration(a.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Devides an acceleration value by another, returning a type-less fraction.
    /// </summary>
    public static float operator /(Acceleration dividend, Acceleration divisor) => dividend.value / divisor.value;

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies an acceleration value by a timespan, returning a speed.
    /// </summary>
    public static Speed operator *(Acceleration a, TimeSpan t) => new Speed(a.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies an acceleration value by a timespan, returning a speed.
    /// </summary>
    public static Speed operator *(TimeSpan t, Acceleration a) => new Speed(a.value * (float)t.NumericValue);

    public static Speed operator /(Acceleration a, Frequency f) => new Speed(a.value / (float)f.NumericValue);

    #endregion

    #region add dimension

    /// <summary>
    /// Multiplies a direction with an acceleration value, returning a typed acceleration vector of the given direction and length.
    /// </summary>
    public static Acceleration2 operator *(Acceleration a, Direction2 d) => new Acceleration2(d.Vector * a.value);

    /// <summary>
    /// Multiplies a direction with an acceleration value, returning a typed acceleration vector of the given direction and length.
    /// </summary>
    public static Acceleration2 operator *(Direction2 d, Acceleration a) => new Acceleration2(d.Vector * a.value);

    /// <summary>
    /// Multiplies an acceleration value with an untyped vector, returning a typed acceleration vector.
    /// </summary>
    public static Acceleration2 operator *(Acceleration u, Vector2 v) => new Acceleration2(v * u.value);

    /// <summary>
    /// Multiplies an acceleration value with an untyped vector, returning a typed acceleration vector.
    /// </summary>
    public static Acceleration2 operator *(Vector2 v, Acceleration u) => new Acceleration2(v * u.value);

    #endregion

    #region add two dimensions

    /// <summary>
    /// Multiplies an acceleration value with an untyped vector, returning a typed acceleration vector.
    /// </summary>
    public static Acceleration3 operator *(Acceleration u, Vector3 v) => new Acceleration3(v * u.value);

    /// <summary>
    /// Multiplies an acceleration value with an untyped vector, returning a typed acceleration vector.
    /// </summary>
    public static Acceleration3 operator *(Vector3 v, Acceleration u) => new Acceleration3(v * u.value);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two acceleration values for equality.
    /// </summary>
    public static bool operator ==(Acceleration a0, Acceleration a1) => a0.Equals(a1);

    /// <summary>
    /// Compares two acceleration values for inequality.
    /// </summary>
    public static bool operator !=(Acceleration a0, Acceleration a1) => !(a0 == a1);

    /// <summary>
    /// Checks if one acceleration value is smaller than another.
    /// </summary>
    public static bool operator <(Acceleration a0, Acceleration a1) => a0.value < a1.value;

    /// <summary>
    /// Checks if one acceleration value is larger than another.
    /// </summary>
    public static bool operator >(Acceleration a0, Acceleration a1) => a0.value > a1.value;

    /// <summary>
    /// Checks if one acceleration value is smaller or equal to another.
    /// </summary>
    public static bool operator <=(Acceleration a0, Acceleration a1) => a0.value <= a1.value;

    /// <summary>
    /// Checks if one acceleration value is larger or equal to another.
    /// </summary>
    public static bool operator >=(Acceleration a0, Acceleration a1) => a0.value >= a1.value;

    #endregion

    #endregion

    #region static methods

    public static Acceleration Min(Acceleration s1, Acceleration s2)
        => new Acceleration(Math.Min(s1.NumericValue, s2.NumericValue));

    public static Acceleration Max(Acceleration s1, Acceleration s2)
        => new Acceleration(Math.Max(s1.NumericValue, s2.NumericValue));

    #endregion

}
