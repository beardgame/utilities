using Bearded.Utilities.Math;

namespace Bearded.Utilities.SpaceTime
{
    static class Extensions
    {
        public static Unit U(this float value)
        {
            return new Unit(value);
        }
        public static Unit U(this int value)
        {
            return new Unit(value);
        }


        public static Unit Sqrt(this Squared<Unit> square)
        {
            return new Unit(square.NumericValue.Sqrted());
        }
        public static Speed Sqrt(this Squared<Speed> square)
        {
            return new Speed(square.NumericValue.Sqrted());
        }
        public static Acceleration Sqrt(this Squared<Acceleration> square)
        {
            return new Acceleration(square.NumericValue.Sqrted());
        }
    }
}