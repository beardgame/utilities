
namespace Bearded.Utilities
{
    /// <summary>
    /// A generic box to keep typesafe to store valuetypes on the heap.
    /// Advantages include reference sharing of a valuetype and atomic access for thread safety.
    /// </summary>
    public sealed class Box<T> where T : struct
    {
        private readonly T value;

        /// <summary>
        /// The value contained in the box.
        /// </summary>
        public T Value { get { return this.value; } }

        /// <summary>
        /// Initialises a new box with a given value.
        /// </summary>
        /// <param name="value">The value of the box.</param>
        public Box(T value)
        {
            this.value = value;
        }
    }
}
