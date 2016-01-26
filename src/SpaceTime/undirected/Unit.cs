
using System;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    struct Unit : IBackedBy<float>, IEquatable<Unit>, IComparable<Unit>
    {
        private readonly float value;

        public Unit(float value)
        {
            this.value = value;
        }

        public float NumericValue { get { return this.value; } }

        public Squared<Unit> Squared { get { return Squared<Unit>.FromRoot(this.value); } }

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

        public static Unit operator +(Unit u0, Unit u1)
        {
            return new Unit(u0.value + u1.value);
        }
        public static Unit operator -(Unit u0, Unit u1)
        {
            return new Unit(u0.value - u1.value);
        }

        #endregion

        #region scaling

        public static Unit operator -(Unit u)
        {
            return new Unit(-u.value);
        }
        public static Unit operator *(Unit u, float scalar)
        {
            return new Unit(u.value * scalar);
        }
        public static Unit operator *(float scalar, Unit u)
        {
            return new Unit(u.value * scalar);
        }
        public static Unit operator /(Unit u, float divisor)
        {
            return new Unit(u.value / divisor);
        }

        #endregion

        #region ratio

        public static float operator /(Unit dividend, Unit divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region differentiate

        public static Speed operator /(Unit u, TimeSpan t)
        {
            return new Speed(u.value / (float)t.NumericValue);
        }

        #endregion

        #region add dimension

        public static Difference2 operator *(Unit u, Direction2 d)
        {
            return new Difference2(d.Vector * u.value);
        }
        public static Difference2 operator *(Direction2 d, Unit u)
        {
            return new Difference2(d.Vector * u.value);
        }

        #endregion

        #region comparision

        public static bool operator ==(Unit u0, Unit u1)
        {
            return u0.value == u1.value;
        }

        public static bool operator !=(Unit u0, Unit u1)
        {
            return u0.value != u1.value;
        }

        public static bool operator <(Unit u0, Unit u1)
        {
            return u0.value < u1.value;
        }

        public static bool operator >(Unit u0, Unit u1)
        {
            return u0.value > u1.value;
        }

        public static bool operator <=(Unit u0, Unit u1)
        {
            return u0.value <= u1.value;
        }

        public static bool operator >=(Unit u0, Unit u1)
        {
            return u0.value >= u1.value;
        }

        #endregion

        #endregion

    }
}