namespace Bearded.Utilities
{
    /// <summary>
    /// Collection of methods performing simple operations.
    /// </summary>
    static class Do
    {
        /// <summary>
        /// Swaps the values of the two variables.
        /// </summary>
        /// <typeparam name="T">The type of the two variables.</typeparam>
        /// <param name="a">The first variable.</param>
        /// <param name="b">The second variable.</param>
        public static void Swap<T>(ref T a, ref T b)
        {
            var t = a;
            a = b;
            b = t;
        }
    }
}