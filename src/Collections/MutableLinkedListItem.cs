namespace Bearded.Utilities.Collections
{

    /// <summary>
    /// This type can be used to have a class be both the value as well as the node in a linked list to prevent unneeded allocations.
    /// Usage:
    /// MyClass : MutableLinkedListItem&lt;MyClass&gt;
    /// MutableLinkedList&lt;MyClass&gt;
    /// Make sure you add this as node and not as item to the linked list to prevent creation of the node wrapper.
    /// </summary>
    /// <typeparam name="TMe">The type of the inheriting class.</typeparam>
    abstract class MutableLinkedListItem<TMe> : MutableLinkedListNode<TMe>
        where TMe : MutableLinkedListNode<TMe>
    {

    }

    // ReSharper disable once UnusedTypeParameter
    /// <summary>
    /// This type can be used to have a class be both the value as well as the node in a linked list, mixed with actual nodes.
    /// Usage:
    /// MyClass : MutableLinkedListItem&lt;MyClass, MyInterface&gt;
    /// MutableLinkedList&lt;MyInterface&gt;
    /// Make sure you add this as node and not as item to the linked list to prevent creation of the node wrapper.
    /// </summary>
    /// <typeparam name="TMe">The type of the inheriting class.</typeparam>
    /// <typeparam name="TInterface">The type of the lists interface.</typeparam>
    abstract class MutableLinkedListItem<TMe, TInterface> : MutableLinkedListNode<TInterface>
        where TInterface : class
        where TMe : TInterface
    {

    }
}
