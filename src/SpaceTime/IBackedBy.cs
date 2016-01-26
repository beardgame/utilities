
namespace Bearded.Utilities.SpaceTime
{
    interface IBackedBy<T>
        where T : struct
    {
        T NumericValue { get; }
    }

}