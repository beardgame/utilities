using System;

namespace Bearded.Utilities.SpaceTime
{
    public struct Instant : IBackedBy<double>, IEquatable<Instant>, IComparable<Instant>
    {
        private readonly double value;

        public Instant(double value)
        {
            this.value = value;
        }

        public double NumericValue { get { return this.value; } }

        public static Instant Zero { get { return new Instant(0); } }

        #region methods

        #region equality and hashcode

        public bool Equals(Instant other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
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

        public static Instant operator +(Instant i, TimeSpan t)
        {
            return new Instant(i.value + t.NumericValue);
        }
        public static Instant operator +(TimeSpan t, Instant i)
        {
            return new Instant(i.value + t.NumericValue);
        }
        public static Instant operator -(Instant i, TimeSpan t)
        {
            return new Instant(i.value - t.NumericValue);
        }
        public static TimeSpan operator -(Instant i0, Instant i1)
        {
            return new TimeSpan(i0.value - i1.value);
        }

        #endregion

        #region comparision

        public static bool operator ==(Instant i0, Instant i1)
        {
            return i0.value == i1.value;
        }

        public static bool operator !=(Instant i0, Instant i1)
        {
            return i0.value != i1.value;
        }

        public static bool operator <(Instant i0, Instant i1)
        {
            return i0.value < i1.value;
        }

        public static bool operator >(Instant i0, Instant i1)
        {
            return i0.value > i1.value;
        }

        public static bool operator <=(Instant i0, Instant i1)
        {
            return i0.value <= i1.value;
        }

        public static bool operator >=(Instant i0, Instant i1)
        {
            return i0.value >= i1.value;
        }

        #endregion

        #endregion

    }
}