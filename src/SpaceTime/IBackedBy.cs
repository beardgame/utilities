
namespace Bearded.Utilities.SpaceTime
{
    /// <summary>
    /// Represents a type that is backed by a specific singular numeric type value.
    /// </summary>
    /// <typeparam name="T">The type of the backing value.</typeparam>
    public interface IBackedBy<out T>
        where T : struct
    {
        /// <summary>
        /// Returns the numeric value of the type.
        /// </summary>
        T NumericValue { get; }
    }

}