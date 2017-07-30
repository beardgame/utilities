using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of a signed timespan.
    /// </summary>
    public struct TimeSpan : IEquatable<TimeSpan>, IComparable<TimeSpan>
    {
        private readonly double value;

        #region construction
        
        public TimeSpan(double value)
        {
            this.value = value;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the timespan.
        /// </summary>
        public double NumericValue => value;

        /// <summary>
        /// Returns the timespan with value 0.
        /// </summary>
        public static TimeSpan Zero => new TimeSpan(0);

        /// <summary>
        /// Returns the timespan with value 1.
        /// </summary>
        public static TimeSpan One => new TimeSpan(1);

        #endregion

        #region methods

        #region equality and hashcode

        // ReSharper disable once CompareOfFloatsByEqualityOperator
        public bool Equals(TimeSpan other) => value == other.value;

        public override bool Equals(object obj) => obj is TimeSpan && Equals((TimeSpan)obj);

        public override int GetHashCode() => value.GetHashCode();

        #endregion

        #region compare

        public int CompareTo(TimeSpan other) => value.CompareTo(other.value);

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two timespans.
        /// </summary>
        public static TimeSpan operator +(TimeSpan t0, TimeSpan t1) => new TimeSpan(t0.value + t1.value);

        /// <summary>
        /// Adds two timespans.
        /// </summary>
        public static TimeSpan operator -(TimeSpan t0, TimeSpan t1) => new TimeSpan(t0.value - t1.value);

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the timespan.
        /// </summary>
        public static TimeSpan operator -(TimeSpan t) => new TimeSpan(-t.value);

        /// <summary>
        /// Multiples the timespan with a scalar.
        /// </summary>
        public static TimeSpan operator *(TimeSpan t, float scalar) => new TimeSpan(t.value * scalar);

        /// <summary>
        /// Multiples the timespan with a scalar.
        /// </summary>
        public static TimeSpan operator *(float scalar, TimeSpan t) => new TimeSpan(t.value * scalar);

        /// <summary>
        /// Divides the timespan by a divisor.
        /// </summary>
        public static TimeSpan operator /(TimeSpan t, float divisor) => new TimeSpan(t.value / divisor);

        #endregion

        #region ratio

        /// <summary>
        /// Devides a timespan by another, returning a type-less fraction.
        /// </summary>
        public static double operator /(TimeSpan dividend, TimeSpan divisor) => dividend.value / divisor.value;

        #endregion

        #region comparision

        /// <summary>
        /// Compares two timespans for equality.
        /// </summary>
        public static bool operator ==(TimeSpan t0, TimeSpan t1) => t0.Equals(t1);

        /// <summary>
        /// Compares two timespans for inequality.
        /// </summary>
        public static bool operator !=(TimeSpan t0, TimeSpan t1) => !(t0 == t1);

        /// <summary>
        /// Checks if one timespan is smaller than another.
        /// </summary>
        public static bool operator <(TimeSpan t0, TimeSpan t1) => t0.value < t1.value;

        /// <summary>
        /// Checks if one timespan is larger than another.
        /// </summary>
        public static bool operator >(TimeSpan t0, TimeSpan t1) => t0.value > t1.value;

        /// <summary>
        /// Checks if one timespan is smaller or equal to another.
        /// </summary>
        public static bool operator <=(TimeSpan t0, TimeSpan t1) => t0.value <= t1.value;

        /// <summary>
        /// Checks if one timespan is larger or equal to another.
        /// </summary>
        public static bool operator >=(TimeSpan t0, TimeSpan t1) => t0.value >= t1.value;

        #endregion

        #region angle differentiation

        /// <summary>
        /// Divides an angle by a timespan, returning an angular acceleration.
        /// </summary>
        public static AngularVelocity operator /(Angle s, TimeSpan t) => AngularVelocity.FromRadians(s.Radians / (float)t.value);

        #endregion

        #endregion

    }
}