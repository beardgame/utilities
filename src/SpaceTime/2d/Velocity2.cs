using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of a 2d directed velocity vector.
    /// </summary>
    public struct Velocity2 : IBackedBy<Vector2>, IEquatable<Velocity2>
    {
        private readonly Vector2 value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Velocity2 type.
        /// </summary>
        public Velocity2(Vector2 value)
        {
            this.value = value;
        }
        /// <summary>
        /// Creates a new instance of the Velocity2 type.
        /// </summary>
        public Velocity2(float x, float y)
            : this(new Vector2(x, y))
        {
        }
        /// <summary>
        /// Creates a new instance of the Velocity2 type.
        /// </summary>
        public Velocity2(Speed x, Speed y)
            : this(new Vector2(x.NumericValue, y.NumericValue))
        {
        }

        /// <summary>
        /// Creates a new instance of the Velocity2 type with a given direction and magnitude.
        /// </summary>
        public static Velocity2 In(Direction2 direction, Speed s)
        {
            return direction * s;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric vector value of the velocity vector.
        /// </summary>
        public Vector2 NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the X component of the velocity vector.
        /// </summary>
        public Speed X { get { return new Speed(this.value.X); } }
        /// <summary>
        /// Returns the Y component of the velocity vector.
        /// </summary>
        public Speed Y { get { return new Speed(this.value.Y); } }

        /// <summary>
        /// Returns the direction of the velocity vector.
        /// </summary>
        public Direction2 Direction { get { return Direction2.Of(this.value); } }

        /// <summary>
        /// Returns the typed magnitude of the velocity vector.
        /// </summary>
        public Speed Length { get { return new Speed(this.value.Length); } }

        /// <summary>
        /// Returns the typed square of the magnitude of the velocity vector.
        /// </summary>
        public Squared<Speed> LengthSquared { get { return new Squared<Speed>(this.value.LengthSquared); } }

        /// <summary>
        /// Returns a Velocity2 type with value 0.
        /// </summary>
        public Velocity2 Zero { get { return new Velocity2(0, 0); } }

        #endregion

        #region methods

        #region lerp

        /// <summary>
        /// Linearly interpolates between two typed velocity vectors.
        /// </summary>
        /// <param name="v0">The velocity vector at t = 0.</param>
        /// <param name="v1">The velocity vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public static Velocity2 Lerp(Velocity2 v0, Velocity2 v1, float t)
        {
            return v0 + (v1 - v0) * t;
        }

        /// <summary>
        /// Linearly interpolates towards another typed velocity vector.
        /// </summary>
        /// <param name="v">The velocity vector at t = 1.</param>
        /// <param name="t">The interpolation scalar.</param>
        public Velocity2 LerpTo(Velocity2 v, float t)
        {
            return Lerp(this, v, t);
        }

        #endregion

        #region projection

        /// <summary>
        /// Projects the velocity vector onto an untyped vector, returning the speed component in that vector's direction.
        /// </summary>
        public Speed ProjectedOn(Vector2 vector)
        {
            return this.projectedOn(vector.NormalizedSafe());
        }
        /// <summary>
        /// Projects the velocity vector onto a difference vector, returning the speed component in that vector's direction.
        /// </summary>
        public Speed ProjectedOn(Difference2 vector)
        {
            return this.projectedOn(vector.NumericValue.NormalizedSafe());
        }
        /// <summary>
        /// Projects the velocity vector onto a direction, returning the speed component in that direction.
        /// </summary>
        public Speed ProjectedOn(Direction2 direction)
        {
            return this.projectedOn(direction.Vector);
        }

        private Speed projectedOn(Vector2 normalisedVector)
        {
            return new Speed(Vector2.Dot(this.value, normalisedVector));
        }

        #endregion

        #region equality and hashcode

        public bool Equals(Velocity2 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Velocity2 && this.Equals((Velocity2)obj);
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
        /// Adds two velocity vectors.
        /// </summary>
        public static Velocity2 operator +(Velocity2 v0, Velocity2 v1)
        {
            return new Velocity2(v0.value + v1.value);
        }
        /// <summary>
        /// Subtracts a velocity vector from another.
        /// </summary>
        public static Velocity2 operator -(Velocity2 v0, Velocity2 v1)
        {
            return new Velocity2(v0.value - v1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the velocity vector.
        /// </summary>
        public static Velocity2 operator -(Velocity2 v)
        {
            return new Velocity2(-v.value);
        }
        /// <summary>
        /// Multiplies the velocity vector with a scalar.
        /// </summary>
        public static Velocity2 operator *(Velocity2 v, float scalar)
        {
            return new Velocity2(v.value * scalar);
        }
        /// <summary>
        /// Multiplies the velocity vector with a scalar.
        /// </summary>
        public static Velocity2 operator *(float scalar, Velocity2 v)
        {
            return new Velocity2(v.value * scalar);
        }
        /// <summary>
        /// Divides the velocity vector by a divisor.
        /// </summary>
        public static Velocity2 operator /(Velocity2 v, float divisor)
        {
            return new Velocity2(v.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides a velocity vector by a speed, returning an untyped vector.
        /// </summary>
        public static Vector2 operator /(Velocity2 v, Speed divisor)
        {
            return v.value / divisor.NumericValue;
        }

        #endregion

        #region differentiate

        /// <summary>
        /// Divides a velocity vector by a timespan, returning an acceleration vector.
        /// </summary>
        public static Acceleration2 operator /(Velocity2 v, TimeSpan t)
        {
            return new Acceleration2(v.value / (float)t.NumericValue);
        }

        #endregion

        #region integrate

        /// <summary>
        /// Multiplies a velocity vector by a timespan, returning a difference vector.
        /// </summary>
        public static Difference2 operator *(Velocity2 v, TimeSpan t)
        {
            return new Difference2(v.value * (float)t.NumericValue);
        }
        /// <summary>
        /// Multiplies a velocity vector by a timespan, returning a difference vector.
        /// </summary>
        public static Difference2 operator *(TimeSpan t, Velocity2 v)
        {
            return new Difference2(v.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two velocity vectors for equality.
        /// </summary>
        public static bool operator ==(Velocity2 v0, Velocity2 v1)
        {
            return v0.value == v1.value;
        }
        /// <summary>
        /// Compares two velocity vectors for inequality.
        /// </summary>
        public static bool operator !=(Velocity2 v0, Velocity2 v1)
        {
            return v0.value != v1.value;
        }

        #endregion

        #endregion

    }
}