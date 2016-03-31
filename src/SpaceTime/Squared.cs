using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// Represents a type-safe squared value, backed by a float.
    /// </summary>
    /// <typeparam name="T">The squared type.</typeparam>
    public struct Squared<T> : IEquatable<Squared<T>>, IComparable<Squared<T>>
        where T : struct
    {
        private readonly float value;

        #region construcing

        internal Squared(float value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creteas a new instance of the Squared type, from a given root value.
        /// </summary>
        public static Squared<T> FromRoot(float root)
        {
            return new Squared<T>(root.Squared());
        }

        /// <summary>
        /// Creteas a new instance of the Squared type, from a given value.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">If value is negative.</exception>
        public static Squared<T> FromValue(float value)
        {
            if(value < 0)
                throw new ArgumentOutOfRangeException("value", "Must be non-negative.");

            return new Squared<T>(value);
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the square.
        /// </summary>
        public float NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns a Square type of value 0.
        /// </summary>
        public static Squared<T> Zero { get { return new Squared<T>(0); } }

        /// <summary>
        /// Returns a Square type of value 1.
        /// </summary>
        public static Squared<T> One { get { return new Squared<T>(1); } }

        #endregion

        #region methods

        #region equality and hashcode

        public bool Equals(Squared<T> other)
        {
            return this.value == other.value;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Squared<T> && this.Equals((Squared<T>)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(Squared<T> other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two squares.
        /// </summary>
        public static Squared<T> operator +(Squared<T> s0, Squared<T> s1)
        {
            return new Squared<T>(s0.value + s1.value);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides a square by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(Squared<T> dividend, Squared<T> divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two squares for equality.
        /// </summary>
        public static bool operator ==(Squared<T> s0, Squared<T> s1)
        {
            return s0.Equals(s1);
        }

        /// <summary>
        /// Compares two squares for inequality.
        /// </summary>
        public static bool operator !=(Squared<T> s0, Squared<T> s1)
        {
            return !(s0 == s1);
        }

        /// <summary>
        /// Checks if one square is smaller than another.
        /// </summary>
        public static bool operator <(Squared<T> s0, Squared<T> s1)
        {
            return s0.value < s1.value;
        }

        /// <summary>
        /// Checks if one square is larger than another.
        /// </summary>
        public static bool operator >(Squared<T> s0, Squared<T> s1)
        {
            return s0.value > s1.value;
        }

        /// <summary>
        /// Checks if one square is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Squared<T> s0, Squared<T> s1)
        {
            return s0.value <= s1.value;
        }

        /// <summary>
        /// Checks if one square is larger or equal to another.
        /// </summary>
        public static bool operator >=(Squared<T> s0, Squared<T> s1)
        {
            return s0.value >= s1.value;
        }

        #endregion

        #endregion
    }
}