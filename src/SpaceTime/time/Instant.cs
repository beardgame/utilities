namespace Bearded.Utilities.SpaceTime
{
    struct Instant : IBackedBy<double>
    {
        private readonly double value;

        public Instant(double value)
        {
            this.value = value;
        }

        public double NumericValue { get { return this.value; } }

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

        #endregion
    }
}