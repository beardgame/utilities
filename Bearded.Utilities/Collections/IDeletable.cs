namespace Bearded.Utilities.Collections;

/// <summary>
/// An interface for objects that can be deleted, used for automatic deletion from DeletableObjectList.
/// </summary>
public interface IDeletable
{
    /// <summary>
    /// Whether the object was deleted. This will cause it to be removed from all deletable object lists it is contained in.
    /// Once this returns true, it should never return false afterwards. Otherwise integrity of lists can not be guarnateed.
    /// </summary>
    bool Deleted { get; }
}
