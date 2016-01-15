
using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    struct Acceleration
    {
        readonly float value;
        
        public Acceleration(float value)
        {
            this.value = value;
        }

        public float NumericValue { get { return this.value; } }


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

        #endregion

        #endregion
    }
}