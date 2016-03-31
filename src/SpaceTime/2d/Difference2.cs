using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of a 2d directed difference vector.
    /// </summary>
    public struct Difference2 : IEquatable<Difference2>
    {
        private readonly Vector2 value;

        #region constructing

        /// <summary>
        /// Creates a new instance of the Difference2 type.
        /// </summary>
        public Difference2(Vector2 value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of the Difference2 type.
        /// </summary>
        public Difference2(float x, float y)
            : this(new Vector2(x, y))
        {
        }

        /// <summary>
        /// Creates a new instance of the Difference2 type.
        /// </summary>
        public Difference2(Unit x, Unit y)
            : this(new Vector2(x.NumericValue, y.NumericValue))
        {
        }

        /// <summary>
        /// Creates a new instance of the Difference2 type with a given direction and magnitude.
        /// </summary>
        public static Difference2 In(Direction2 direction, Unit u)
        {
            return direction * u;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric vector value of the difference vector.
        /// </summary>
        public Vector2 NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the X component of the difference vector.
        /// </summary>
        public Unit X { get { return new Unit(this.value.X); } }

        /// <summary>
        /// Returns the Y component of the difference vector.
        /// </summary>
        public Unit Y { get { return new Unit(this.value.Y); } }

        /// <summary>
        /// Returns the direction of the difference vector.
        /// </summary>
        public Direction2 Direction { get { return Direction2.Of(this.value); } }

        /// <summary>
        /// Returns the typed magnitude of the difference vector.
        /// </summary>
        public Unit Length { get { return new Unit(this.value.Length); } }

        /// <summary>
        /// Returns the typed square of the magnitude of the difference vector.
        /// </summary>
        public Squared<Unit> LengthSquared { get { return new Squared<Unit>(this.value.LengthSquared); } }

        /// <summary>
        /// Returns a Difference2 type with value 0.
        /// </summary>
        public Difference2 Zero { get { return new Difference2(0, 0); } }

        #endregion

        #region methods

        #region lerp

        /// <summary>
        /// Linearly interpolates between two typed difference vectors.
        /// </summary>
        /// <param name="d0">The difference vector at t = 0.</param>
        /// <param name="d1">The difference vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public static Difference2 Lerp(Difference2 d0, Difference2 d1, float t)
        {
            return d0 + (d1 - d0) * t;
        }

        /// <summary>
        /// Linearly interpolates towards another typed difference vector.
        /// </summary>
        /// <param name="d">The difference vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public Difference2 LerpTo(Difference2 d, float t)
        {
            return Lerp(this, d, t);
        }

        #endregion

        #region projection

        /// <summary>
        /// Projects the difference vector onto an untyped vector, returning the speed component in that vector's direction.
        /// </summary>
        public Unit ProjectedOn(Vector2 vector)
        {
            return this.projectedOn(vector.NormalizedSafe());
        }

        /// <summary>
        /// Projects the difference vector onto a difference vector, returning the speed component in that vector's direction.
        /// </summary>
        public Unit ProjectedOn(Difference2 vector)
        {
            return this.projectedOn(vector.NumericValue.NormalizedSafe());
        }

        /// <summary>
        /// Projects the difference vector onto a direction, returning the speed component in that direction.
        /// </summary>
        public Unit ProjectedOn(Direction2 direction)
        {
            return this.projectedOn(direction.Vector);
        }

        private Unit projectedOn(Vector2 normalisedVector)
        {
            return new Unit(Vector2.Dot(this.value, normalisedVector));
        }

        #endregion

        #region equality and hashcode

        public bool Equals(Difference2 other)
        {
            return this.value == other.value;
        }

        public override bool Equals(object obj)
        {
            return obj is Difference2 && this.Equals((Difference2)obj);
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
        /// Adds two difference vectors.
        /// </summary>
        public static Difference2 operator +(Difference2 d0, Difference2 d1)
        {
            return new Difference2(d0.value + d1.value);
        }

        /// <summary>
        /// Subtracts a difference vector from another.
        /// </summary>
        public static Difference2 operator -(Difference2 d0, Difference2 d1)
        {
            return new Difference2(d0.value - d1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the difference vector.
        /// </summary>
        public static Difference2 operator -(Difference2 d)
        {
            return new Difference2(-d.value);
        }

        /// <summary>
        /// Multiplies the difference vector with a scalar.
        /// </summary>
        public static Difference2 operator *(Difference2 d, float scalar)
        {
            return new Difference2(d.value * scalar);
        }

        /// <summary>
        /// Multiplies the difference vector with a scalar.
        /// </summary>
        public static Difference2 operator *(float scalar, Difference2 d)
        {
            return new Difference2(d.value * scalar);
        }

        /// <summary>
        /// Divides the difference vector by a divisor.
        /// </summary>
        public static Difference2 operator /(Difference2 d, float divisor)
        {
            return new Difference2(d.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides a difference vector by a speed, returning an untyped vector.
        /// </summary>
        public static Vector2 operator /(Difference2 d, Unit divisor)
        {
            return d.value / divisor.NumericValue;
        }

        #endregion

        #region differentiate

        /// <summary>
        /// Divides a difference vector by a timespan, returning an velocity vector.
        /// </summary>
        public static Velocity2 operator /(Difference2 d, TimeSpan t)
        {
            return new Velocity2(d.value / (float)t.NumericValue);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two difference vectors for equality.
        /// </summary>
        public static bool operator ==(Difference2 d0, Difference2 d1)
        {
            return d0.Equals(d1);
        }

        /// <summary>
        /// Compares two difference vectors for inequality.
        /// </summary>
        public static bool operator !=(Difference2 d0, Difference2 d1)
        {
            return !(d0 == d1);
        }

        #endregion

        #endregion

    }
}