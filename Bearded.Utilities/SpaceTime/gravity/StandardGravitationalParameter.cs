namespace Bearded.Utilities.SpaceTime
{
    public struct StandardGravitationalParameter
    {
        // cubic unit per second squared
        private readonly float value;

        public StandardGravitationalParameter(float value)
        {
            this.value = value;
        }

        public float NumericValue => value;

        public static Squared<TimeSpan> operator /(Cubed<Unit> cube, StandardGravitationalParameter mu) =>
            new Squared<TimeSpan>(cube.NumericValue / mu.NumericValue);
    }
}