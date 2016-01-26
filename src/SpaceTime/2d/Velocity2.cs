using System;
using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    public struct Velocity2 : IBackedBy<Vector2>, IEquatable<Velocity2>
    {
        private readonly Vector2 value;

        public Velocity2(Vector2 value)
        {
            this.value = value;
        }
        
        public Velocity2(float x, float y)
            : this(new Vector2(x, y))
        {
        }

        public Velocity2(Speed x, Speed y)
            : this(new Vector2(x.NumericValue, y.NumericValue))
        {
        }

        public static Velocity2 In(Direction2 direction, Speed u)
        {
            return direction * u;
        }

        #region properties

        public Vector2 NumericValue { get { return this.value; } }

        public Speed X { get { return new Speed(this.value.X); } }
        public Speed Y { get { return new Speed(this.value.Y); } }

        public Direction2 Direction { get { return Direction2.Of(this.value); } }

        public Speed Length { get { return new Speed(this.value.Length); } }

        public Squared<Speed> LengthSquared { get { return new Squared<Speed>(this.value.LengthSquared); } }

        #endregion

        #region methods

        #region lerp

        public static Velocity2 Lerp(Velocity2 v0, Velocity2 v1, float t)
        {
            return v0 + (v1 - v0) * t;
        }

        public Velocity2 LerpTo(Velocity2 v, float t)
        {
            return Lerp(this, v, t);
        }

        #endregion

        #region projection

        public Speed ProjectedOn(Vector2 vector)
        {
            return this.projectedOn(vector.NormalizedSafe());
        }

        public Speed ProjectedOn(Difference2 vector)
        {
            return this.projectedOn(vector.NumericValue.NormalizedSafe());
        }

        public Speed ProjectedOn(Direction2 direction)
        {
            return this.projectedOn(direction.Vector);
        }

        private Speed projectedOn(Vector2 normalisedVector)
        {
            return new Speed(Vector2.Dot(this.value, normalisedVector));
        }

        #endregion


        #region equality and hashcode

        public bool Equals(Velocity2 other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            return obj is Velocity2 && this.Equals((Velocity2)obj);
        }

        public override int GetHashCode()
        {
            return this.value.GetHashCode();
        }

        #endregion

        #endregion

        #region operators

        #region algebra

        public static Velocity2 operator +(Velocity2 v0, Velocity2 v1)
        {
            return new Velocity2(v0.value + v1.value);
        }
        public static Velocity2 operator -(Velocity2 v0, Velocity2 v1)
        {
            return new Velocity2(v0.value - v1.value);
        }

        #endregion

        #region scaling

        public static Velocity2 operator -(Velocity2 v)
        {
            return new Velocity2(-v.value);
        }
        public static Velocity2 operator *(Velocity2 v, float scalar)
        {
            return new Velocity2(v.value * scalar);
        }
        public static Velocity2 operator *(float scalar, Velocity2 v)
        {
            return new Velocity2(v.value * scalar);
        }
        public static Velocity2 operator /(Velocity2 v, float divisor)
        {
            return new Velocity2(v.value / divisor);
        }

        #endregion

        #region ratio

        public static Vector2 operator /(Velocity2 v, Speed divisor)
        {
            return v.value / divisor.NumericValue;
        }

        #endregion

        #region differentiate

        public static Acceleration2 operator /(Velocity2 v, TimeSpan t)
        {
            return new Acceleration2(v.value / (float)t.NumericValue);
        }

        #endregion

        #region integrate

        public static Difference2 operator *(Velocity2 v, TimeSpan t)
        {
            return new Difference2(v.value * (float)t.NumericValue);
        }
        public static Difference2 operator *(TimeSpan t, Velocity2 v)
        {
            return new Difference2(v.value * (float)t.NumericValue);
        }

        #endregion

        #region comparision

        public static bool operator ==(Velocity2 v0, Velocity2 v1)
        {
            return v0.value == v1.value;
        }

        public static bool operator !=(Velocity2 v0, Velocity2 v1)
        {
            return v0.value != v1.value;
        }

        #endregion

        #endregion

    }
}