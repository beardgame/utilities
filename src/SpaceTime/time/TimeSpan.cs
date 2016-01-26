using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    public struct TimeSpan : IBackedBy<double>, IEquatable<TimeSpan>, IComparable<TimeSpan>
    {
        private readonly double value;

        public TimeSpan(double value)
        {
            this.value = value;
        }

        public double NumericValue { get { return this.value; } }

        public static TimeSpan Zero { get { return new TimeSpan(0); } }
        public static TimeSpan One { get { return new TimeSpan(1); } }

        #region methods

        #region equality and hashcode

        public bool Equals(TimeSpan other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is TimeSpan && this.Equals((TimeSpan)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(TimeSpan other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        public static TimeSpan operator +(TimeSpan t0, TimeSpan t1)
        {
            return new TimeSpan(t0.value + t1.value);
        }
        public static TimeSpan operator -(TimeSpan t0, TimeSpan t1)
        {
            return new TimeSpan(t0.value - t1.value);
        }

        #endregion

        #region scaling

        public static TimeSpan operator -(TimeSpan t)
        {
            return new TimeSpan(-t.value);
        }
        public static TimeSpan operator *(TimeSpan t, float scalar)
        {
            return new TimeSpan(t.value * scalar);
        }
        public static TimeSpan operator *(float scalar, TimeSpan t)
        {
            return new TimeSpan(t.value * scalar);
        }
        public static TimeSpan operator /(TimeSpan t, float divisor)
        {
            return new TimeSpan(t.value / divisor);
        }

        #endregion

        #region ratio

        public static double operator /(TimeSpan dividend, TimeSpan divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region comparision

        public static bool operator ==(TimeSpan t0, TimeSpan t1)
        {
            return t0.value == t1.value;
        }

        public static bool operator !=(TimeSpan t0, TimeSpan t1)
        {
            return t0.value != t1.value;
        }

        public static bool operator <(TimeSpan t0, TimeSpan t1)
        {
            return t0.value < t1.value;
        }

        public static bool operator >(TimeSpan t0, TimeSpan t1)
        {
            return t0.value > t1.value;
        }

        public static bool operator <=(TimeSpan t0, TimeSpan t1)
        {
            return t0.value <= t1.value;
        }

        public static bool operator >=(TimeSpan t0, TimeSpan t1)
        {
            return t0.value >= t1.value;
        }

        #endregion

        #region angle differentiation

        public static AngularVelocity operator /(Angle s, TimeSpan t)
        {
            return AngularVelocity.FromRadians(s.Radians / (float)t.value);
        }

        #endregion

        #endregion

    }
}