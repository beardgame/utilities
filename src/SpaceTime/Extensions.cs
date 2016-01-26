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
    }
}