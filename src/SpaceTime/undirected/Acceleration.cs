
using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    public struct Acceleration : IBackedBy<float>, IEquatable<Acceleration>, IComparable<Acceleration>
    {
        private readonly float value;
        
        public Acceleration(float value)
        {
            this.value = value;
        }

        public float NumericValue { get { return this.value; } }

        public Squared<Acceleration> Squared { get { return Squared<Acceleration>.FromRoot(this.value); } }

        public static Acceleration Zero { get { return new Acceleration(0); } }
        public static Acceleration One { get { return new Acceleration(1); } }

        #region methods

        #region equality and hashcode

        public bool Equals(Acceleration other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Acceleration && this.Equals((Acceleration)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #region compare

        public int CompareTo(Acceleration other)
        {
            return this.value.CompareTo(other.value);
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        public static Acceleration operator +(Acceleration a0, Acceleration a1)
        {
            return new Acceleration(a0.value + a1.value);
        }
        public static Acceleration operator -(Acceleration a0, Acceleration a1)
        {
            return new Acceleration(a0.value - a1.value);
        }

        #endregion

        #region scaling

        public static Acceleration operator -(Acceleration a)
        {
            return new Acceleration(-a.value);
        }
        public static Acceleration operator *(Acceleration a, float scalar)
        {
            return new Acceleration(a.value * scalar);
        }
        public static Acceleration operator *(float scalar, Acceleration a)
        {
            return new Acceleration(a.value * scalar);
        }
        public static Acceleration operator /(Acceleration a, float divisor)
        {
            return new Acceleration(a.value / divisor);
        }

        #endregion

        #region ratio

        public static float operator /(Acceleration dividend, Acceleration divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region integrate

        public static Speed operator *(Acceleration a, TimeSpan t)
        {
            return new Speed(a.value * (float)t.NumericValue);
        }
        public static Speed operator *(TimeSpan t, Acceleration a)
        {
            return new Speed(a.value * (float)t.NumericValue);
        }

        #endregion

        #region add dimension

        public static Acceleration2 operator *(Acceleration a, Direction2 d)
        {
            return new Acceleration2(d.Vector * a.value);
        }
        public static Acceleration2 operator *(Direction2 d, Acceleration a)
        {
            return new Acceleration2(d.Vector * a.value);
        }

        public static Acceleration2 operator *(Acceleration u, Vector2 v)
        {
            return new Acceleration2(v * u.value);
        }
        public static Acceleration2 operator *(Vector2 v, Acceleration u)
        {
            return new Acceleration2(v * u.value);
        }

        #endregion

        #region comparision

        public static bool operator ==(Acceleration a0, Acceleration a1)
        {
            return a0.value == a1.value;
        }

        public static bool operator !=(Acceleration a0, Acceleration a1)
        {
            return a0.value != a1.value;
        }

        public static bool operator <(Acceleration a0, Acceleration a1)
        {
            return a0.value < a1.value;
        }

        public static bool operator >(Acceleration a0, Acceleration a1)
        {
            return a0.value > a1.value;
        }

        public static bool operator <=(Acceleration a0, Acceleration a1)
        {
            return a0.value <= a1.value;
        }

        public static bool operator >=(Acceleration a0, Acceleration a1)
        {
            return a0.value >= a1.value;
        }

        #endregion

        #endregion

    }
}