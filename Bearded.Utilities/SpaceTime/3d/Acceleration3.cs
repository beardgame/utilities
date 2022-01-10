using System;
using System.Globalization;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a 3d directed acceleration vector.
/// </summary>
public readonly struct Acceleration3 : IEquatable<Acceleration3>, IFormattable
{
    private readonly Vector3 value;

    #region construction

    public Acceleration3(Vector3 value)
    {
        this.value = value;
    }

    public Acceleration3(float x, float y, float z)
        : this(new Vector3(x, y, z))
    {
    }

    public Acceleration3(Acceleration x, Acceleration y, Acceleration z)
        : this(new Vector3(x.NumericValue, y.NumericValue, z.NumericValue))
    {
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric vector value of the acceleration vector.
    /// </summary>
    public Vector3 NumericValue => value;

    /// <summary>
    /// Returns the X component of the acceleration vector.
    /// </summary>
    public Acceleration X => new Acceleration(value.X);

    /// <summary>
    /// Returns the Y component of the acceleration vector.
    /// </summary>
    public Acceleration Y => new Acceleration(value.Y);

    /// <summary>
    /// Returns the Z component of the acceleration vector.
    /// </summary>
    public Acceleration Z => new Acceleration(value.Z);

    /// <summary>
    /// Returns the typed magnitude of the acceleration vector.
    /// </summary>
    public Acceleration Length => new Acceleration(value.Length);

    /// <summary>
    /// Returns the typed square of the magnitude of the acceleration vector.
    /// </summary>
    public Squared<Acceleration> LengthSquared => new Squared<Acceleration>(value.LengthSquared);

    /// <summary>
    /// Returns a Acceleration3 type with value 0.
    /// </summary>
    public static Acceleration3 Zero => new Acceleration3(0, 0, 0);

    #endregion

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed acceleration vectors.
    /// </summary>
    /// <param name="a0">The acceleration vector at t = 0.</param>
    /// <param name="a1">The acceleration vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Acceleration3 Lerp(Acceleration3 a0, Acceleration3 a1, float t) => a0 + (a1 - a0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed acceleration vector.
    /// </summary>
    /// <param name="a">The acceleration vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Acceleration3 LerpTo(Acceleration3 a, float t) => Lerp(this, a, t);

    #endregion

    #region projection

    /// <summary>
    /// Projects the acceleration vector onto an untyped vector, returning the acceleration component in that
    /// vector's direction.
    /// </summary>
    public Acceleration ProjectedOn(Vector3 vector) => projectedOn(vector.NormalizedSafe());

    /// <summary>
    /// Projects the acceleration vector onto a difference vector, returning the acceleration component in that
    /// vector's direction.
    /// </summary>
    public Acceleration ProjectedOn(Difference3 vector) => projectedOn(vector.NumericValue.NormalizedSafe());

    private Acceleration projectedOn(Vector3 normalisedVector) =>
        new Acceleration(Vector3.Dot(value, normalisedVector));

    #endregion

    #region equality and hashcode

    public bool Equals(Acceleration3 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Acceleration3 a && Equals(a);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) => "(" +
        $"{value.X.ToString(format, formatProvider)}, " +
        $"{value.Y.ToString(format, formatProvider)}, " +
        $"{value.Z.ToString(format, formatProvider)}) u/t²";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two acceleration vectors.
    /// </summary>
    public static Acceleration3 operator +(Acceleration3 a0, Acceleration3 a1) =>
        new Acceleration3(a0.value + a1.value);

    /// <summary>
    /// Subtracts a acceleration vector from another.
    /// </summary>
    public static Acceleration3 operator -(Acceleration3 a0, Acceleration3 a1) =>
        new Acceleration3(a0.value - a1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the acceleration vector.
    /// </summary>
    public static Acceleration3 operator -(Acceleration3 a) => new Acceleration3(-a.value);

    /// <summary>
    /// Multiplies the acceleration vector with a scalar.
    /// </summary>
    public static Acceleration3 operator *(Acceleration3 a, float scalar) => new Acceleration3(a.value * scalar);

    /// <summary>
    /// Multiplies the acceleration vector with a scalar.
    /// </summary>
    public static Acceleration3 operator *(float scalar, Acceleration3 a) => new Acceleration3(a.value * scalar);

    /// <summary>
    /// Divides the acceleration vector by a divisor.
    /// </summary>
    public static Acceleration3 operator /(Acceleration3 a, float divisor) => new Acceleration3(a.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a acceleration vector by a speed, returning an untyped vector.
    /// </summary>
    public static Vector3 operator /(Acceleration3 a, Acceleration divisor) => a.value / divisor.NumericValue;

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies a acceleration vector by a timespan, returning a velocity vector.
    /// </summary>
    public static Velocity3 operator *(Acceleration3 a, TimeSpan t) =>
        new Velocity3(a.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies a acceleration vector by a timespan, returning a velocity vector.
    /// </summary>
    public static Velocity3 operator *(TimeSpan t, Acceleration3 a) =>
        new Velocity3(a.value * (float)t.NumericValue);

    #endregion

    #region comparison

    /// <summary>
    /// Compares two acceleration vectors for equality.
    /// </summary>
    public static bool operator ==(Acceleration3 a0, Acceleration3 a1) => a0.Equals(a1);

    /// <summary>
    /// Compares two acceleration vectors for inequality.
    /// </summary>
    public static bool operator !=(Acceleration3 a0, Acceleration3 a1) => !(a0 == a1);

    #endregion

    #endregion

}
