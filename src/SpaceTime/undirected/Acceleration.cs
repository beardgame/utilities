using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of an undirected signed acceleration.
    /// </summary>
    public struct Acceleration : IBackedBy<float>, IEquatable<Acceleration>, IComparable<Acceleration>
    {
        private readonly float value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Acceleration type.
        /// </summary>
        public Acceleration(float value)
        {
            this.value = value;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the acceleration value.
        /// </summary>
        public float NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the type-safe square of the acceleration value.
        /// </summary>
        public Squared<Acceleration> Squared { get { return Squared<Acceleration>.FromRoot(this.value); } }

        /// <summary>
        /// Returns an Acceleration type with value 0.
        /// </summary>
        public static Acceleration Zero { get { return new Acceleration(0); } }
        /// <summary>
        /// Returns an Acceleration type with value 1.
        /// </summary>
        public static Acceleration One { get { return new Acceleration(1); } }

        #endregion

        #region methods

        #region equality and hashcode

        public bool Equals(Acceleration other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Acceleration && this.Equals((Acceleration)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(Acceleration other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two acceleration values.
        /// </summary>
        public static Acceleration operator +(Acceleration a0, Acceleration a1)
        {
            return new Acceleration(a0.value + a1.value);
        }
        /// <summary>
        /// Subtracts an acceleration value from another.
        /// </summary>
        public static Acceleration operator -(Acceleration a0, Acceleration a1)
        {
            return new Acceleration(a0.value - a1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the acceleration value.
        /// </summary>
        public static Acceleration operator -(Acceleration a)
        {
            return new Acceleration(-a.value);
        }
        /// <summary>
        /// Multiples the acceleration value with a scalar.
        /// </summary>
        public static Acceleration operator *(Acceleration a, float scalar)
        {
            return new Acceleration(a.value * scalar);
        }
        /// <summary>
        /// Multiples the acceleration value with a scalar.
        /// </summary>
        public static Acceleration operator *(float scalar, Acceleration a)
        {
            return new Acceleration(a.value * scalar);
        }
        /// <summary>
        /// Divides the acceleration value by a divisor.
        /// </summary>
        public static Acceleration operator /(Acceleration a, float divisor)
        {
            return new Acceleration(a.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Devides an acceleration value by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(Acceleration dividend, Acceleration divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region integrate

        /// <summary>
        /// Multiplies an acceleration value by a timespan, returning a speed.
        /// </summary>
        public static Speed operator *(Acceleration a, TimeSpan t)
        {
            return new Speed(a.value * (float)t.NumericValue);
        }
        /// <summary>
        /// Multiplies an acceleration value by a timespan, returning a speed.
        /// </summary>
        public static Speed operator *(TimeSpan t, Acceleration a)
        {
            return new Speed(a.value * (float)t.NumericValue);
        }

        #endregion

        #region add dimension

        /// <summary>
        /// Multiplies a direction with an acceleration value, returning a typed acceleration vector of the given direction and length.
        /// </summary>
        public static Acceleration2 operator *(Acceleration a, Direction2 d)
        {
            return new Acceleration2(d.Vector * a.value);
        }
        /// <summary>
        /// Multiplies a direction with an acceleration value, returning a typed acceleration vector of the given direction and length.
        /// </summary>
        public static Acceleration2 operator *(Direction2 d, Acceleration a)
        {
            return new Acceleration2(d.Vector * a.value);
        }

        /// <summary>
        /// Multiplies an acceleration value with an untyped vector, returning a typed acceleration vector.
        /// </summary>
        public static Acceleration2 operator *(Acceleration u, Vector2 v)
        {
            return new Acceleration2(v * u.value);
        }
        /// <summary>
        /// Multiplies an acceleration value with an untyped vector, returning a typed acceleration vector.
        /// </summary>
        public static Acceleration2 operator *(Vector2 v, Acceleration u)
        {
            return new Acceleration2(v * u.value);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two acceleration values for equality.
        /// </summary>
        public static bool operator ==(Acceleration a0, Acceleration a1)
        {
            return a0.value == a1.value;
        }
        /// <summary>
        /// Compares two acceleration values for inequality.
        /// </summary>
        public static bool operator !=(Acceleration a0, Acceleration a1)
        {
            return a0.value != a1.value;
        }
        /// <summary>
        /// Checks if one acceleration value is smaller than another.
        /// </summary>
        public static bool operator <(Acceleration a0, Acceleration a1)
        {
            return a0.value < a1.value;
        }
        /// <summary>
        /// Checks if one acceleration value is larger than another.
        /// </summary>
        public static bool operator >(Acceleration a0, Acceleration a1)
        {
            return a0.value > a1.value;
        }
        /// <summary>
        /// Checks if one acceleration value is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Acceleration a0, Acceleration a1)
        {
            return a0.value <= a1.value;
        }
        /// <summary>
        /// Checks if one acceleration value is larger or equal to another.
        /// </summary>
        public static bool operator >=(Acceleration a0, Acceleration a1)
        {
            return a0.value >= a1.value;
        }

        #endregion

        #endregion

    }
}