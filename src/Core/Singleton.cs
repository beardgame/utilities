using System;

namespace Bearded.Utilities
{
    /// <summary>
    /// A helper class to make singleton creation trivial.
    /// Simply inherit from this class giving the inheriting own type as generic parameter.
    /// The first object created of the inheriting type will be the singleton object accessible through the instance property on the inheriting type.
    /// An exception is thrown when a second instance of the type is created to enforce the singleton pattern.
    /// </summary>
    /// <typeparam name="TSelf">The inheriting type.</typeparam>
    public abstract class Singleton<TSelf> where TSelf : Singleton<TSelf>
    {
        // Singletons smell of bad design. But while we are using them, might as well automate them.

        /// <summary>
        /// Gets the singleton instance.
        /// </summary>
        public static TSelf Instance { get; private set; }

        /// <summary>
        /// Creates a new instance of the inheriting type which will act as singleton.
        /// Calling this more than once will throw an exception.
        /// </summary>
        protected Singleton()
        {
            if (Instance != null)
                throw new Exception("A singleton can only be instantiated once.");

            Instance = (TSelf)this;
        }
    }
}
