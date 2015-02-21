using System;
using System.Collections.Generic;

namespace Bearded.Utilities.Collections
{
    /// <summary>
    /// A generic linked list that can be modified while it is being enumerated.
    /// This list is not threadsafe.
    /// </summary>
    public sealed class MutableLinkedList<T> : IEnumerable<T>
        where T : class
    {
        #region Fields and Properties

        private readonly LinkedList<MutableLinkedListEnumerator<T>> enumerators =
            new LinkedList<MutableLinkedListEnumerator<T>>();

        /// <summary>
        /// Gets the first node in the linked list. Null if empty.
        /// </summary>
        public MutableLinkedListNode<T> First { get; private set; }

        /// <summary>
        /// Gets the last node in the linked list. Null if empty.
        /// </summary>
        public MutableLinkedListNode<T> Last { get; private set; }

        /// <summary>
        /// Gets the number of elements in the list.
        /// </summary>
        public int Count { get; private set; }

        #endregion

        #region Constructor

        /// <summary>
        /// Instantiates a new instance.
        /// </summary>
        public MutableLinkedList()
        {
            this.Count = 0;
        }

        #endregion

        #region Methods

        #region Add()

        /// <summary>
        /// Adds an item to the linked list. The node used to store the item is returned.
        /// </summary>
        /// <param name="item">The item to add.</param>
        public MutableLinkedListNode<T> Add(T item)
        {
            var node = MutableLinkedListNode.For(item);
            this.Add(node);
            return node;
        }

        /// <summary>
        /// Adds a linked list node to this list.
        /// If the node is already in another list, an exception is thrown.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void Add(MutableLinkedListNode<T> node)
        {
            if (node.List != null)
                throw new Exception("Node cannot be in a list when adding it to one.");

            node.List = this;

            if (this.Count == 0)
            {
                this.First = node;
            }
            else
            {
                node.Prev = this.Last;
                this.Last.Next = node;
            }

            this.Last = node;

            this.Count++;
        }

        #endregion

        #region Remove()

        /// <summary>
        /// Tries to return the first occurrence of an item from the list.
        /// Returns true if it found and removed the item or false if the item is not in the list.
        /// </summary>
        /// <remarks>This method takes O(n) time to complete and should not be used unless necessary.</remarks>
        /// <param name="item">The item to remove.</param>
        public bool Remove(T item)
        {
            var n = this.First;
            while (n != null)
            {
                if (n.Value == item)
                {
                    this.Remove(n);
                    return true;
                }
                n = n.Next;
            }
            return false;
        }

        /// <summary>
        /// Removes the specified node from the list.
        /// Throws an exception if the given node is not in the list.
        /// </summary>
        /// <param name="node">The node to remove.</param>
        public void Remove(MutableLinkedListNode<T> node)
        {
            if (node.List == null)
                throw new Exception("Node must be in list to be removed.");

            foreach (MutableLinkedListEnumerator<T> e in this.enumerators)
                e.OnObjectRemove(node);

            if (node == this.Last)
                this.Last = node.Prev;
            if (node == this.First)
                this.First = node.Next;

            this.Count--;

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
        /// <param name="newNode">The node to add and insert.</param>
        /// <param name="beforeThis">The node to insert before.</param>
        public void AddBefore(MutableLinkedListNode<T> newNode, MutableLinkedListNode<T> beforeThis)
        {
            if (beforeThis.List != this)
                throw new Exception("The object to insert before must be in the same list.");
            this.Add(newNode);
            this.InsertBefore(newNode, beforeThis);
        }

        /// <summary>
        /// Inserts a node before another node in the list.
        /// The given nodes must both be in the list, and the node to insert must be the last one in it.
        /// Otherwise an exception is thrown.
        /// </summary>
        /// <param name="node">The node to insert.</param>
        /// <param name="beforeThis">The node to insert before.</param>
        public void InsertBefore(MutableLinkedListNode<T> node, MutableLinkedListNode<T> beforeThis)
        {
            if (node.List != this)
                throw new Exception("Object must already be in list before inserting.");
            if (beforeThis.List != this)
                throw new Exception("The object to insert before must be in the same list.");
            if (node != this.Last)
                throw new Exception("Inserted object must be last object in list.");
            if (node == beforeThis)
                throw new Exception("Cannot insert object before itself.");

            this.Last = node.Prev;
            if (beforeThis == this.First)
                this.First = node;

            node.Prev.Next = null;
            node.Prev = beforeThis.Prev;
            beforeThis.Prev = node;
            node.Next = beforeThis;

            if (node.Prev != null)
                node.Prev.Next = node;
        }

        #endregion

        #region Enumerating

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<T> GetEnumerator()
        {
            var e = new MutableLinkedListEnumerator<T>(this);
            this.enumerators.AddFirst(e);
            e.SetNode(this.enumerators.First);
            return e;
        }

        internal void ForgetEnumerator(LinkedListNode<MutableLinkedListEnumerator<T>> node)
        {
            this.enumerators.Remove(node);
        }

        /// <summary>
        /// Returns an enumerator that iterates through a collection.
        /// </summary>
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        #endregion

        #endregion

    }
}
