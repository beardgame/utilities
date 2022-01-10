using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Collections;

/// <summary>
/// A priority queue that is implemented with a binary min-heap.
/// </summary>
/// <typeparam name="TPriority"></typeparam>
/// <typeparam name="TValue"></typeparam>
public sealed class PriorityQueue<TPriority, TValue> : StaticPriorityQueue<TPriority, TValue> where TPriority : IComparable<TPriority>
{
    private readonly Dictionary<TValue, int> valueDict = new Dictionary<TValue, int>();

    /// <summary>
    /// Creates a new instance of a priority queue.
    /// </summary>
    public PriorityQueue() { }

    /// <summary>
    /// Creates a new instance of a priority queue with the specified initial capacity.
    /// </summary>
    /// <param name="capacity">Initial capacity of the priority queue.</param>
    public PriorityQueue(int capacity)
        : base(capacity) { }

    /// <summary>
    /// Creates a new instance of a priority queue with the specified initial data.
    /// </summary>
    /// <param name="data">Initial data to fill the priority queue with.</param>
    public PriorityQueue(IEnumerable<KeyValuePair<TPriority, TValue>> data)
    {
        this.data = data.ToArray();
        Count = this.data.Length;
        for (int i = 0; i < this.data.Length; i++)
            valueDict.Add(this.data[i].Value, i);
        for (int i = this.data.Length / 2 - 1; i >= 0; i--)
            cascadeDown(i);
    }

    /// <summary>
    /// Decreases the priority of the specified element [O(log n)].
    /// </summary>
    /// <param name="value">The element that should change.</param>
    /// <param name="newPriority">The new priority of the element.</param>
    public void DecreasePriority(TValue value, TPriority newPriority)
    {
        int i = valueDict[value];
        if (data[i].Key.CompareTo(newPriority) < 0)
            throw new InvalidOperationException("Can not increase the priority.");
        data[i] = new KeyValuePair<TPriority, TValue>(newPriority, value);
        cascadeUp(i);
    }

    /// <summary>
    /// Empties the priority queue.
    /// </summary>
    public override void Clear()
    {
        base.Clear();
        valueDict.Clear();
    }

    /// <summary>
    /// Adds a new element to the end of the tree.
    /// </summary>
    /// <param name="priority">The priority of the new element.</param>
    /// <param name="value">The element itself.</param>
    protected override void add(TPriority priority, TValue value)
    {
        valueDict.Add(value, Count);
        base.add(priority, value);
    }

    /// <summary>
    /// Swaps two elements in the tree.
    /// </summary>
    /// <param name="i1">The index of the first element.</param>
    /// <param name="i2">The index of the second element.</param>
    protected override void swap(int i1, int i2)
    {
        valueDict[data[i1].Value] = i2;
        valueDict[data[i2].Value] = i1;

        base.swap(i1, i2);
    }

    /// <summary>
    /// Removes an element from the tree, resetting its value to the default state.
    /// </summary>
    /// <param name="i">The index of the element to be removed.</param>
    protected override void reset(int i)
    {
        valueDict.Remove(data[i].Value);

        base.reset(i);
    }
}
