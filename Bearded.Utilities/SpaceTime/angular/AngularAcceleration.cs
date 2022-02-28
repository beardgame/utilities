using System;
using System.Globalization;
using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a signed angular acceleration.
/// </summary>
public readonly struct AngularAcceleration : IEquatable<AngularAcceleration>, IComparable<AngularAcceleration>, IFormattable
{
    private readonly float value;

    #region constructing

    public AngularAcceleration(Angle value)
        : this(value.Radians)
    {
    }

    private AngularAcceleration(float value)
    {
        this.value = value;
    }

    /// <summary>
    /// Creates a new instance of the AngularAcceleration type from an angle in radians.
    /// </summary>
    public static AngularAcceleration FromRadians(float radians) => new AngularAcceleration(radians);

    /// <summary>
    /// Creates a new instance of the AngularAcceleration type from an angle in degrees.
    /// </summary>
    public static AngularAcceleration FromDegrees(float degrees) =>
        new AngularAcceleration(MoreMath.DegreesToRadians(degrees));

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the angular acceleration in radians.
    /// </summary>
    public float NumericValue => value;

    /// <summary>
    /// Returns the angular value of the angular acceleration.
    /// </summary>
    public Angle AngleValue => Angle.FromRadians(value);

    /// <summary>
    /// Returns an angular acceleration with value 0.
    /// </summary>
    public static AngularAcceleration Zero => new AngularAcceleration(0);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(AngularAcceleration other) => value == other.value;

    public override bool Equals(object? obj) => obj is AngularAcceleration angularAcceleration && Equals(angularAcceleration);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{MoreMath.RadiansToDegrees(value).ToString(format, formatProvider)} °/t²";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region compare

    public int CompareTo(AngularAcceleration other) => value.CompareTo(other.value);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two angular acceleration values.
    /// </summary>
    public static AngularAcceleration operator +(AngularAcceleration v0, AngularAcceleration v1) => new AngularAcceleration(v0.value + v1.value);

    /// <summary>
    /// Adds two angular acceleration values.
    /// </summary>
    public static AngularAcceleration operator -(AngularAcceleration v0, AngularAcceleration v1) => new AngularAcceleration(v0.value - v1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the angular acceleration.
    /// </summary>
    public static AngularAcceleration operator -(AngularAcceleration s) => new AngularAcceleration(-s.value);

    /// <summary>
    /// Multiplies the angular acceleration with a scalar.
    /// </summary>
    public static AngularAcceleration operator *(AngularAcceleration s, float scalar) => new AngularAcceleration(s.value * scalar);

    /// <summary>
    /// Multiplies the angular acceleration with a scalar.
    /// </summary>
    public static AngularAcceleration operator *(float scalar, AngularAcceleration s) => new AngularAcceleration(s.value * scalar);

    /// <summary>
    /// Divides the angular acceleration by a divisor.
    /// </summary>
    public static AngularAcceleration operator /(AngularAcceleration s, float divisor) => new AngularAcceleration(s.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides an angular acceleration by another, returning a type-less fraction.
    /// </summary>
    public static float operator /(AngularAcceleration dividend, AngularAcceleration divisor) => dividend.value / divisor.value;

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies an angular acceleration by a timespan, returning an anglular velocity.
    /// </summary>
    public static AngularVelocity operator *(AngularAcceleration s, TimeSpan t) => AngularVelocity.FromRadians(s.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies an angular acceleration by a timespan, returning an anglular velocity.
    /// </summary>
    public static AngularVelocity operator *(TimeSpan t, AngularAcceleration s) => AngularVelocity.FromRadians(s.value * (float)t.NumericValue);

    public static AngularVelocity operator /(AngularAcceleration a, Frequency f) => AngularVelocity.FromRadians(a.value / (float)f.NumericValue);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two angular accelerations for equality.
    /// </summary>
    public static bool operator ==(AngularAcceleration v0, AngularAcceleration v1) => v0.Equals(v1);

    /// <summary>
    /// Compares two angular accelerations for inequality.
    /// </summary>
    public static bool operator !=(AngularAcceleration v0, AngularAcceleration v1) => !(v0 == v1);

    /// <summary>
    /// Checks if one angular acceleration is smaller than another.
    /// </summary>
    public static bool operator <(AngularAcceleration v0, AngularAcceleration v1) => v0.value < v1.value;

    /// <summary>
    /// Checks if one angular acceleration is larger than another.
    /// </summary>
    public static bool operator >(AngularAcceleration v0, AngularAcceleration v1) => v0.value > v1.value;

    /// <summary>
    /// Checks if one angular acceleration is smaller or equal to another.
    /// </summary>
    public static bool operator <=(AngularAcceleration v0, AngularAcceleration v1) => v0.value <= v1.value;

    /// <summary>
    /// Checks if one angular acceleration is larger or equal to another.
    /// </summary>
    public static bool operator >=(AngularAcceleration v0, AngularAcceleration v1) => v0.value >= v1.value;

    #endregion

    #endregion

}
