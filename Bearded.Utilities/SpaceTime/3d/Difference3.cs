using System;
using System.Globalization;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a 3d directed difference vector.
/// </summary>
public readonly struct Difference3 : IEquatable<Difference3>, IFormattable
{
    private readonly Vector3 value;

    #region constructing

    public Difference3(Vector3 value)
    {
        this.value = value;
    }

    public Difference3(float x, float y, float z)
        : this(new Vector3(x, y, z))
    {
    }

    public Difference3(Unit x, Unit y, Unit z)
        : this(new Vector3(x.NumericValue, y.NumericValue, z.NumericValue))
    {
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric vector value of the difference vector.
    /// </summary>
    public Vector3 NumericValue => value;

    /// <summary>
    /// Returns the X component of the difference vector.
    /// </summary>
    public Unit X => new Unit(value.X);

    /// <summary>
    /// Returns the Y component of the difference vector.
    /// </summary>
    public Unit Y => new Unit(value.Y);

    /// <summary>
    /// Returns the Z component of the difference vector.
    /// </summary>
    public Unit Z => new Unit(value.Z);

    /// <summary>
    /// Returns the typed magnitude of the difference vector.
    /// </summary>
    public Unit Length => new Unit(value.Length);

    /// <summary>
    /// Returns the typed square of the magnitude of the difference vector.
    /// </summary>
    public Squared<Unit> LengthSquared => new Squared<Unit>(value.LengthSquared);

    /// <summary>
    /// Returns a Difference3 type with value 0.
    /// </summary>
    public static Difference3 Zero => new Difference3(0, 0, 0);

    #endregion

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed difference vectors.
    /// </summary>
    /// <param name="d0">The difference vector at t = 0.</param>
    /// <param name="d1">The difference vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Difference3 Lerp(Difference3 d0, Difference3 d1, float t) => d0 + (d1 - d0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed difference vector.
    /// </summary>
    /// <param name="d">The difference vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Difference3 LerpTo(Difference3 d, float t) => Lerp(this, d, t);

    #endregion

    #region projection

    /// <summary>
    /// Projects the difference vector onto an untyped vector, returning the speed component in that vector's direction.
    /// </summary>
    public Unit ProjectedOn(Vector3 vector) => projectedOn(vector.NormalizedSafe());

    /// <summary>
    /// Projects the difference vector onto a difference vector, returning the speed component in that vector's direction.
    /// </summary>
    public Unit ProjectedOn(Difference3 vector) => projectedOn(vector.NumericValue.NormalizedSafe());

    private Unit projectedOn(Vector3 normalisedVector) => new Unit(Vector3.Dot(value, normalisedVector));

    #endregion

    #region equality and hashcode

    public bool Equals(Difference3 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Difference3 diff && Equals(diff);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) => "(" +
        $"{value.X.ToString(format, formatProvider)}, " +
        $"{value.Y.ToString(format, formatProvider)}, " +
        $"{value.Z.ToString(format, formatProvider)}) u";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two difference vectors.
    /// </summary>
    public static Difference3 operator +(Difference3 d0, Difference3 d1) => new Difference3(d0.value + d1.value);

    /// <summary>
    /// Subtracts a difference vector from another.
    /// </summary>
    public static Difference3 operator -(Difference3 d0, Difference3 d1) => new Difference3(d0.value - d1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the difference vector.
    /// </summary>
    public static Difference3 operator -(Difference3 d) => new Difference3(-d.value);

    /// <summary>
    /// Multiplies the difference vector with a scalar.
    /// </summary>
    public static Difference3 operator *(Difference3 d, float scalar) => new Difference3(d.value * scalar);

    /// <summary>
    /// Multiplies the difference vector with a scalar.
    /// </summary>
    public static Difference3 operator *(float scalar, Difference3 d) => new Difference3(d.value * scalar);

    /// <summary>
    /// Divides the difference vector by a divisor.
    /// </summary>
    public static Difference3 operator /(Difference3 d, float divisor) => new Difference3(d.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a difference vector by a speed, returning an untyped vector.
    /// </summary>
    public static Vector3 operator /(Difference3 d, Unit divisor) => d.value / divisor.NumericValue;

    #endregion

    #region differentiate

    /// <summary>
    /// Divides a difference vector by a timespan, returning an velocity vector.
    /// </summary>
    public static Velocity3 operator /(Difference3 d, TimeSpan t) =>
        new Velocity3(d.value / (float)t.NumericValue);

    public static Velocity3 operator *(Difference3 d, Frequency f) =>
        new Velocity3(d.value * (float)f.NumericValue);

    public static Velocity3 operator *(Frequency f, Difference3 d) =>
        new Velocity3(d.value * (float)f.NumericValue);

    #endregion

    #region comparison

    /// <summary>
    /// Compares two difference vectors for equality.
    /// </summary>
    public static bool operator ==(Difference3 d0, Difference3 d1) => d0.Equals(d1);

    /// <summary>
    /// Compares two difference vectors for inequality.
    /// </summary>
    public static bool operator !=(Difference3 d0, Difference3 d1) => !(d0 == d1);

    #endregion

    #endregion
}
