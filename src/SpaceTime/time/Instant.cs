using System;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of an absolute instant in time.
    /// </summary>
    public struct Instant : IEquatable<Instant>, IComparable<Instant>
    {
        private readonly double value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Instant type.
        /// </summary>
        public Instant(double value)
        {
            this.value = value;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the time instant.
        /// </summary>
        public double NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns an Instant type with value 0.
        /// </summary>
        public static Instant Zero { get { return new Instant(0); } }

        #endregion

        #region methods

        #region equality and hashcode

        public bool Equals(Instant other)
        {
            return this.value == other.value;
        }

        public override bool Equals(object obj)
        {
            return obj is Instant && this.Equals((Instant)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(Instant other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region TimeSpan interaction

        /// <summary>
        /// Adds a timespan to a time instant.
        /// </summary>
        public static Instant operator +(Instant i, TimeSpan t)
        {
            return new Instant(i.value + t.NumericValue);
        }

        /// <summary>
        /// Adds a timespan to a time instant.
        /// </summary>
        public static Instant operator +(TimeSpan t, Instant i)
        {
            return new Instant(i.value + t.NumericValue);
        }

        /// <summary>
        /// Subtracts a timespan from a time instant.
        /// </summary>
        public static Instant operator -(Instant i, TimeSpan t)
        {
            return new Instant(i.value - t.NumericValue);
        }

        /// <summary>
        /// Subtracts two time instants, returning a timespan.
        /// </summary>
        public static TimeSpan operator -(Instant i0, Instant i1)
        {
            return new TimeSpan(i0.value - i1.value);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two time instants for equality.
        /// </summary>
        public static bool operator ==(Instant i0, Instant i1)
        {
            return i0.Equals(i1);
        }

        /// <summary>
        /// Compares two time instants for inequality.
        /// </summary>
        public static bool operator !=(Instant i0, Instant i1)
        {
            return !(i0 == i1);
        }

        /// <summary>
        /// Checks if one time instant is smaller than another.
        /// </summary>
        public static bool operator <(Instant i0, Instant i1)
        {
            return i0.value < i1.value;
        }

        /// <summary>
        /// Checks if one time instant is larger than another.
        /// </summary>
        public static bool operator >(Instant i0, Instant i1)
        {
            return i0.value > i1.value;
        }

        /// <summary>
        /// Checks if one time instant is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Instant i0, Instant i1)
        {
            return i0.value <= i1.value;
        }

        /// <summary>
        /// Checks if one time instant is larger or equal to another.
        /// </summary>
        public static bool operator >=(Instant i0, Instant i1)
        {
            return i0.value >= i1.value;
        }

        #endregion

        #endregion

    }
}