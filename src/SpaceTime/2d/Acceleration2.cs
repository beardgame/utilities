using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of a 2d directed acceleration vector.
    /// </summary>
    public struct Acceleration2 : IEquatable<Acceleration2>
    {
        private readonly Vector2 value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Acceleration2 type.
        /// </summary>
        public Acceleration2(Vector2 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of the Acceleration2 type.
        /// </summary>
        public Acceleration2(float x, float y)
            : this(new Vector2(x, y))
        {
        }

        /// <summary>
        /// Creates a new instance of the Acceleration2 type.
        /// </summary>
        public Acceleration2(Acceleration x, Acceleration y)
            : this(new Vector2(x.NumericValue, y.NumericValue))
        {
        }

        /// <summary>
        /// Creates a new instance of the Acceleration2 type with a given direction and magnitude.
        /// </summary>
        public static Acceleration2 In(Direction2 direction, Acceleration u)
        {
            return direction * u;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric vector value of the acceleration vector.
        /// </summary>
        public Vector2 NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the X component of the acceleration vector.
        /// </summary>
        public Acceleration X { get { return new Acceleration(this.value.X); } }

        /// <summary>
        /// Returns the Y component of the acceleration vector.
        /// </summary>
        public Acceleration Y { get { return new Acceleration(this.value.Y); } }

        /// <summary>
        /// Returns the direction of the acceleration vector.
        /// </summary>
        public Direction2 Direction { get { return Direction2.Of(this.value); } }

        /// <summary>
        /// Returns the typed magnitude of the acceleration vector.
        /// </summary>
        public Acceleration Length { get { return new Acceleration(this.value.Length); } }

        /// <summary>
        /// Returns the typed square of the magnitude of the acceleration vector.
        /// </summary>
        public Squared<Acceleration> LengthSquared { get { return new Squared<Acceleration>(this.value.LengthSquared); } }

        /// <summary>
        /// Returns a Acceleration2 type with value 0.
        /// </summary>
        public static Acceleration2 Zero { get { return new Acceleration2(0, 0); } }

        #endregion

        #region methods

        #region lerp

        /// <summary>
        /// Linearly interpolates between two typed acceleration vectors.
        /// </summary>
        /// <param name="a0">The acceleration vector at t = 0.</param>
        /// <param name="a1">The acceleration vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public static Acceleration2 Lerp(Acceleration2 a0, Acceleration2 a1, float t)
        {
            return a0 + (a1 - a0) * t;
        }

        /// <summary>
        /// Linearly interpolates towards another typed acceleration vector.
        /// </summary>
        /// <param name="a">The acceleration vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public Acceleration2 LerpTo(Acceleration2 a, float t)
        {
            return Lerp(this, a, t);
        }

        #endregion

        #region projection

        /// <summary>
        /// Projects the acceleration vector onto an untyped vector, returning the acceleration component in that vector's direction.
        /// </summary>
        public Acceleration ProjectedOn(Vector2 vector)
        {
            return this.projectedOn(vector.NormalizedSafe());
        }

        /// <summary>
        /// Projects the acceleration vector onto a difference vector, returning the acceleration component in that vector's direction.
        /// </summary>
        public Acceleration ProjectedOn(Difference2 vector)
        {
            return this.projectedOn(vector.NumericValue.NormalizedSafe());
        }

        /// <summary>
        /// Projects the acceleration vector onto a direction, returning the acceleration component in that direction.
        /// </summary>
        public Acceleration ProjectedOn(Direction2 direction)
        {
            return this.projectedOn(direction.Vector);
        }

        private Acceleration projectedOn(Vector2 normalisedVector)
        {
            return new Acceleration(Vector2.Dot(this.value, normalisedVector));
        }

        #endregion

        #region equality and hashcode

        public bool Equals(Acceleration2 other)
        {
            return this.value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Acceleration2 && this.Equals((Acceleration2)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two acceleration vectors.
        /// </summary>
        public static Acceleration2 operator +(Acceleration2 a0, Acceleration2 a1)
        {
            return new Acceleration2(a0.value + a1.value);
        }

        /// <summary>
        /// Subtracts a acceleration vector from another.
        /// </summary>
        public static Acceleration2 operator -(Acceleration2 a0, Acceleration2 a1)
        {
            return new Acceleration2(a0.value - a1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the acceleration vector.
        /// </summary>
        public static Acceleration2 operator -(Acceleration2 a)
        {
            return new Acceleration2(-a.value);
        }

        /// <summary>
        /// Multiplies the acceleration vector with a scalar.
        /// </summary>
        public static Acceleration2 operator *(Acceleration2 a, float scalar)
        {
            return new Acceleration2(a.value * scalar);
        }

        /// <summary>
        /// Multiplies the acceleration vector with a scalar.
        /// </summary>
        public static Acceleration2 operator *(float scalar, Acceleration2 a)
        {
            return new Acceleration2(a.value * scalar);
        }

        /// <summary>
        /// Divides the acceleration vector by a divisor.
        /// </summary>
        public static Acceleration2 operator /(Acceleration2 a, float divisor)
        {
            return new Acceleration2(a.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides a acceleration vector by a speed, returning an untyped vector.
        /// </summary>
        public static Vector2 operator /(Acceleration2 a, Acceleration divisor)
        {
            return a.value / divisor.NumericValue;
        }

        #endregion

        #region integrate

        /// <summary>
        /// Multiplies a acceleration vector by a timespan, returning a velocity vector.
        /// </summary>
        public static Velocity2 operator *(Acceleration2 a, TimeSpan t)
        {
            return new Velocity2(a.value * (float)t.NumericValue);
        }

        /// <summary>
        /// Multiplies a acceleration vector by a timespan, returning a velocity vector.
        /// </summary>
        public static Velocity2 operator *(TimeSpan t, Acceleration2 a)
        {
            return new Velocity2(a.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two acceleration vectors for equality.
        /// </summary>
        public static bool operator ==(Acceleration2 a0, Acceleration2 a1)
        {
            return a0.Equals(a1);
        }

        /// <summary>
        /// Compares two acceleration vectors for inequality.
        /// </summary>
        public static bool operator !=(Acceleration2 a0, Acceleration2 a1)
        {
            return !(a0 == a1);
        }

        #endregion

        #endregion

    }
}