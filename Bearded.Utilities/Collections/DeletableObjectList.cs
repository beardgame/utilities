using System;
using System.Collections.Generic;

namespace Bearded.Utilities.Collections;

/// <summary>
/// This class represents a list that automatically removes items that have been deleted as specified by the IDeletable interface.
/// It uses a List as backing data structure.
/// The backing data structure is automatically compacted after too many deletions, and when there are no active enumerators.
/// Enumeration of the list skips all deleted items, and items can be deleted while enumerating.
/// Apart from manual removal and forced compaction all operations of this class are armourtised constant time.
/// This list is not threadsafe.
/// </summary>
public sealed class DeletableObjectList<T> : IEnumerable<T>
    where T : class, IDeletable
{
    #region Fields and Properties

    private readonly List<T> list;

    private int enumerators;
    private int count;
    private float emptyFraction => (float)(list.Count - count) / count;

    /// <summary>
    /// Gets or sets the maximum fraction of deleted objects this list can contain before it compacts the backing data structure.
    /// The lower the value, the more aggressively and more often is the list compacted.
    /// A value of 0 compacts as often as possible.
    /// </summary>
    public float MaxEmptyFraction { get; set; }


    /// <summary>
    /// Gets an approximation of the number of items in the list.
    /// The value is an inclusive upper bound to the actual number of items.
    /// </summary>
    public int ApproximateCount => count;

    #endregion

    #region Constructor

    public DeletableObjectList()
        : this(4)
    {
    }

    public DeletableObjectList(int capacity)
    {
        list = new List<T>(capacity);
        MaxEmptyFraction = 0.2f;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds an item to this deletable object list. The item cannot be null.
    /// </summary>
    /// <param name="item">The item to add.</param>
    public void Add(T item)
    {
        if (item == null)
            throw new ArgumentNullException(nameof(item));

        list.Add(item);
        count++;
    }

    /// <summary>
    /// Tries removing a given item from the list.
    /// This performs a linear search and therefor runs in O(n) time, where n the number of items in the list,
    /// assuming frequent enumeration or forced compaction.
    /// This method does not have to be called for items with Deleted set to true. Those items will be removed automatically.
    /// </summary>
    /// <param name="item">The item to remove.</param>
    /// <returns>True if the item was found and removed, false otherwise.</returns>
    public bool Remove(T item)
    {
        var i = list.IndexOf(item);
        if (i == -1)
            return false;
        ClearBackingArrayIndex(i);
        considerCompacting();
        return true;
    }

    internal void ClearBackingArrayIndex(int i)
    {
        list[i] = null;
        count--;

        if (count == 0)
            list.Clear();
    }

    /// <summary>
    /// Clears the list of all items.
    /// </summary>
    public void Clear()
    {
        list.Clear();
        count = 0;
    }

    #region Enumeration

    /// <inheritdoc />
    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();

    /// <inheritdoc />
    public IEnumerator<T> GetEnumerator() => new DeletableObjectListEnumerator<T>(this, list);

    internal void RegisterEnumerator()
    {
        enumerators++;
    }

    internal void UnregisterEnumerator()
    {
        enumerators--;
        considerCompacting();
    }

    #endregion

    #region Compaction and Trimming

    /// <summary>
    /// Force the list to compact its backing data structure for optimal enumeration performance.
    /// It further trims it to use minimal memory.
    /// This operation takes O(n) time, where n the number of items in the list,
    /// assuming frequent enumeration or forced compaction.
    /// </summary>
    public void TrimExcess()
    {
        ForceCompact();
        list.TrimExcess();
    }

    /// <summary>
    /// Force the list to compact its backing data structure for optimal enumeration performance.
    /// This operation takes O(n) time, where n the number of items in the list,
    /// assuming frequent enumeration or forced compaction.
    /// </summary>
    public void ForceCompact()
    {
        if (enumerators != 0)
        {
            throw new InvalidOperationException(
                "Cannot compact list while enumerating. " +
                "If you were not enumerating, check that your enumerators are disposed of correctly.");
        }

        compact();
    }

    private void considerCompacting()
    {
        if (enumerators == 0 &&
            count > 0 &&
            emptyFraction > MaxEmptyFraction)
        {
            compact();
        }
    }

    private void compact()
    {
        list.RemoveAll(t => t == null || t.Deleted);
        count = list.Count;
    }

    #endregion

    #endregion

}
