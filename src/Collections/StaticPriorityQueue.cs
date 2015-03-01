using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Collections
{
    /// <summary>
    /// A priority queue that is implemented with a binary min-heap and does not support the updating of priorities.
    /// </summary>
    /// <typeparam name="TPriority"></typeparam>
    /// <typeparam name="TValue"></typeparam>
    public class StaticPriorityQueue<TPriority, TValue> where TPriority : IComparable<TPriority>
    {
        /// <summary>
        /// Array-representation of the entire heap.
        /// </summary>
        protected KeyValuePair<TPriority, TValue>[] data;

// ReSharper disable once MemberCanBeProtected.Global
        /// <summary>
        /// The amount of elements the tree contains.
        /// </summary>
        public int Count { get; protected set; }

// ReSharper disable once MemberCanBeProtected.Global
        /// <summary>
        /// Creates a new instance of a static priority queue.
        /// </summary>
        public StaticPriorityQueue()
            : this(1) { }

        /// <summary>
        /// Creates a new instance of a static priority queue with the specified initial capacity.
        /// </summary>
        /// <param name="capacity">Initial capacity of the priority queue.</param>
        public StaticPriorityQueue(int capacity)
        {
            this.data = new KeyValuePair<TPriority, TValue>[capacity];
        }

        /// <summary>
        /// Creates a new instance of a static priority queue containing the initial data.
        /// </summary>
        /// <param name="data">Initial data to fill the queue with.</param>
        public StaticPriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data)
        {
            this.data = data.ToArray();
            this.Count = this.data.Length;
            for (int i = this.data.Length / 2 - 1; i >= 0; i--)
                this.cascadeDown(i);
        }

        /// <summary>
        /// Adds a new element to the tree [O(log n)].
        /// </summary>
        /// <param name="priority">The priority of the new element.</param>
        /// <param name="value">The element itself.</param>
        public void Enqueue(TPriority priority, TValue value)
        {
            if (this.Count == this.data.Length)
                this.increaseCapacity();

            this.add(priority, value);
            this.cascadeUp(this.Count);
            this.Count++;
        }

        /// <summary>
        /// Returns the element with the lowest priority from the tree without removing it [O(1)].
        /// </summary>
        /// <returns>The element with the lowest priority.</returns>
        public KeyValuePair<TPriority, TValue> Peek()
        {
            if (this.Count == 0)
                throw new InvalidOperationException();

            return this.data[0];
        }

        /// <summary>
        /// Dequeues the element with the lowest priority from the tree [O(log n)].
        /// </summary>
        /// <returns>The element with the lowest priority.</returns>
        public KeyValuePair<TPriority, TValue> Dequeue()
        {
            if (this.Count == 0)
                throw new InvalidOperationException();

            var oldRoot = this.data[0];
            this.swap(0, this.Count - 1);
            this.reset(this.Count - 1);
            this.Count--;
            this.cascadeDown(0);

            return oldRoot;
        }

        /// <summary>
        /// Empties the priority queue.
        /// </summary>
        public virtual void Clear()
        {
            this.data = new KeyValuePair<TPriority, TValue>[this.data.Length];
            this.Count = 0;
        }

        private void increaseCapacity()
        {
            var newData = new KeyValuePair<TPriority, TValue>[2 * this.data.Length + 1];
            Array.Copy(data, newData, data.Length);
            this.data = newData;
        }

        /// <summary>
        /// Cascades the changes upwards in the tree starting from the specified index [O(log n)].
        /// </summary>
        /// <param name="i">The index at which the heap property might be violated.</param>
        protected void cascadeUp(int i)
        {
            while (i > 0)
            {
                var parent = getParent(i);

                if (data[i].Key.CompareTo(data[parent].Key) < 0)
                    this.swap(i, parent);
                else return;

                i = parent;
            }
        }

        /// <summary>
        /// Cascades the changes downwards in the tree starting from the specified index [O(log n)].
        /// </summary>
        /// <param name="i">The index at which the heap proprty might be violated.</param>
        protected void cascadeDown(int i)
        {
            while (true)
            {
                var left = getLeftChild(i);
                var right = getRightChild(i);
                var smallest = i;

                if (left < this.Count && this.data[left].Key.CompareTo(this.data[smallest].Key) < 0)
                    smallest = left;
                if (right < this.Count && this.data[right].Key.CompareTo(this.data[smallest].Key) < 0)
                    smallest = right;

                if (smallest == i)
                    return;

                this.swap(i, smallest);
                i = smallest;
            }
        }

        #region Index operations
        /// <summary>
        /// Adds a new element to the end of the tree.
        /// </summary>
        /// <param name="priority">The priority of the new element.</param>
        /// <param name="value">The element itself.</param>
        protected virtual void add(TPriority priority, TValue value)
        {
            this.data[this.Count] = new KeyValuePair<TPriority, TValue>(priority, value);
        }

        /// <summary>
        /// Swaps two elements in the tree.
        /// </summary>
        /// <param name="i1">The index of the first element.</param>
        /// <param name="i2">The index of the second element.</param>
        protected virtual void swap(int i1, int i2)
        {
            Do.Swap(ref this.data[i1], ref this.data[i2]);
        }

        /// <summary>
        /// Removes an element from the tree, resetting its value to the default state.
        /// </summary>
        /// <param name="i">The index of the element to be removed.</param>
        protected virtual void reset(int i)
        {
            this.data[i] = default(KeyValuePair<TPriority, TValue>);
        }
        #endregion

        #region Tree traversal
        private static int getParent(int i)
        {
            return (i - 1) / 2;
        }

        private static int getLeftChild(int i)
        {
            return 2 * i + 1;
        }

        private static int getRightChild(int i)
        {
            return 2 * i + 2;
        }
        #endregion
    }
}