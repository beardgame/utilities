using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    struct Speed
    {
        readonly float value;

        public Speed(float value)
        {
            this.value = value;
        }

        public float NumericValue { get { return this.value; } }

        #region operators

        #region algebra

        public static Speed operator +(Speed s0, Speed s1)
        {
            return new Speed(s0.value + s1.value);
        }
        public static Speed operator -(Speed s0, Speed s1)
        {
            return new Speed(s0.value - s1.value);
        }

        #endregion

        #region scaling

        public static Speed operator -(Speed s)
        {
            return new Speed(-s.value);
        }
        public static Speed operator *(Speed s, float scalar)
        {
            return new Speed(s.value * scalar);
        }
        public static Speed operator *(float scalar, Speed s)
        {
            return new Speed(s.value * scalar);
        }
        public static Speed operator /(Speed s, float divisor)
        {
            return new Speed(s.value / divisor);
        }

        #endregion

        #region ratio

        public static float operator /(Speed dividend, Speed divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #region differentiate

        public static Acceleration operator /(Speed s, TimeSpan t)
        {
            return new Acceleration(s.value / (float)t.NumericValue);
        }

        #endregion

        #region integrate

        public static Unit operator *(Speed s, TimeSpan t)
        {
            return new Unit(s.value * (float)t.NumericValue);
        }
        public static Unit operator *(TimeSpan t, Speed s)
        {
            return new Unit(s.value * (float)t.NumericValue);
        }

        #endregion

        #region add dimension

        public static Velocity2 operator *(Speed s, Direction2 d)
        {
            return new Velocity2(d.Vector * s.value);
        }
        public static Velocity2 operator *(Direction2 d, Speed s)
        {
            return new Velocity2(d.Vector * s.value);
        }

        #endregion

        #endregion
    }
}