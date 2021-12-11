using System;
using System.Globalization;
using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a signed angular velocity.
/// </summary>
public readonly struct AngularVelocity : IEquatable<AngularVelocity>, IComparable<AngularVelocity>, IFormattable
{
    private readonly float value;

    #region constructing

    public AngularVelocity(Angle value)
        : this(value.Radians)
    {
    }

    private AngularVelocity(float value)
    {
        this.value = value;
    }

    /// <summary>
    /// Creates a new instance of the AngularVelocity type from an angle in radians.
    /// </summary>
    public static AngularVelocity FromRadians(float radians) => new AngularVelocity(radians);

    /// <summary>
    /// Creates a new instance of the AngularVelocity type from an angle in degrees.
    /// </summary>
    public static AngularVelocity FromDegrees(float degrees) =>
        new AngularVelocity(MoreMath.DegreesToRadians(degrees));

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the angular velocity in radians.
    /// </summary>
    public float NumericValue => value;

    /// <summary>
    /// Returns the angular value of the angular velocity.
    /// </summary>
    public Angle AngleValue => Angle.FromRadians(value);

    /// <summary>
    /// Returns an angular velocity with value 0.
    /// </summary>
    public static AngularVelocity Zero => new AngularVelocity(0);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(AngularVelocity other) => value == other.value;

    public override bool Equals(object? obj) => obj is AngularVelocity angularVelocity && Equals(angularVelocity);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{MoreMath.RadiansToDegrees(value).ToString(format, formatProvider)} °/t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region compare

    public int CompareTo(AngularVelocity other) => value.CompareTo(other.value);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two angular velocity values.
    /// </summary>
    public static AngularVelocity operator +(AngularVelocity v0, AngularVelocity v1) => new AngularVelocity(v0.value + v1.value);

    /// <summary>
    /// Adds two angular velocity values.
    /// </summary>
    public static AngularVelocity operator -(AngularVelocity v0, AngularVelocity v1) => new AngularVelocity(v0.value - v1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the angular velocity.
    /// </summary>
    public static AngularVelocity operator -(AngularVelocity s) => new AngularVelocity(-s.value);

    /// <summary>
    /// Multiplies the angular velocity with a scalar.
    /// </summary>
    public static AngularVelocity operator *(AngularVelocity s, float scalar) => new AngularVelocity(s.value * scalar);

    /// <summary>
    /// Multiplies the angular velocity with a scalar.
    /// </summary>
    public static AngularVelocity operator *(float scalar, AngularVelocity s) => new AngularVelocity(s.value * scalar);

    /// <summary>
    /// Divides the angular velocity by a divisor.
    /// </summary>
    public static AngularVelocity operator /(AngularVelocity s, float divisor) => new AngularVelocity(s.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides an angular velocity by another, returning a type-less fraction.
    /// </summary>
    public static float operator /(AngularVelocity dividend, AngularVelocity divisor) => dividend.value / divisor.value;

    #endregion

    #region differentiate

    /// <summary>
    /// Divides an angular velocity by a timespan, returning an angular acceleration.
    /// </summary>
    public static AngularAcceleration operator /(AngularVelocity s, TimeSpan t) => AngularAcceleration.FromRadians(s.value / (float)t.NumericValue);

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies an angular velocity by a timespan, returning an angle.
    /// </summary>
    public static Angle operator *(AngularVelocity s, TimeSpan t) => Angle.FromRadians(s.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies an angular velocity by a timespan, returning an angle.
    /// </summary>
    public static Angle operator *(TimeSpan t, AngularVelocity s) => Angle.FromRadians(s.value * (float)t.NumericValue);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two angular velocities for equality.
    /// </summary>
    public static bool operator ==(AngularVelocity v0, AngularVelocity v1) => v0.Equals(v1);

    /// <summary>
    /// Compares two angular velocities for inequality.
    /// </summary>
    public static bool operator !=(AngularVelocity v0, AngularVelocity v1) => !(v0 == v1);

    /// <summary>
    /// Checks if one angular velocity is smaller than another.
    /// </summary>
    public static bool operator <(AngularVelocity v0, AngularVelocity v1) => v0.value < v1.value;

    /// <summary>
    /// Checks if one angular velocity is larger than another.
    /// </summary>
    public static bool operator >(AngularVelocity v0, AngularVelocity v1) => v0.value > v1.value;

    /// <summary>
    /// Checks if one angular velocity is smaller or equal to another.
    /// </summary>
    public static bool operator <=(AngularVelocity v0, AngularVelocity v1) => v0.value <= v1.value;

    /// <summary>
    /// Checks if one angular velocity is larger or equal to another.
    /// </summary>
    public static bool operator >=(AngularVelocity v0, AngularVelocity v1) => v0.value >= v1.value;

    #endregion

    #endregion

}
