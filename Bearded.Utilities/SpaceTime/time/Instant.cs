using System;
using System.Globalization;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of an absolute instant in time.
/// </summary>
public readonly struct Instant : IEquatable<Instant>, IComparable<Instant>, IFormattable
{
    private readonly double value;

    #region construction

    public Instant(double value)
    {
        this.value = value;
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the time instant.
    /// </summary>
    public double NumericValue => value;

    /// <summary>
    /// Returns an Instant type with value 0.
    /// </summary>
    public static Instant Zero => new Instant(0);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Instant other) => value == other.value;

    public override bool Equals(object? obj) => obj is Instant instant && Equals(instant);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region compare

    public int CompareTo(Instant other) => value.CompareTo(other.value);

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region TimeSpan interaction

    /// <summary>
    /// Adds a timespan to a time instant.
    /// </summary>
    public static Instant operator +(Instant i, TimeSpan t) => new Instant(i.value + t.NumericValue);

    /// <summary>
    /// Adds a timespan to a time instant.
    /// </summary>
    public static Instant operator +(TimeSpan t, Instant i) => new Instant(i.value + t.NumericValue);

    /// <summary>
    /// Subtracts a timespan from a time instant.
    /// </summary>
    public static Instant operator -(Instant i, TimeSpan t) => new Instant(i.value - t.NumericValue);

    /// <summary>
    /// Subtracts two time instants, returning a timespan.
    /// </summary>
    public static TimeSpan operator -(Instant i0, Instant i1) => new TimeSpan(i0.value - i1.value);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two time instants for equality.
    /// </summary>
    public static bool operator ==(Instant i0, Instant i1) => i0.Equals(i1);

    /// <summary>
    /// Compares two time instants for inequality.
    /// </summary>
    public static bool operator !=(Instant i0, Instant i1) => !(i0 == i1);

    /// <summary>
    /// Checks if one time instant is smaller than another.
    /// </summary>
    public static bool operator <(Instant i0, Instant i1) => i0.value < i1.value;

    /// <summary>
    /// Checks if one time instant is larger than another.
    /// </summary>
    public static bool operator >(Instant i0, Instant i1) => i0.value > i1.value;

    /// <summary>
    /// Checks if one time instant is smaller or equal to another.
    /// </summary>
    public static bool operator <=(Instant i0, Instant i1) => i0.value <= i1.value;

    /// <summary>
    /// Checks if one time instant is larger or equal to another.
    /// </summary>
    public static bool operator >=(Instant i0, Instant i1) => i0.value >= i1.value;

    #endregion

    #endregion

}
