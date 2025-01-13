using System;
using System.Collections.Generic;

namespace Bearded.Utilities.Collections;

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

    private LinkedListNode<MutableLinkedListEnumerator<T>>? node;
    public void SetNode(LinkedListNode<MutableLinkedListEnumerator<T>> node)
    {
        this.node ??= node;
    }

    public T Current => current?.Value ?? throw new InvalidOperationException();

    private MutableLinkedListNode<T>? current;

    public void OnObjectRemove(MutableLinkedListNode<T> obj)
    {
        if (obj != current)
            return;

        currentWasDeleted = true;
        current = obj.Next;
        if (current == null)
            done = true;
    }

    public void Dispose()
    {
        if (node is null) return;

        list.ForgetEnumerator(node);
    }

    public bool MoveNext()
    {
        if (done)
            return false;
        if (!initialised)
        {
            if (list.Count == 0)
                return false;
            initialised = true;
            current = list.First;
        }
        else
        {
            if (currentWasDeleted)
            {
                currentWasDeleted = false;
                return true;
            }

            if (current?.Next == null)
            {
                done = true;
                return false;
            }

            current = current.Next;
        }
        return true;
    }

    public void Reset()
    {
        current = null;
        done = false;
        initialised = false;
        currentWasDeleted = false;
    }

    object System.Collections.IEnumerator.Current => Current;
}
