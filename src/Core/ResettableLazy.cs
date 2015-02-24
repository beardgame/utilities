using System;

namespace Bearded.Utilities
{
    /// <summary>
    /// Static class to help make resetable lazies.
    /// </summary>
    public static class ResettableLazy
    {
        /// <summary>
        /// Creates a new resetable lazy with a given initialisation function.
        /// </summary>
        /// <param name="maker">The function to initialise the value of the lazy.</param>
        public static ResettableLazy<T> From<T>(Func<T> maker)
        {
            return new ResettableLazy<T>(maker);
        }
    }

    /// <summary>
    /// This class represents a lazily initialised value. It can be reset to call the initialisation again afterwards.
    /// It is not thread safe.
    /// </summary>
    public sealed class ResettableLazy<T>
    {
        private readonly Func<T> maker;

        private bool hasValue;
        private T value;

        /// <summary>
        /// Gets the value of this instance.
        /// </summary>
        public T Value
        {
            get
            {
                if (!this.hasValue)
                {
                    this.value = maker();
                    this.hasValue = true;
                }
                return this.value;
            }
        }

        /// <summary>
        /// Creates a new resetable lazy instance.
        /// </summary>
        /// <param name="maker">The function that initialises the value for this instance.</param>
        public ResettableLazy(Func<T> maker)
        {
            this.maker = maker;
        }

        /// <summary>
        /// Resets the lazy to get a new value when accessed next.
        /// </summary>
        public void Reset()
        {
            this.hasValue = false;
        }
    }
}
