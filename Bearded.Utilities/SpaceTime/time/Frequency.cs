using System;
using System.Globalization;
using Bearded.Utilities.Geometry;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a signed frequency.
/// </summary>
public readonly struct Frequency : IEquatable<Frequency>, IComparable<Frequency>, IFormattable
{
    private readonly double value;

    #region construction

    public Frequency(double value)
    {
        this.value = value;
    }

    #endregion

    #region properties

    public double NumericValue => value;

    public static Frequency Never => new Frequency(0);

    public static Frequency One => new Frequency(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Frequency other) => value == other.value;

    public override bool Equals(object? obj) => obj is Frequency f && Equals(f);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} 1/t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region compare

    public int CompareTo(Frequency other) => value.CompareTo(other.value);

    #endregion

    #endregion

    #region operators

    #region scaling

    public static Frequency operator *(Frequency t, double scalar) => new Frequency(t.value * scalar);

    public static Frequency operator *(double scalar, Frequency t) => new Frequency(t.value * scalar);

    public static Frequency operator /(Frequency t, double divisor) => new Frequency(t.value / divisor);

    #endregion

    #region ratio

    public static double operator /(Frequency dividend, Frequency divisor) => dividend.value / divisor.value;

    #endregion

    #region comparison

    public static bool operator ==(Frequency f0, Frequency f1) => f0.Equals(f1);

    public static bool operator !=(Frequency f0, Frequency f1) => !(f0 == f1);

    public static bool operator <(Frequency f0, Frequency f1) => f0.value < f1.value;

    public static bool operator >(Frequency f0, Frequency f1) => f0.value > f1.value;

    public static bool operator <=(Frequency f0, Frequency f1) => f0.value <= f1.value;

    public static bool operator >=(Frequency f0, Frequency f1) => f0.value >= f1.value;

    #endregion

    #region angle differentiation

    public static AngularVelocity operator *(Angle s, Frequency f)
        => AngularVelocity.FromRadians(s.Radians * (float)f.value);

    #endregion

    #region TimeSpan interaction

    public static double operator *(Frequency f, TimeSpan t) => f.value * t.NumericValue;

    public static double operator *(TimeSpan t, Frequency f) => f.value * t.NumericValue;

    public static TimeSpan operator /(double d, Frequency f) => new TimeSpan(d / f.value);

    #endregion

    #endregion

    #region static methods

    public static Frequency Min(Frequency f1, Frequency t2)
        => new Frequency(Math.Min(f1.NumericValue, t2.NumericValue));

    public static Frequency Max(Frequency f1, Frequency t2)
        => new Frequency(Math.Max(f1.NumericValue, t2.NumericValue));

    #endregion
}
