using System;
using System.Globalization;
using Bearded.Utilities.Geometry;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of an undirected signed speed.
/// </summary>
public readonly struct Speed : IEquatable<Speed>, IComparable<Speed>, IFormattable
{
    private readonly float value;

    #region construction

    public Speed(float value)
    {
        this.value = value;
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the speed value.
    /// </summary>
    public float NumericValue => value;

    /// <summary>
    /// Returns the type-safe square of the speed value.
    /// </summary>
    public Squared<Speed> Squared => Squared<Speed>.FromRoot(value);

    /// <summary>
    /// Returns a Speed type with value 0.
    /// </summary>
    public static Speed Zero => new Speed(0);

    /// <summary>
    /// Returns a Speed type with value 1.
    /// </summary>
    public static Speed One => new Speed(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Speed other) => value == other.value;

    public override bool Equals(object? obj) => obj is Speed speed && Equals(speed);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region compare

    public int CompareTo(Speed other) => value.CompareTo(other.value);

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} u/t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two speed values.
    /// </summary>
    public static Speed operator +(Speed s0, Speed s1) => new Speed(s0.value + s1.value);

    /// <summary>
    /// Subtracts a speed value from another.
    /// </summary>
    public static Speed operator -(Speed s0, Speed s1) => new Speed(s0.value - s1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the speed value.
    /// </summary>
    public static Speed operator -(Speed s) => new Speed(-s.value);

    /// <summary>
    /// Multiplies the speed value with a scalar.
    /// </summary>
    public static Speed operator *(Speed s, float scalar) => new Speed(s.value * scalar);

    /// <summary>
    /// Multiplies the speed value with a scalar.
    /// </summary>
    public static Speed operator *(float scalar, Speed s) => new Speed(s.value * scalar);

    /// <summary>
    /// Divides the speed value by a divisor.
    /// </summary>
    public static Speed operator /(Speed s, float divisor) => new Speed(s.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a speed value by another, returning a type-less fraction.
    /// </summary>
    public static float operator /(Speed dividend, Speed divisor) => dividend.value / divisor.value;

    #endregion

    #region differentiate

    /// <summary>
    /// Divides a speed value by a timespan, returning an acceleration.
    /// </summary>
    public static Acceleration operator /(Speed s, TimeSpan t) => new Acceleration(s.value / (float)t.NumericValue);

    public static Acceleration operator *(Speed s, Frequency f) => new Acceleration(s.value * (float)f.NumericValue);

    public static Acceleration operator *(Frequency f, Speed s) => new Acceleration(s.value * (float)f.NumericValue);

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies a speed value by a timespan, returning a unit value.
    /// </summary>
    public static Unit operator *(Speed s, TimeSpan t) => new Unit(s.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies a speed value by a timespan, returning a unit value.
    /// </summary>
    public static Unit operator *(TimeSpan t, Speed s) => new Unit(s.value * (float)t.NumericValue);

    public static Unit operator /(Speed s, Frequency f) => new Unit(s.value / (float)f.NumericValue);

    #endregion

    #region add dimension

    /// <summary>
    /// Multiplies a direction with a speed value, returning a typed speed vector of the given direction and length.
    /// </summary>
    public static Velocity2 operator *(Speed s, Direction2 d) => new Velocity2(d.Vector * s.value);

    /// <summary>
    /// Multiplies a direction with a speed value, returning a typed speed vector of the given direction and length.
    /// </summary>
    public static Velocity2 operator *(Direction2 d, Speed s) => new Velocity2(d.Vector * s.value);

    /// <summary>
    /// Multiplies a speed value with an untyped vector, returning a typed speed vector.
    /// </summary>
    public static Velocity2 operator *(Speed u, Vector2 v) => new Velocity2(v * u.value);

    /// <summary>
    /// Multiplies a speed value with an untyped vector, returning a typed speed vector.
    /// </summary>
    public static Velocity2 operator *(Vector2 v, Speed u) => new Velocity2(v * u.value);

    #endregion

    #region add two dimensions

    /// <summary>
    /// Multiplies a speed value with an untyped vector, returning a typed speed vector.
    /// </summary>
    public static Velocity3 operator *(Speed u, Vector3 v) => new Velocity3(v * u.value);

    /// <summary>
    /// Multiplies a speed value with an untyped vector, returning a typed speed vector.
    /// </summary>
    public static Velocity3 operator *(Vector3 v, Speed u) => new Velocity3(v * u.value);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two speed values for equality.
    /// </summary>
    public static bool operator ==(Speed s0, Speed s1) => s0.Equals(s1);

    /// <summary>
    /// Compares two speed values for inequality.
    /// </summary>
    public static bool operator !=(Speed s0, Speed s1) => !(s0 == s1);

    /// <summary>
    /// Checks if one speed value is smaller than another.
    /// </summary>
    public static bool operator <(Speed s0, Speed s1) => s0.value < s1.value;

    /// <summary>
    /// Checks if one speed value is larger than another.
    /// </summary>
    public static bool operator >(Speed s0, Speed s1) => s0.value > s1.value;

    /// <summary>
    /// Checks if one speed value is smaller or equal to another.
    /// </summary>
    public static bool operator <=(Speed s0, Speed s1) => s0.value <= s1.value;

    /// <summary>
    /// Checks if one speed value is larger or equal to another.
    /// </summary>
    public static bool operator >=(Speed s0, Speed s1) => s0.value >= s1.value;

    #endregion

    #endregion

    #region static methods

    public static Speed Min(Speed s1, Speed s2)
        => new Speed(Math.Min(s1.NumericValue, s2.NumericValue));

    public static Speed Max(Speed s1, Speed s2)
        => new Speed(Math.Max(s1.NumericValue, s2.NumericValue));

    #endregion

}
