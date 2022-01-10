using System;
using System.Globalization;
using Bearded.Utilities.Geometry;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a 2d directed velocity vector.
/// </summary>
public readonly struct Velocity2 : IEquatable<Velocity2>, IFormattable
{
    private readonly Vector2 value;

    #region construction

    public Velocity2(Vector2 value)
    {
        this.value = value;
    }

    public Velocity2(float x, float y)
        : this(new Vector2(x, y))
    {
    }

    public Velocity2(Speed x, Speed y)
        : this(new Vector2(x.NumericValue, y.NumericValue))
    {
    }

    /// <summary>
    /// Creates a new instance of the Velocity2 type with a given direction and magnitude.
    /// </summary>
    public static Velocity2 In(Direction2 direction, Speed s) => direction * s;

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric vector value of the velocity vector.
    /// </summary>
    public Vector2 NumericValue => value;

    /// <summary>
    /// Returns the X component of the velocity vector.
    /// </summary>
    public Speed X => new Speed(value.X);

    /// <summary>
    /// Returns the Y component of the velocity vector.
    /// </summary>
    public Speed Y => new Speed(value.Y);

    /// <summary>
    /// Returns the direction of the velocity vector.
    /// </summary>
    public Direction2 Direction => Direction2.Of(value);

    /// <summary>
    /// Returns the typed magnitude of the velocity vector.
    /// </summary>
    public Speed Length => new Speed(value.Length);

    /// <summary>
    /// Returns the typed square of the magnitude of the velocity vector.
    /// </summary>
    public Squared<Speed> LengthSquared => new Squared<Speed>(value.LengthSquared);

    /// <summary>
    /// Returns a Velocity2 type with value 0.
    /// </summary>
    public static Velocity2 Zero => new Velocity2(0, 0);

    #endregion

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed velocity vectors.
    /// </summary>
    /// <param name="v0">The velocity vector at t = 0.</param>
    /// <param name="v1">The velocity vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Velocity2 Lerp(Velocity2 v0, Velocity2 v1, float t) => v0 + (v1 - v0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed velocity vector.
    /// </summary>
    /// <param name="v">The velocity vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Velocity2 LerpTo(Velocity2 v, float t) => Lerp(this, v, t);

    #endregion

    #region projection

    /// <summary>
    /// Projects the velocity vector onto an untyped vector, returning the speed component in that vector's direction.
    /// </summary>
    public Speed ProjectedOn(Vector2 vector) => projectedOn(vector.NormalizedSafe());

    /// <summary>
    /// Projects the velocity vector onto a difference vector, returning the speed component in that vector's direction.
    /// </summary>
    public Speed ProjectedOn(Difference2 vector) => projectedOn(vector.NumericValue.NormalizedSafe());

    /// <summary>
    /// Projects the velocity vector onto a direction, returning the speed component in that direction.
    /// </summary>
    public Speed ProjectedOn(Direction2 direction) => projectedOn(direction.Vector);

    private Speed projectedOn(Vector2 normalisedVector) => new Speed(Vector2.Dot(value, normalisedVector));

    #endregion

    #region equality and hashcode

    public bool Equals(Velocity2 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Velocity2 velocity2 && Equals(velocity2);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"({value.X.ToString(format, formatProvider)}, {value.Y.ToString(format, formatProvider)}) u/t";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two velocity vectors.
    /// </summary>
    public static Velocity2 operator +(Velocity2 v0, Velocity2 v1) => new Velocity2(v0.value + v1.value);

    /// <summary>
    /// Subtracts a velocity vector from another.
    /// </summary>
    public static Velocity2 operator -(Velocity2 v0, Velocity2 v1) => new Velocity2(v0.value - v1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the velocity vector.
    /// </summary>
    public static Velocity2 operator -(Velocity2 v) => new Velocity2(-v.value);

    /// <summary>
    /// Multiplies the velocity vector with a scalar.
    /// </summary>
    public static Velocity2 operator *(Velocity2 v, float scalar) => new Velocity2(v.value * scalar);

    /// <summary>
    /// Multiplies the velocity vector with a scalar.
    /// </summary>
    public static Velocity2 operator *(float scalar, Velocity2 v) => new Velocity2(v.value * scalar);

    /// <summary>
    /// Divides the velocity vector by a divisor.
    /// </summary>
    public static Velocity2 operator /(Velocity2 v, float divisor) => new Velocity2(v.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a velocity vector by a speed, returning an untyped vector.
    /// </summary>
    public static Vector2 operator /(Velocity2 v, Speed divisor) => v.value / divisor.NumericValue;

    #endregion

    #region differentiate

    /// <summary>
    /// Divides a velocity vector by a timespan, returning an acceleration vector.
    /// </summary>
    public static Acceleration2 operator /(Velocity2 v, TimeSpan t) => new Acceleration2(v.value / (float)t.NumericValue);

    public static Acceleration2 operator *(Velocity2 v, Frequency f) => new Acceleration2(v.value * (float)f.NumericValue);

    public static Acceleration2 operator *(Frequency f, Velocity2 v) => new Acceleration2(v.value * (float)f.NumericValue);

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies a velocity vector by a timespan, returning a difference vector.
    /// </summary>
    public static Difference2 operator *(Velocity2 v, TimeSpan t) => new Difference2(v.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies a velocity vector by a timespan, returning a difference vector.
    /// </summary>
    public static Difference2 operator *(TimeSpan t, Velocity2 v) => new Difference2(v.value * (float)t.NumericValue);

    public static Difference2 operator /(Velocity2 v, Frequency f) => new Difference2(v.value / (float)f.NumericValue);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two velocity vectors for equality.
    /// </summary>
    public static bool operator ==(Velocity2 v0, Velocity2 v1) => v0.Equals(v1);

    /// <summary>
    /// Compares two velocity vectors for inequality.
    /// </summary>
    public static bool operator !=(Velocity2 v0, Velocity2 v1) => !(v0 == v1);

    #endregion

    #endregion

}
