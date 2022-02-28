using System;
using System.Globalization;

namespace Bearded.Utilities.SpaceTime;

public readonly struct Mass : IEquatable<Mass>, IComparable<Mass>, IFormattable
{
    private readonly float value;

    public Mass(float value)
    {
        this.value = value;
    }

    #region properties

    public float NumericValue => value;

    public static Mass Zero => new Mass(0);

    public static Mass One => new Mass(1);

    #endregion

    #region methods

    #region equality and hashcode

    // ReSharper disable once CompareOfFloatsByEqualityOperator
    public bool Equals(Mass other) => value == other.value;

    public override bool Equals(object? obj) => obj is Mass mass && Equals(mass);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"{value.ToString(format, formatProvider)} m";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #region compare

    public int CompareTo(Mass other) => value.CompareTo(other.value);

    #endregion

    #endregion

    #region operators

    #region algebra

    public static Mass operator +(Mass m0, Mass m1) => new Mass(m0.value + m1.value);

    public static Mass operator -(Mass m0, Mass m1) => new Mass(m0.value - m1.value);

    public static Mass operator -(Mass m) => new Mass(-m.value);

    #endregion

    #region scaling

    public static Mass operator *(Mass m, float scalar) => new Mass(m.value * scalar);

    public static Mass operator *(float scalar, Mass m) => new Mass(m.value * scalar);

    public static Mass operator /(Mass m, float divisor) => new Mass(m.value / divisor);

    #endregion

    #region ratio

    public static float operator /(Mass dividend, Mass divisor) => dividend.value / divisor.value;

    #endregion

    #region comparison

    public static bool operator ==(Mass m0, Mass m1) => m0.Equals(m1);

    public static bool operator !=(Mass m0, Mass m1) => !(m0 == m1);

    public static bool operator <(Mass m0, Mass m1) => m0.value < m1.value;

    public static bool operator >(Mass m0, Mass m1) => m0.value > m1.value;

    public static bool operator <=(Mass m0, Mass m1) => m0.value <= m1.value;

    public static bool operator >=(Mass m0, Mass m1) => m0.value >= m1.value;

    #endregion

    #endregion

    #region static methods

    public static Mass Min(Mass m0, Mass m1)
        => new Mass(Math.Min(m0.NumericValue, m1.NumericValue));

    public static Mass Max(Mass m0, Mass m1)
        => new Mass(Math.Max(m0.NumericValue, m1.NumericValue));

    #endregion
}
