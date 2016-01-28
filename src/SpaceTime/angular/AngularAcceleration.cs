using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of a signed ancular acceleration.
    /// </summary>
    public struct AngularAcceleration : IBackedBy<float>, IEquatable<AngularAcceleration>, IComparable<AngularAcceleration>
    {
        private readonly float value;

        #region constructing

        /// <summary>
        /// Creates a new instance of the AngularAcceleration type.
        /// </summary>
        public AngularAcceleration(Angle value)
            : this(value.Radians)
        {
        }

        private AngularAcceleration(float value)
        {
            this.value = value;
        }

        /// <summary>
        /// Creates a new instance of the AngularAcceleration type from an angle in radians.
        /// </summary>
        public static AngularAcceleration FromRadians(float radians)
        {
            return new AngularAcceleration(radians);
        }
        /// <summary>
        /// Creates a new instance of the AngularAcceleration type from an angle in degrees.
        /// </summary>
        public static AngularAcceleration FromDegrees(float degrees)
        {
            return new AngularAcceleration(Mathf.DegreesToRadians(degrees));
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the angular acceleration in radians.
        /// </summary>
        public float NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the angular value of the angular acceleration.
        /// </summary>
        public Angle AngleValue { get { return Angle.FromRadians(this.value); } }

        /// <summary>
        /// Returns an angular acceleration with value 0.
        /// </summary>
        public static AngularAcceleration Zero { get { return new AngularAcceleration(0); } }

        #endregion

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

        /// <summary>
        /// Adds two angular acceleration values.
        /// </summary>
        public static AngularAcceleration operator +(AngularAcceleration v0, AngularAcceleration v1)
        {
            return new AngularAcceleration(v0.value + v1.value);
        }
        /// <summary>
        /// Adds two angular acceleration values.
        /// </summary>
        public static AngularAcceleration operator -(AngularAcceleration v0, AngularAcceleration v1)
        {
            return new AngularAcceleration(v0.value - v1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the angular acceleration.
        /// </summary>
        public static AngularAcceleration operator -(AngularAcceleration s)
        {
            return new AngularAcceleration(-s.value);
        }
        /// <summary>
        /// Multiplies the angular acceleration with a scalar.
        /// </summary>
        public static AngularAcceleration operator *(AngularAcceleration s, float scalar)
        {
            return new AngularAcceleration(s.value * scalar);
        }
        /// <summary>
        /// Multiplies the angular acceleration with a scalar.
        /// </summary>
        public static AngularAcceleration operator *(float scalar, AngularAcceleration s)
        {
            return new AngularAcceleration(s.value * scalar);
        }
        /// <summary>
        /// Divides the angular acceleration by a divisor.
        /// </summary>
        public static AngularAcceleration operator /(AngularAcceleration s, float divisor)
        {
            return new AngularAcceleration(s.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Divides an angular acceleration by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(AngularAcceleration dividend, AngularAcceleration divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region integrate

        /// <summary>
        /// Multiplies an angular acceleration by a timespan, returning an anglular velocity.
        /// </summary>
        public static AngularVelocity operator *(AngularAcceleration s, TimeSpan t)
        {
            return AngularVelocity.FromRadians(s.value * (float)t.NumericValue);
        }
        /// <summary>
        /// Multiplies an angular acceleration by a timespan, returning an anglular velocity.
        /// </summary>
        public static AngularVelocity operator *(TimeSpan t, AngularAcceleration s)
        {
            return AngularVelocity.FromRadians(s.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two angular accelerations for equality.
        /// </summary>
        public static bool operator ==(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value == v1.value;
        }
        /// <summary>
        /// Compares two angular accelerations for inequality.
        /// </summary>
        public static bool operator !=(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value != v1.value;
        }
        /// <summary>
        /// Checks if one angular acceleration is smaller than another.
        /// </summary>
        public static bool operator <(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value < v1.value;
        }
        /// <summary>
        /// Checks if one angular acceleration is larger than another.
        /// </summary>
        public static bool operator >(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value > v1.value;
        }
        /// <summary>
        /// Checks if one angular acceleration is smaller or equal to another.
        /// </summary>
        public static bool operator <=(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value <= v1.value;
        }
        /// <summary>
        /// Checks if one angular acceleration is larger or equal to another.
        /// </summary>
        public static bool operator >=(AngularAcceleration v0, AngularAcceleration v1)
        {
            return v0.value >= v1.value;
        }

        #endregion

        #endregion

    }
}