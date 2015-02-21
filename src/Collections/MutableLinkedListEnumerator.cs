using System.Collections.Generic;

namespace Bearded.Utilities.Collections
{
    /// <summary>
    /// Enumerator for mutable linked lists.
    /// Kept internal to hide implementation.
    /// </summary>
    internal sealed class MutableLinkedListEnumerator<T> : IEnumerator<T>
        where T : class
    {

        private readonly MutableLinkedList<T> list;

        private bool initialised;
        private bool currentWasDeleted;
        private bool done;

        public MutableLinkedListEnumerator(MutableLinkedList<T> list)
        {
            this.list = list;
        }

        private LinkedListNode<MutableLinkedListEnumerator<T>> node;
        public void SetNode(LinkedListNode<MutableLinkedListEnumerator<T>> node)
        {
            if (this.node == null)
                this.node = node;
        }

        public T Current
        {
            get { return this.current.Value; }
        }

        private MutableLinkedListNode<T> current;

        public void OnObjectRemove(MutableLinkedListNode<T> obj)
        {
            if (obj != this.current)
                return;

            this.currentWasDeleted = true;
            this.current = obj.Next;
            if (this.current == null)
                this.done = true;
        }

        public void Dispose()
        {
            this.list.ForgetEnumerator(this.node);
        }

        public bool MoveNext()
        {
            if (this.done)
                return false;
            if (!this.initialised)
            {
                if (this.list.Count == 0)
                    return false;
                this.initialised = true;
                this.current = this.list.First;
            }
            else
            {
                if (this.currentWasDeleted)
                {
                    this.currentWasDeleted = false;
                    return true;
                }
                this.current = this.current.Next;
                if (this.current == null)
                {
                    this.done = true;
                    return false;
                }
            }
            return true;
        }

        public void Reset()
        {
            this.current = null;
            this.done = false;
            this.initialised = false;
            this.currentWasDeleted = false;
        }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }

    }
}
