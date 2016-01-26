
namespace Bearded.Utilities.SpaceTime
{
    public interface IBackedBy<T>
        where T : struct
    {
        T NumericValue { get; }
    }

}