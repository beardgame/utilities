namespace Bearded.Utilities.SpaceTime
{
    struct TimeSpan : IBackedBy<double>
    {
        private readonly double value;

        public TimeSpan(double value)
        {
            this.value = value;
        }

        public double NumericValue { get { return this.value; } }

        #region operators

        #region algebra

        public static TimeSpan operator +(TimeSpan t0, TimeSpan t1)
        {
            return new TimeSpan(t0.value + t1.value);
        }
        public static TimeSpan operator -(TimeSpan t0, TimeSpan t1)
        {
            return new TimeSpan(t0.value - t1.value);
        }

        #endregion

        #region scaling

        public static TimeSpan operator -(TimeSpan t)
        {
            return new TimeSpan(-t.value);
        }
        public static TimeSpan operator *(TimeSpan t, float scalar)
        {
            return new TimeSpan(t.value * scalar);
        }
        public static TimeSpan operator *(float scalar, TimeSpan t)
        {
            return new TimeSpan(t.value * scalar);
        }
        public static TimeSpan operator /(TimeSpan t, float divisor)
        {
            return new TimeSpan(t.value / divisor);
        }

        #endregion

        #region ratio

        public static float operator /(TimeSpan dividend, TimeSpan divisor)
        {
            return dividend.value / divisor.value;
        }

        #endregion

        #endregion
    }
}