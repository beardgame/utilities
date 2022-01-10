using System;
using System.Globalization;
using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a signed timespan.
/// </summary>
public readonly struct TimeSpan : IEquatable<TimeSpan>, IComparable<TimeSpan>, IFormattable
{
    private readonly double value;

    #region construction

    public TimeSpan(double value)
    {
        this.value = value;
    }

    #endregion

    #region properties

    public double NumericValue => value;

    public static TimeSpan Zero => new TimeSpan(0);

    public static TimeSpan One => new TimeSpan(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(TimeSpan other) => value == other.value;

    public override bool Equals(object? obj) => obj is TimeSpan t && Equals(t);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region compare

    public int CompareTo(TimeSpan other) => value.CompareTo(other.value);

    #endregion

    #endregion

    #region operators

    #region algebra

    public static TimeSpan operator +(TimeSpan t0, TimeSpan t1) => new TimeSpan(t0.value + t1.value);

    public static TimeSpan operator -(TimeSpan t0, TimeSpan t1) => new TimeSpan(t0.value - t1.value);

    public static TimeSpan operator -(TimeSpan t) => new TimeSpan(-t.value);

    #endregion

    #region scaling

    public static TimeSpan operator *(TimeSpan t, double scalar) => new TimeSpan(t.value * scalar);

    public static TimeSpan operator *(double scalar, TimeSpan t) => new TimeSpan(t.value * scalar);

    public static TimeSpan operator /(TimeSpan t, double divisor) => new TimeSpan(t.value / divisor);

    #endregion

    #region ratio

    public static double operator /(TimeSpan dividend, TimeSpan divisor) => dividend.value / divisor.value;

    #endregion

    #region comparison

    public static bool operator ==(TimeSpan t0, TimeSpan t1) => t0.Equals(t1);

    public static bool operator !=(TimeSpan t0, TimeSpan t1) => !(t0 == t1);

    public static bool operator <(TimeSpan t0, TimeSpan t1) => t0.value < t1.value;

    public static bool operator >(TimeSpan t0, TimeSpan t1) => t0.value > t1.value;

    public static bool operator <=(TimeSpan t0, TimeSpan t1) => t0.value <= t1.value;

    public static bool operator >=(TimeSpan t0, TimeSpan t1) => t0.value >= t1.value;

    #endregion

    #region angle differentiation

    public static AngularVelocity operator /(Angle s, TimeSpan t)
        => AngularVelocity.FromRadians(s.Radians / (float)t.value);

    #endregion

    #region Frequency interaction

    public static Frequency operator /(double d, TimeSpan t) => new Frequency(d / t.NumericValue);

    #endregion

    #endregion

    #region static methods

    public static TimeSpan Min(TimeSpan t1, TimeSpan t2)
        => new TimeSpan(Math.Min(t1.NumericValue, t2.NumericValue));

    public static TimeSpan Max(TimeSpan t1, TimeSpan t2)
        => new TimeSpan(Math.Max(t1.NumericValue, t2.NumericValue));

    #endregion
}
