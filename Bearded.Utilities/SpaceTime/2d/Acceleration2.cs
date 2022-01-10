using System;
using System.Globalization;
using Bearded.Utilities.Geometry;
using OpenTK.Mathematics;

namespace Bearded.Utilities.SpaceTime;

/// <summary>
/// A type-safe representation of a 2d directed acceleration vector.
/// </summary>
public readonly struct Acceleration2 : IEquatable<Acceleration2>, IFormattable
{
    private readonly Vector2 value;

    #region construction

    public Acceleration2(Vector2 value)
    {
        this.value = value;
    }

    public Acceleration2(float x, float y)
        : this(new Vector2(x, y))
    {
    }

    public Acceleration2(Acceleration x, Acceleration y)
        : this(new Vector2(x.NumericValue, y.NumericValue))
    {
    }

    /// <summary>
    /// Creates a new instance of the Acceleration2 type with a given direction and magnitude.
    /// </summary>
    public static Acceleration2 In(Direction2 direction, Acceleration u) => direction * u;

    #endregion

    #region properties

    /// <summary>
    /// Returns the numeric vector value of the acceleration vector.
    /// </summary>
    public Vector2 NumericValue => value;

    /// <summary>
    /// Returns the X component of the acceleration vector.
    /// </summary>
    public Acceleration X => new Acceleration(value.X);

    /// <summary>
    /// Returns the Y component of the acceleration vector.
    /// </summary>
    public Acceleration Y => new Acceleration(value.Y);

    /// <summary>
    /// Returns the direction of the acceleration vector.
    /// </summary>
    public Direction2 Direction => Direction2.Of(value);

    /// <summary>
    /// Returns the typed magnitude of the acceleration vector.
    /// </summary>
    public Acceleration Length => new Acceleration(value.Length);

    /// <summary>
    /// Returns the typed square of the magnitude of the acceleration vector.
    /// </summary>
    public Squared<Acceleration> LengthSquared => new Squared<Acceleration>(value.LengthSquared);

    /// <summary>
    /// Returns a Acceleration2 type with value 0.
    /// </summary>
    public static Acceleration2 Zero => new Acceleration2(0, 0);

    #endregion

    #region methods

    #region lerp

    /// <summary>
    /// Linearly interpolates between two typed acceleration vectors.
    /// </summary>
    /// <param name="a0">The acceleration vector at t = 0.</param>
    /// <param name="a1">The acceleration vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public static Acceleration2 Lerp(Acceleration2 a0, Acceleration2 a1, float t) => a0 + (a1 - a0) * t;

    /// <summary>
    /// Linearly interpolates towards another typed acceleration vector.
    /// </summary>
    /// <param name="a">The acceleration vector at t = 1.</param>
    /// <param name="t">The interpolation scalar.</param>
    public Acceleration2 LerpTo(Acceleration2 a, float t) => Lerp(this, a, t);

    #endregion

    #region projection

    /// <summary>
    /// Projects the acceleration vector onto an untyped vector, returning the acceleration component in that vector's direction.
    /// </summary>
    public Acceleration ProjectedOn(Vector2 vector) => projectedOn(vector.NormalizedSafe());

    /// <summary>
    /// Projects the acceleration vector onto a difference vector, returning the acceleration component in that vector's direction.
    /// </summary>
    public Acceleration ProjectedOn(Difference2 vector) => projectedOn(vector.NumericValue.NormalizedSafe());

    /// <summary>
    /// Projects the acceleration vector onto a direction, returning the acceleration component in that direction.
    /// </summary>
    public Acceleration ProjectedOn(Direction2 direction) => projectedOn(direction.Vector);

    private Acceleration projectedOn(Vector2 normalisedVector) => new Acceleration(Vector2.Dot(value, normalisedVector));

    #endregion

    #region equality and hashcode

    public bool Equals(Acceleration2 other) => value == other.value;

    public override bool Equals(object? obj) => obj is Acceleration2 acceleration2 && Equals(acceleration2);

    public override int GetHashCode() => value.GetHashCode();

    #endregion

    #region tostring

    public override string ToString() => ToString(null, CultureInfo.CurrentCulture);

    public string ToString(string? format, IFormatProvider? formatProvider)
        => $"({value.X.ToString(format, formatProvider)}, {value.Y.ToString(format, formatProvider)}) u/t²";

    public string ToString(string? format)
        => ToString(format, CultureInfo.CurrentCulture);

    #endregion

    #endregion

    #region operators

    #region algebra

    /// <summary>
    /// Adds two acceleration vectors.
    /// </summary>
    public static Acceleration2 operator +(Acceleration2 a0, Acceleration2 a1) => new Acceleration2(a0.value + a1.value);

    /// <summary>
    /// Subtracts a acceleration vector from another.
    /// </summary>
    public static Acceleration2 operator -(Acceleration2 a0, Acceleration2 a1) => new Acceleration2(a0.value - a1.value);

    #endregion

    #region scaling

    /// <summary>
    /// Inverts the acceleration vector.
    /// </summary>
    public static Acceleration2 operator -(Acceleration2 a) => new Acceleration2(-a.value);

    /// <summary>
    /// Multiplies the acceleration vector with a scalar.
    /// </summary>
    public static Acceleration2 operator *(Acceleration2 a, float scalar) => new Acceleration2(a.value * scalar);

    /// <summary>
    /// Multiplies the acceleration vector with a scalar.
    /// </summary>
    public static Acceleration2 operator *(float scalar, Acceleration2 a) => new Acceleration2(a.value * scalar);

    /// <summary>
    /// Divides the acceleration vector by a divisor.
    /// </summary>
    public static Acceleration2 operator /(Acceleration2 a, float divisor) => new Acceleration2(a.value / divisor);

    #endregion

    #region ratio

    /// <summary>
    /// Divides a acceleration vector by a speed, returning an untyped vector.
    /// </summary>
    public static Vector2 operator /(Acceleration2 a, Acceleration divisor) => a.value / divisor.NumericValue;

    #endregion

    #region integrate

    /// <summary>
    /// Multiplies a acceleration vector by a timespan, returning a velocity vector.
    /// </summary>
    public static Velocity2 operator *(Acceleration2 a, TimeSpan t) => new Velocity2(a.value * (float)t.NumericValue);

    /// <summary>
    /// Multiplies a acceleration vector by a timespan, returning a velocity vector.
    /// </summary>
    public static Velocity2 operator *(TimeSpan t, Acceleration2 a) => new Velocity2(a.value * (float)t.NumericValue);

    #endregion

    #region comparision

    /// <summary>
    /// Compares two acceleration vectors for equality.
    /// </summary>
    public static bool operator ==(Acceleration2 a0, Acceleration2 a1) => a0.Equals(a1);

    /// <summary>
    /// Compares two acceleration vectors for inequality.
    /// </summary>
    public static bool operator !=(Acceleration2 a0, Acceleration2 a1) => !(a0 == a1);

    #endregion

    #endregion

}
