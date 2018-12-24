namespace Bearded.Utilities
{
    public static class Do
    {
        public static void Swap<T>(ref T a, ref T b)
        {
            (a, b) = (b, a);
        }

        public static Box<T> Box<T>(T value)
            where T : struct
            => new Box<T>(value);
    }
}
