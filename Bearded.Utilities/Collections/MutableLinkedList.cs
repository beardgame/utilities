using System;
using System.Collections.Generic;

namespace Bearded.Utilities.Collections;

/// <summary>
/// A generic linked list that can be modified while it is being enumerated.
/// Unless otherwise specified, all operations on this list, including removal by node, run in constant time.
/// This list is not threadsafe.
/// </summary>
public sealed class MutableLinkedList<T> : IEnumerable<T>
    where T : class
{
    #region Fields and Properties

    private readonly LinkedList<MutableLinkedListEnumerator<T>> enumerators =
        new LinkedList<MutableLinkedListEnumerator<T>>();

    public MutableLinkedListNode<T> First { get; private set; }

    public MutableLinkedListNode<T> Last { get; private set; }

    public int Count { get; private set; }

    #endregion

    #region Methods

    #region Add()

    /// <summary>
    /// Adds an item to the linked list. The node used to store the item is returned.
    /// </summary>
    public MutableLinkedListNode<T> Add(T item)
    {
        var node = MutableLinkedListNode.For(item);
        Add(node);
        return node;
    }

    /// <summary>
    /// Adds a linked list node to this list.
    /// If the node is already in another list, an exception is thrown.
    /// </summary>
    public void Add(MutableLinkedListNode<T> node)
    {
        if (node.List != null)
            throw new ArgumentException("Node cannot already be in a list when adding it to one.", nameof(node));

        node.List = this;

        if (Count == 0)
        {
            First = node;
        }
        else
        {
            node.Prev = Last;
            Last.Next = node;
        }

        Last = node;

        Count++;
    }

    #endregion

    #region Remove()

    /// <summary>
    /// Tries to return the first occurrence of an item from the list.
    /// Returns true if it found and removed the item or false if the item is not in the list.
    /// </summary>
    /// <remarks>This method takes O(Count) time and its use is discouraged.</remarks>
    public bool Remove(T item)
    {
        var node = First;
        while (node != null)
        {
            if (node.Value == item)
            {
                Remove(node);
                return true;
            }
            node = node.Next;
        }
        return false;
    }

    /// <summary>
    /// Removes the specified node from the list.
    /// Throws an exception if the given node is not in the list.
    /// </summary>
    public void Remove(MutableLinkedListNode<T> node)
    {
        if (node.List != this)
            throw new ArgumentException("Node must be in list to be removed.");

        foreach (var enumerator in enumerators)
            enumerator.OnObjectRemove(node);

        if (node == Last)
            Last = node.Prev;
        if (node == First)
            First = node.Next;

        Count--;

        if (node.Next != null)
        {
            node.Next.Prev = node.Prev;
        }
        if (node.Prev != null)
        {
            node.Prev.Next = node.Next;
        }
        node.Next = null;
        node.Prev = null;
        node.List = null;
    }

    #endregion

    #region Insertion

    /// <summary>
    /// Adds a given linked list node before another one already in this list.
    /// This will throw an exception if the node is already in another list, of if the node to add before is null, or not in this list.
    /// </summary>
    public void AddBefore(MutableLinkedListNode<T> newNode, MutableLinkedListNode<T> beforeThis)
    {
        if (beforeThis.List != this)
            throw new ArgumentException("The object to insert before must be in the same list.");
        Add(newNode);
        InsertBefore(newNode, beforeThis);
    }

    /// <summary>
    /// Inserts a node before another node in the list.
    /// The given nodes must both be in the list, and the node to insert must be the last one in it.
    /// Otherwise an exception is thrown.
    /// </summary>
    public void InsertBefore(MutableLinkedListNode<T> node, MutableLinkedListNode<T> beforeThis)
    {
        if (node.List != this)
            throw new ArgumentException("Object must already be in list before inserting.");
        if (beforeThis.List != this)
            throw new ArgumentException("The object to insert before must be in the same list.");
        if (node != Last)
            throw new ArgumentException("Inserted object must be last object in list.");
        if (node == beforeThis)
            throw new ArgumentException("Cannot insert object before itself.");

        Last = node.Prev;
        if (beforeThis == First)
            First = node;

        node.Prev.Next = null;
        node.Prev = beforeThis.Prev;
        beforeThis.Prev = node;
        node.Next = beforeThis;

        if (node.Prev != null)
            node.Prev.Next = node;
    }

    #endregion

    #region Enumerating

    public IEnumerator<T> GetEnumerator()
    {
        var e = new MutableLinkedListEnumerator<T>(this);
        enumerators.AddFirst(e);
        e.SetNode(enumerators.First);
        return e;
    }

    internal void ForgetEnumerator(LinkedListNode<MutableLinkedListEnumerator<T>> node)
        => enumerators.Remove(node);

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        => GetEnumerator();

    #endregion

    #endregion

}
