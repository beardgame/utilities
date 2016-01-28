using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// A type-safe representation of an undirected signed distance or length.
    /// </summary>
    public struct Unit : IBackedBy<float>, IEquatable<Unit>, IComparable<Unit>
    {
        private readonly float value;

        #region construction

        /// <summary>
        /// Creates a new instance of the Unit type.
        /// </summary>
        public Unit(float value)
        {
            this.value = value;
        }

        #endregion

        #region properties

        /// <summary>
        /// Returns the numeric value of the unit value.
        /// </summary>
        public float NumericValue { get { return this.value; } }

        /// <summary>
        /// Returns the type-safe square of the unit value.
        /// </summary>
        public Squared<Unit> Squared { get { return Squared<Unit>.FromRoot(this.value); } }

        /// <summary>
        /// Returns a Unit type with value 0.
        /// </summary>
        public static Unit Zero { get { return new Unit(0); } }
        /// <summary>
        /// Returns a Unit type with value 1.
        /// </summary>
        public static Unit One { get { return new Unit(1); } }

        #endregion

        #region methods

        #region equality and hashcode

        public bool Equals(Unit other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Unit && this.Equals((Unit)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(Unit other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        /// <summary>
        /// Adds two unit values.
        /// </summary>
        public static Unit operator +(Unit u0, Unit u1)
        {
            return new Unit(u0.value + u1.value);
        }
        /// <summary>
        /// Subtracts a unit value from another.
        /// </summary>
        public static Unit operator -(Unit u0, Unit u1)
        {
            return new Unit(u0.value - u1.value);
        }

        #endregion

        #region scaling

        /// <summary>
        /// Inverts the unit value.
        /// </summary>
        public static Unit operator -(Unit u)
        {
            return new Unit(-u.value);
        }
        /// <summary>
        /// Multiples the unit value with a scalar.
        /// </summary>
        public static Unit operator *(Unit u, float scalar)
        {
            return new Unit(u.value * scalar);
        }
        /// <summary>
        /// Multiples the unit value with a scalar.
        /// </summary>
        public static Unit operator *(float scalar, Unit u)
        {
            return new Unit(u.value * scalar);
        }
        /// <summary>
        /// Divides the unit value by a divisor.
        /// </summary>
        public static Unit operator /(Unit u, float divisor)
        {
            return new Unit(u.value / divisor);
        }

        #endregion

        #region ratio

        /// <summary>
        /// Devides a unit value by another, returning a type-less fraction.
        /// </summary>
        public static float operator /(Unit dividend, Unit divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region differentiate

        /// <summary>
        /// Divides a unit value by a timespan, returning a speed.
        /// </summary>
        public static Speed operator /(Unit u, TimeSpan t)
        {
            return new Speed(u.value / (float)t.NumericValue);
        }

        #endregion

        #region add dimension

        /// <summary>
        /// Multiplies a direction with a unit value, returning a typed vector of the given direction and length.
        /// </summary>
        public static Difference2 operator *(Unit u, Direction2 d)
        {
            return new Difference2(d.Vector * u.value);
        }
        /// <summary>
        /// Multiplies a direction with a unit value, returning a typed vector of the given direction and length.
        /// </summary>
        public static Difference2 operator *(Direction2 d, Unit u)
        {
            return new Difference2(d.Vector * u.value);
        }

        /// <summary>
        /// Multiplies a unit value with an untyped vector, returning a typed vector.
        /// </summary>
        public static Difference2 operator *(Unit u, Vector2 v)
        {
            return new Difference2(v * u.value);
        }
        /// <summary>
        /// Multiplies a unit value with an untyped vector, returning a typed vector.
        /// </summary>
        public static Difference2 operator *(Vector2 v, Unit u)
        {
            return new Difference2(v * u.value);
        }

        #endregion

        #region comparision

        /// <summary>
        /// Compares two unit values for equality.
        /// </summary>
        public static bool operator ==(Unit u0, Unit u1)
        {
            return u0.value == u1.value;
        }
        /// <summary>
        /// Compares two unit values for inequality.
        /// </summary>
        public static bool operator !=(Unit u0, Unit u1)
        {
            return u0.value != u1.value;
        }
        /// <summary>
        /// Checks if one unit value is smaller than another.
        /// </summary>
        public static bool operator <(Unit u0, Unit u1)
        {
            return u0.value < u1.value;
        }
        /// <summary>
        /// Checks if one unit value is larger than another.
        /// </summary>
        public static bool operator >(Unit u0, Unit u1)
        {
            return u0.value > u1.value;
        }
        /// <summary>
        /// Checks if one unit value is smaller or equal to another.
        /// </summary>
        public static bool operator <=(Unit u0, Unit u1)
        {
            return u0.value <= u1.value;
        }
        /// <summary>
        /// Checks if one unit value is larger or equal to another.
        /// </summary>
        public static bool operator >=(Unit u0, Unit u1)
        {
            return u0.value >= u1.value;
        }

        #endregion

        #endregion

    }
}