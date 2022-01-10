using System;
using System.Globalization;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a 3d directed velocity vector.
/// </summary>
public readonly struct Velocity3 : IEquatable<Velocity3>, IFormattable
{
    private readonly Vector3 value;

    #region construction

    public Velocity3(Vector3 value)
    {
        this.value = value;
    }

    public Velocity3(float x, float y, float z)
        : this(new Vector3(x, y, z))
    {
    }

    public Velocity3(Speed x, Speed y, Speed z)
        : this(new Vector3(x.NumericValue, y.NumericValue, z.NumericValue))
    {
    }

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric vector value of the velocity vector.
    /// </summary>
    public Vector3 NumericValue => value;

    /// <summary>
    /// Returns the X component of the velocity vector.
    /// </summary>
    public Speed X => new Speed(value.X);

    /// <summary>
    /// Returns the Y component of the velocity vector.
    /// </summary>
    public Speed Y => new Speed(value.Y);

    /// <summary>
    /// Returns the Z component of the velocity vector.
    /// </summary>
    public Speed Z => new Speed(value.Z);

    /// <summary>
    /// Returns the typed magnitude of the velocity vector.
    /// </summary>
    public Speed Length => new Speed(value.Length);

    /// <summary>
    /// Returns the typed square of the magnitude of the velocity vector.
    /// </summary>
    public Squared<Speed> LengthSquared => new Squared<Speed>(value.LengthSquared);

    /// <summary>
    /// Returns a Velocity3 type with value 0.
    /// </summary>
    public static Velocity3 Zero => new Velocity3(0, 0, 0);

    #endregion

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed velocity vectors.
    /// </summary>
    /// <param name="v0">The velocity vector at t = 0.</param>
    /// <param name="v1">The velocity vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Velocity3 Lerp(Velocity3 v0, Velocity3 v1, float t) => v0 + (v1 - v0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed velocity vector.
    /// </summary>
    /// <param name="v">The velocity vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Velocity3 LerpTo(Velocity3 v, float t) => Lerp(this, v, t);

    #endregion

    #region projection

    /// <summary>
    /// Projects the velocity vector onto an untyped vector, returning the speed component in that vector's direction.
    /// </summary>
    public Speed ProjectedOn(Vector3 vector) => projectedOn(vector.NormalizedSafe());

    /// <summary>
    /// Projects the velocity vector onto a difference vector, returning the speed component in that vector's direction.
    /// </summary>
    public Speed ProjectedOn(Difference3 vector) => projectedOn(vector.NumericValue.NormalizedSafe());

    private Speed projectedOn(Vector3 normalisedVector) => new Speed(Vector3.Dot(value, normalisedVector));

    #endregion

    #region equality and hashcode

    public bool Equals(Velocity3 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Velocity3 v && Equals(v);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider) => "(" +
        $"{value.X.ToString(format, formatProvider)}, " +
        $"{value.Y.ToString(format, formatProvider)}, " +
        $"{value.Z.ToString(format, formatProvider)}) u/t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two velocity vectors.
    /// </summary>
    public static Velocity3 operator +(Velocity3 v0, Velocity3 v1) => new Velocity3(v0.value + v1.value);

    /// <summary>
    /// Subtracts a velocity vector from another.
    /// </summary>
    public static Velocity3 operator -(Velocity3 v0, Velocity3 v1) => new Velocity3(v0.value - v1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the velocity vector.
    /// </summary>
    public static Velocity3 operator -(Velocity3 v) => new Velocity3(-v.value);

    /// <summary>
    /// Multiplies the velocity vector with a scalar.
    /// </summary>
    public static Velocity3 operator *(Velocity3 v, float scalar) => new Velocity3(v.value * scalar);

    /// <summary>
    /// Multiplies the velocity vector with a scalar.
    /// </summary>
    public static Velocity3 operator *(float scalar, Velocity3 v) => new Velocity3(v.value * scalar);

    /// <summary>
    /// Divides the velocity vector by a divisor.
    /// </summary>
    public static Velocity3 operator /(Velocity3 v, float divisor) => new Velocity3(v.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a velocity vector by a speed, returning an untyped vector.
    /// </summary>
    public static Vector3 operator /(Velocity3 v, Speed divisor) => v.value / divisor.NumericValue;

    #endregion

    #region differentiate

    /// <summary>
    /// Divides a velocity vector by a timespan, returning an acceleration vector.
    /// </summary>
    public static Acceleration3 operator /(Velocity3 v, TimeSpan t) =>
        new Acceleration3(v.value / (float)t.NumericValue);

    public static Acceleration3 operator *(Velocity3 v, Frequency f) =>
        new Acceleration3(v.value * (float)f.NumericValue);

    public static Acceleration3 operator *(Frequency f, Velocity3 v) =>
        new Acceleration3(v.value * (float)f.NumericValue);

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies a velocity vector by a timespan, returning a difference vector.
    /// </summary>
    public static Difference3 operator *(Velocity3 v, TimeSpan t) =>
        new Difference3(v.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies a velocity vector by a timespan, returning a difference vector.
    /// </summary>
    public static Difference3 operator *(TimeSpan t, Velocity3 v) =>
        new Difference3(v.value * (float)t.NumericValue);

    public static Difference3 operator /(Velocity3 v, Frequency f) =>
        new Difference3(v.value / (float)f.NumericValue);

    #endregion

    #region comparison

    /// <summary>
    /// Compares two velocity vectors for equality.
    /// </summary>
    public static bool operator ==(Velocity3 v0, Velocity3 v1) => v0.Equals(v1);

    /// <summary>
    /// Compares two velocity vectors for inequality.
    /// </summary>
    public static bool operator !=(Velocity3 v0, Velocity3 v1) => !(v0 == v1);

    #endregion

    #endregion

}
