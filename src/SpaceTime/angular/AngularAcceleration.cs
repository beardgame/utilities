using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    struct AngularAcceleration : IBackedBy<float>, IEquatable<AngularAcceleration>, IComparable<AngularAcceleration>
    {
        private readonly float value;

        #region constructing

        private AngularAcceleration(float value)
        {
            this.value = value;
        }

        public static AngularAcceleration FromRadians(float radians)
        {
            return new AngularAcceleration(radians);
        }
        public static AngularAcceleration FromDegrees(float radians)
        {
            return new AngularAcceleration(radians);
        }

        #endregion

        public Angle AngleValue { get { return Angle.FromRadians(this.value); } }

        public float NumericValue { get { return this.value; } }

        #region methods

        #region equality and hashcode

        public bool Equals(AngularAcceleration other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is AngularAcceleration && this.Equals((AngularAcceleration)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(AngularAcceleration other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        public static AngularAcceleration operator +(AngularAcceleration v0, AngularAcceleration v1)
        {
            return new AngularAcceleration(v0.value + v1.value);
        }
        public static AngularAcceleration operator -(AngularAcceleration v0, AngularAcceleration v1)
        {
            return new AngularAcceleration(v0.value - v1.value);
        }

        #endregion

        #region scaling

        public static AngularAcceleration operator -(AngularAcceleration s)
        {
            return new AngularAcceleration(-s.value);
        }
        public static AngularAcceleration operator *(AngularAcceleration s, float scalar)
        {
            return new AngularAcceleration(s.value * scalar);
        }
        public static AngularAcceleration operator *(float scalar, AngularAcceleration s)
        {
            return new AngularAcceleration(s.value * scalar);
        }
        public static AngularAcceleration operator /(AngularAcceleration s, float divisor)
        {
            return new AngularAcceleration(s.value / divisor);
        }

        #endregion

        #region ratio

        public static float operator /(AngularAcceleration dividend, AngularAcceleration divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region integrate

        public static AngularVelocity operator *(AngularAcceleration s, TimeSpan t)
        {
            return AngularVelocity.FromRadians(s.value * (float)t.NumericValue);
        }
        public static AngularVelocity operator *(TimeSpan t, AngularAcceleration s)
        {
            return AngularVelocity.FromRadians(s.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        public static bool operator ==(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value == v1.value;
        }

        public static bool operator !=(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value != v1.value;
        }

        public static bool operator <(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value < v1.value;
        }

        public static bool operator >(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value > v1.value;
        }

        public static bool operator <=(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value <= v1.value;
        }

        public static bool operator >=(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value >= v1.value;
        }

        #endregion

        #endregion

    }
}