using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of an undirected signed speed.
    /// </summary>
    public struct Speed : IBackedBy<float>, IEquatable<Speed>, IComparable<Speed>
    {
        private readonly float value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Speed type.
        /// </summary>
        public Speed(float value)
        {
            this.value = value;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the speed value.
        /// </summary>
        public float NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the type-safe square of the speed value.
        /// </summary>
        public Squared<Speed> Squared { get { return Squared<Speed>.FromRoot(this.value); } }

        /// <summary>
        /// Returns a Speed type with value 0.
        /// </summary>
        public static Speed Zero { get { return new Speed(0); } }
        /// <summary>
        /// Returns a Speed type with value 1.
        /// </summary>
        public static Speed One { get { return new Speed(1); } }

        #endregion

        #region methods

        #region equality and hashcode

        public bool Equals(Speed other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Speed && this.Equals((Speed)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(Speed other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two speed values.
        /// </summary>
        public static Speed operator +(Speed s0, Speed s1)
        {
            return new Speed(s0.value + s1.value);
        }
        /// <summary>
        /// Subtracts a speed value from another.
        /// </summary>
        public static Speed operator -(Speed s0, Speed s1)
        {
            return new Speed(s0.value - s1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the speed value.
        /// </summary>
        public static Speed operator -(Speed s)
        {
            return new Speed(-s.value);
        }
        /// <summary>
        /// Multiplies the speed value with a scalar.
        /// </summary>
        public static Speed operator *(Speed s, float scalar)
        {
            return new Speed(s.value * scalar);
        }
        /// <summary>
        /// Multiplies the speed value with a scalar.
        /// </summary>
        public static Speed operator *(float scalar, Speed s)
        {
            return new Speed(s.value * scalar);
        }
        /// <summary>
        /// Divides the speed value by a divisor.
        /// </summary>
        public static Speed operator /(Speed s, float divisor)
        {
            return new Speed(s.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides a speed value by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(Speed dividend, Speed divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region differentiate

        /// <summary>
        /// Divides a speed value by a timespan, returning an acceleration.
        /// </summary>
        public static Acceleration operator /(Speed s, TimeSpan t)
        {
            return new Acceleration(s.value / (float)t.NumericValue);
        }

        #endregion

        #region integrate

        /// <summary>
        /// Multiplies a speed value by a timespan, returning a unit value.
        /// </summary>
        public static Unit operator *(Speed s, TimeSpan t)
        {
            return new Unit(s.value * (float)t.NumericValue);
        }
        /// <summary>
        /// Multiplies a speed value by a timespan, returning a unit value.
        /// </summary>
        public static Unit operator *(TimeSpan t, Speed s)
        {
            return new Unit(s.value * (float)t.NumericValue);
        }

        #endregion

        #region add dimension

        /// <summary>
        /// Multiplies a direction with a speed value, returning a typed speed vector of the given direction and length.
        /// </summary>
        public static Velocity2 operator *(Speed s, Direction2 d)
        {
            return new Velocity2(d.Vector * s.value);
        }
        /// <summary>
        /// Multiplies a direction with a speed value, returning a typed speed vector of the given direction and length.
        /// </summary>
        public static Velocity2 operator *(Direction2 d, Speed s)
        {
            return new Velocity2(d.Vector * s.value);
        }

        /// <summary>
        /// Multiplies a speed value with an untyped vector, returning a typed speed vector.
        /// </summary>
        public static Velocity2 operator *(Speed u, Vector2 v)
        {
            return new Velocity2(v * u.value);
        }
        /// <summary>
        /// Multiplies a speed value with an untyped vector, returning a typed speed vector.
        /// </summary>
        public static Velocity2 operator *(Vector2 v, Speed u)
        {
            return new Velocity2(v * u.value);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two speed values for equality.
        /// </summary>
        public static bool operator ==(Speed s0, Speed s1)
        {
            return s0.value == s1.value;
        }
        /// <summary>
        /// Compares two speed values for inequality.
        /// </summary>
        public static bool operator !=(Speed s0, Speed s1)
        {
            return s0.value != s1.value;
        }
        /// <summary>
        /// Checks if one speed value is smaller than another.
        /// </summary>
        public static bool operator <(Speed s0, Speed s1)
        {
            return s0.value < s1.value;
        }
        /// <summary>
        /// Checks if one speed value is larger than another.
        /// </summary>
        public static bool operator >(Speed s0, Speed s1)
        {
            return s0.value > s1.value;
        }
        /// <summary>
        /// Checks if one speed value is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Speed s0, Speed s1)
        {
            return s0.value <= s1.value;
        }
        /// <summary>
        /// Checks if one speed value is larger or equal to another.
        /// </summary>
        public static bool operator >=(Speed s0, Speed s1)
        {
            return s0.value >= s1.value;
        }

        #endregion

        #endregion

    }
}