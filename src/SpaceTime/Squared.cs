using System;
using System.CodeDom;
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    struct Squared<T> : IBackedBy<float>, IEquatable<Squared<T>>, IComparable<Squared<T>>
        where T : struct, IBackedBy<float>
    {
        private readonly float value;

        #region construcing

        internal Squared(float value)
        {
            this.value = value;
        }

        public static Squared<T> FromRoot(float root)
        {
            return new Squared<T>(root.Squared());
        }
        public static Squared<T> FromValue(float value)
        {
            if(value < 0)
                throw new ArgumentOutOfRangeException("value", "Must be non-negative.");

            return new Squared<T>(value);
        }

        #endregion

        #region properties

        public float NumericValue { get { return this.value; } }

        #endregion

        #region methods

        #region equality and hashcode

        public bool Equals(Squared<T> other)
        {
            return this == other;
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

        public static Squared<T> operator +(Squared<T> s0, Squared<T> s1)
        {
            return new Squared<T>(s0.value + s1.value);
        }

        #endregion

        #region ratio

        public static float operator /(Squared<T> dividend, Squared<T> divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region comparision

        public static bool operator ==(Squared<T> s0, Squared<T> s1)
        {
            return s0.value == s1.value;
        }

        public static bool operator !=(Squared<T> s0, Squared<T> s1)
        {
            return s0.value != s1.value;
        }

        public static bool operator <(Squared<T> s0, Squared<T> s1)
        {
            return s0.value < s1.value;
        }

        public static bool operator >(Squared<T> s0, Squared<T> s1)
        {
            return s0.value > s1.value;
        }

        public static bool operator <=(Squared<T> s0, Squared<T> s1)
        {
            return s0.value <= s1.value;
        }

        public static bool operator >=(Squared<T> s0, Squared<T> s1)
        {
            return s0.value >= s1.value;
        }

        #endregion

        #endregion
    }
}