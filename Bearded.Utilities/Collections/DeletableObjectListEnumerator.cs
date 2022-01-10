using System.Collections.Generic;

namespace Bearded.Utilities.Collections;

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

    object System.Collections.IEnumerator.Current => Current;
    public T Current { get; private set; }

    public DeletableObjectListEnumerator(DeletableObjectList<T> deletableObjectList, List<T> list)
    {
        this.deletableObjectList = deletableObjectList;
        this.list = list;

        deletableObjectList.RegisterEnumerator();
    }

    public void Dispose()
    {
        deletableObjectList.UnregisterEnumerator();
    }

    public bool MoveNext()
    {
        while (list.Count > i)
        {
            var item = list[i];
            if (item != null)
            {
                if (item.Deleted)
                {
                    deletableObjectList.ClearBackingArrayIndex(i);
                }
                else
                {
                    Current = item;
                    i++;
                    return true;
                }
            }

            i++;
        }
        return false;
    }

    public void Reset()
    {
        i = 0;
    }
}
