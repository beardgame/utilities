using System;
using System.Globalization;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// Represents a type-safe squared value, backed by a float.
/// </summary>
/// <typeparam name="T">The squared type.</typeparam>
public readonly struct Squared<T> : IEquatable<Squared<T>>, IComparable<Squared<T>>, IFormattable
    where T : struct
{
    private readonly float value;

    #region construcing

    internal Squared(float value)
    {
        this.value = value;
    }

    /// <summary>
    /// Creates a new instance of the Squared type, from a given root value.
    /// </summary>
    public static Squared<T> FromRoot(float root)
    {
        return new Squared<T>(root.Squared());
    }

    /// <summary>
    /// Creates a new instance of the Squared type, from a given value.
    /// </summary>
    /// <exception cref="ArgumentOutOfRangeException">If value is negative.</exception>
    public static Squared<T> FromValue(float value)
    {
        if (value < 0)
            throw new ArgumentOutOfRangeException(nameof(value), "Must be non-negative.");

        return new Squared<T>(value);
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric value of the square.
    /// </summary>
    public float NumericValue => value;

    /// <summary>
    /// Returns a Square type of value 0.
    /// </summary>
    public static Squared<T> Zero => new Squared<T>(0);

    /// <summary>
    /// Returns a Square type of value 1.
    /// </summary>
    public static Squared<T> One => new Squared<T>(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Squared<T> other) => value == other.value;

    public override bool Equals(object? obj) => obj is Squared<T> squared && Equals(squared);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region compare

    public int CompareTo(Squared<T> other) => value.CompareTo(other.value);

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"|{value.ToString(format, formatProvider)}|";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two squares.
    /// </summary>
    public static Squared<T> operator +(Squared<T> s0, Squared<T> s1) => new Squared<T>(s0.value + s1.value);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a square by another, returning a type-less fraction.
    /// </summary>
    public static float operator /(Squared<T> dividend, Squared<T> divisor) => dividend.value / divisor.value;

    #endregion

    #region comparision

    /// <summary>
    /// Compares two squares for equality.
    /// </summary>
    public static bool operator ==(Squared<T> s0, Squared<T> s1) => s0.Equals(s1);

    /// <summary>
    /// Compares two squares for inequality.
    /// </summary>
    public static bool operator !=(Squared<T> s0, Squared<T> s1) => !(s0 == s1);

    /// <summary>
    /// Checks if one square is smaller than another.
    /// </summary>
    public static bool operator <(Squared<T> s0, Squared<T> s1) => s0.value < s1.value;

    /// <summary>
    /// Checks if one square is larger than another.
    /// </summary>
    public static bool operator >(Squared<T> s0, Squared<T> s1) => s0.value > s1.value;

    /// <summary>
    /// Checks if one square is smaller or equal to another.
    /// </summary>
    public static bool operator <=(Squared<T> s0, Squared<T> s1) => s0.value <= s1.value;

    /// <summary>
    /// Checks if one square is larger or equal to another.
    /// </summary>
    public static bool operator >=(Squared<T> s0, Squared<T> s1) => s0.value >= s1.value;

    #endregion

    #endregion

    #region static methods

    public static Squared<T> Min(Squared<T> s1, Squared<T> s2)
        => new Squared<T>(Math.Min(s1.NumericValue, s2.NumericValue));

    public static Squared<T> Max(Squared<T> s1, Squared<T> s2)
        => new Squared<T>(Math.Max(s1.NumericValue, s2.NumericValue));

    #endregion
}
