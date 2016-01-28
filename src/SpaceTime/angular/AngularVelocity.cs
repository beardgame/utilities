using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of a signed ancular velocity.
    /// </summary>
    public struct AngularVelocity : IBackedBy<float>, IEquatable<AngularVelocity>, IComparable<AngularVelocity>
    {
        private readonly float value;

        #region constructing

        /// <summary>
        /// Creates a new instance of the AngularVelocity type.
        /// </summary>
        public AngularVelocity(Angle value)
            : this(value.Radians)
        {
        }

        private AngularVelocity(float value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of the AngularVelocity type from an angle in radians.
        /// </summary>
        public static AngularVelocity FromRadians(float radians)
        {
            return new AngularVelocity(radians);
        }
        /// <summary>
        /// Creates a new instance of the AngularVelocity type from an angle in degrees.
        /// </summary>
        public static AngularVelocity FromDegrees(float degrees)
        {
            return new AngularVelocity(Mathf.DegreesToRadians(degrees));
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the angular velocity in radians.
        /// </summary>
        public float NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the angular value of the angular velocity.
        /// </summary>
        public Angle AngleValue { get { return Angle.FromRadians(this.value); } }

        /// <summary>
        /// Returns an angular velocity with value 0.
        /// </summary>
        public static AngularVelocity Zero { get { return new AngularVelocity(0); } }

        #endregion

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

        /// <summary>
        /// Adds two angular velocity values.
        /// </summary>
        public static AngularVelocity operator +(AngularVelocity v0, AngularVelocity v1)
        {
            return new AngularVelocity(v0.value + v1.value);
        }
        /// <summary>
        /// Adds two angular velocity values.
        /// </summary>
        public static AngularVelocity operator -(AngularVelocity v0, AngularVelocity v1)
        {
            return new AngularVelocity(v0.value - v1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the angular velocity.
        /// </summary>
        public static AngularVelocity operator -(AngularVelocity s)
        {
            return new AngularVelocity(-s.value);
        }
        /// <summary>
        /// Multiplies the angular velocity with a scalar.
        /// </summary>
        public static AngularVelocity operator *(AngularVelocity s, float scalar)
        {
            return new AngularVelocity(s.value * scalar);
        }
        /// <summary>
        /// Multiplies the angular velocity with a scalar.
        /// </summary>
        public static AngularVelocity operator *(float scalar, AngularVelocity s)
        {
            return new AngularVelocity(s.value * scalar);
        }
        /// <summary>
        /// Divides the angular velocity by a divisor.
        /// </summary>
        public static AngularVelocity operator /(AngularVelocity s, float divisor)
        {
            return new AngularVelocity(s.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides an angular velocity by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(AngularVelocity dividend, AngularVelocity divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region differentiate

        /// <summary>
        /// Divides an angular velocity by a timespan, returning an angular acceleration.
        /// </summary>
        public static AngularAcceleration operator /(AngularVelocity s, TimeSpan t)
        {
            return AngularAcceleration.FromRadians(s.value / (float)t.NumericValue);
        }

        #endregion

        #region integrate

        /// <summary>
        /// Multiplies an angular velocity by a timespan, returning an angle.
        /// </summary>
        public static Angle operator *(AngularVelocity s, TimeSpan t)
        {
            return Angle.FromRadians(s.value * (float)t.NumericValue);
        }
        /// <summary>
        /// Multiplies an angular velocity by a timespan, returning an angle.
        /// </summary>
        public static Angle operator *(TimeSpan t, AngularVelocity s)
        {
            return Angle.FromRadians(s.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two angular velocities for equality.
        /// </summary>
        public static bool operator ==(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value == v1.value;
        }
        /// <summary>
        /// Compares two angular velocities for inequality.
        /// </summary>
        public static bool operator !=(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value != v1.value;
        }
        /// <summary>
        /// Checks if one angular velocity is smaller than another.
        /// </summary>
        public static bool operator <(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value < v1.value;
        }
        /// <summary>
        /// Checks if one angular velocity is larger than another.
        /// </summary>
        public static bool operator >(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value > v1.value;
        }
        /// <summary>
        /// Checks if one angular velocity is smaller or equal to another.
        /// </summary>
        public static bool operator <=(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value <= v1.value;
        }
        /// <summary>
        /// Checks if one angular velocity is larger or equal to another.
        /// </summary>
        public static bool operator >=(AngularVelocity v0, AngularVelocity v1)
        {
            return v0.value >= v1.value;
        }

        #endregion

        #endregion

    }
}