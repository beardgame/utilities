using Bearded.Utilities.Math;
using OpenTK;

namespace Bearded.Utilities.SpaceTime
{
    struct Velocity2 : IBackedBy<Vector2>
    {
        private readonly Vector2 value;

        public Velocity2(Vector2 value)
        {
            this.value = value;
        }

        #region properties

        public Vector2 NumericValue { get { return this.value; } }

        public Direction2 Direction { get { return Direction2.Of(this.value); } }

        public Speed Length { get { return new Speed(this.value.Length); } }

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

        #endregion
    }
}