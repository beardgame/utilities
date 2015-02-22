using System.Collections.Generic;

namespace Bearded.Utilities.Collections
{
    /// <summary>
    /// Enumerator for deletable object list.
    /// Kept internal to hide implementation.
    /// </summary>
    internal class DeletableObjectListEnumerator<T> : IEnumerator<T>
        where T : class, IDeletable
    {
        private readonly DeletableObjectList<T> deletableObjectList;
        private readonly List<T> list;
        private int i;

        public DeletableObjectListEnumerator(DeletableObjectList<T> deletableObjectList, List<T> list)
        {
            this.deletableObjectList = deletableObjectList;
            this.list = list;

            deletableObjectList.RegisterEnumerator();
        }

        public void Dispose()
        {
            this.deletableObjectList.UnregisterEnumerator();
        }

        public bool MoveNext()
        {
            while (this.list.Count > this.i)
            {
                var item = this.list[this.i];
                if (item != null)
                    if (item.Deleted)
                    {
                        this.deletableObjectList.ClearBackingArrayIndex(this.i);
                    }
                    else
                    {
                        this.Current = item;
                        this.i++;
                        return true;
                    }
                this.i++;
            }
            return false;
        }

        public void Reset()
        {
            this.i = 0;
        }

        public T Current { get; private set; }

        object System.Collections.IEnumerator.Current
        {
            get { return this.Current; }
        }
    }
}
