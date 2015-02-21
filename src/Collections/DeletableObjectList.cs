using System.Collections.Generic;

namespace Bearded.Utilities.Collections
{
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
        private float emptyFraction { get { return (float)(this.list.Count - this.count) / this.count; } }

        /// <summary>
        /// Gets or sets the maximmum fraction of deleted objects this list can contain before it compacts the backing data structure.
        /// The lower the value, the more aggressively and more often is the list compacted.
        /// A value of 0 compacts as often as possible.
        /// </summary>
        public float MaxEmptyFraction { get; set; }


        /// <summary>
        /// Gets an approximation of the number of items in the list.
        /// The value is an inclusive upper bound to the actual number of items.
        /// </summary>
        public int ApproximateCount { get { return this.count; } }

        #endregion

        #region Constructor

        /// <summary>
        /// Initializes a new deletable object list.
        /// </summary>
        /// <param name="capacity">Initial capacity of the backing data structure.</param>
        public DeletableObjectList(int capacity = 4)
        {
            this.list = new List<T>(capacity);
            this.MaxEmptyFraction = 0.2f;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Adds an item to this deletable object list.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public void Add(T item)
        {
            this.list.Add(item);
            this.count++;
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
            var i = this.list.IndexOf(item);
            if (i == -1)
                return false;
            this.ClearBackingArrayIndex(i);
            this.considerCompacting();
            return true;
        }

        internal void ClearBackingArrayIndex(int i)
        {
            this.list[i] = null;
            this.count--;

            if(this.count == 0)
                this.list.Clear();
        }

        /// <summary>
        /// Clears the list of all items.
        /// </summary>
        public void Clear()
        {
            this.list.Clear();
            this.count = 0;
        }

        #region Enumeration

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            return new DeletableObjectListEnumerator<T>(this, this.list);
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        internal void RegisterEnumerator()
        {
            this.enumerators++;
        }
        internal void UnregisterEnumerator()
        {
            this.enumerators--;
            this.considerCompacting();
        }

        #endregion

        #region Compaction and Trimming

        private void considerCompacting()
        {
            if (this.enumerators == 0 &&
                this.count > 0 &&
                this.emptyFraction > this.MaxEmptyFraction)
            {
                this.compact();
            }
        }

        private void compact()
        {
            this.list.RemoveAll(t => t == null || t.Deleted);
            this.count = list.Count;
        }

        /// <summary>
        /// Force the list to compact its backing data structure for optimal enumeration performance.
        /// This operation takes O(n) time, where n the number of items in the list,
        /// assuming frequent enumeration or forced compaction.
        /// </summary>
        public void ForceCompact()
        {
            this.compact();
        }

        /// <summary>
        /// Force the list to compact its backing data structure for optimal enumeration performance.
        /// It further trims it to use minimal memory.
        /// This operation takes O(n) time, where n the number of items in the list,
        /// assuming frequent enumeration or forced compaction.
        /// </summary>
        public void TrimExcess()
        {
            this.compact();
            this.list.TrimExcess();
        }

        #endregion

        #endregion

    }
}
