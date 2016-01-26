using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    public struct AngularVelocity : IBackedBy<float>, IEquatable<AngularVelocity>, IComparable<AngularVelocity>
    {
        private readonly float value;

        #region constructing

        private AngularVelocity(float value)
        {
            this.value = value;
        }

        public static AngularVelocity FromRadians(float radians)
        {
            return new AngularVelocity(radians);
        }
        public static AngularVelocity FromDegrees(float radians)
        {
            return new AngularVelocity(radians);
        }

        #endregion

        public Angle AngleValue { get { return Angle.FromRadians(this.value); } }

        public float NumericValue { get { return this.value; } }

        public static AngularVelocity Zero { get { return new AngularVelocity(0); } }
        public static AngularVelocity One { get { return new AngularVelocity(1); } }

        #region methods

        #region equality and hashcode

        public bool Equals(AngularVelocity other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AngularVelocity && this.Equals((AngularVelocity)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(AngularVelocity other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        public static AngularVelocity operator +(AngularVelocity v0, AngularVelocity v1)
        {
            return new AngularVelocity(v0.value + v1.value);
        }
        public static AngularVelocity operator -(AngularVelocity v0, AngularVelocity v1)
        {
            return new AngularVelocity(v0.value - v1.value);
        }

        #endregion

        #region scaling

        public static AngularVelocity operator -(AngularVelocity s)
        {
            return new AngularVelocity(-s.value);
        }
        public static AngularVelocity operator *(AngularVelocity s, float scalar)
        {
            return new AngularVelocity(s.value * scalar);
        }
        public static AngularVelocity operator *(float scalar, AngularVelocity s)
        {
            return new AngularVelocity(s.value * scalar);
        }
        public static AngularVelocity operator /(AngularVelocity s, float divisor)
        {
            return new AngularVelocity(s.value / divisor);
        }

        #endregion

        #region ratio

        public static float operator /(AngularVelocity dividend, AngularVelocity divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region differentiate

        public static AngularAcceleration operator /(AngularVelocity s, TimeSpan t)
        {
            return AngularAcceleration.FromRadians(s.value / (float)t.NumericValue);
        }

        #endregion

        #region integrate

        public static Angle operator *(AngularVelocity s, TimeSpan t)
        {
            return Angle.FromRadians(s.value * (float)t.NumericValue);
        }
        public static Angle operator *(TimeSpan t, AngularVelocity s)
        {
            return Angle.FromRadians(s.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        public static bool operator ==(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value == v1.value;
        }

        public static bool operator !=(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value != v1.value;
        }

        public static bool operator <(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value < v1.value;
        }

        public static bool operator >(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value > v1.value;
        }

        public static bool operator <=(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value <= v1.value;
        }

        public static bool operator >=(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value >= v1.value;
        }

        #endregion

        #endregion

    }
}