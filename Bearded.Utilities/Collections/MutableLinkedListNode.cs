
namespace Bearded.Utilities.Collections;

/// <summary>
/// Static class to initialise new mutable linked list nodes.
/// </summary>
public static class MutableLinkedListNode
{
    /// <summary>
    /// Returns a new mutable linked list node for the given value.
    /// </summary>
    public static MutableLinkedListNode<T> For<T>(T value)
        where T : class
    {
        return new MutableLinkedListNode<T>(value);
    }
}

/// <summary>
/// A node for mutable linked lists.
/// </summary>
public class MutableLinkedListNode<T>
    where T : class
{

    #region Fields and Properties

    private readonly T value;

    /// <summary>
    /// The value stored in the node.
    /// </summary>
    public T Value { get { return value; } }

    // Next, Prev and List are internally writeable to
    // simplify addition, removal and insertion code.
    // Do not mess with them!
    internal MutableLinkedListNode<T> Next { get; set; }
    internal MutableLinkedListNode<T> Prev { get; set; }

    /// <summary>
    /// The list the node is part of.
    /// </summary>
    public MutableLinkedList<T> List { get; internal set; }

    internal bool ChangingList { get; private set; }

    #endregion

    #region Constructors

    internal MutableLinkedListNode(T value)
    {
        this.value = value;
    }

    internal MutableLinkedListNode()
    {
        value = this as T;
    }

    #endregion

    #region Methods

    /// <summary>
    /// Adds this node to a given list.
    /// </summary>
    /// <param name="list">The list to add the node to.</param>
    public void AddToList(MutableLinkedList<T> list)
    {
        list.Add(this);
    }

    /// <summary>
    /// Adds this node to a given list before another node.
    /// </summary>
    /// <param name="list">The list to add the node to.</param>
    /// <param name="beforeThis">The node to add this before.</param>
    public void AddToListBefore(MutableLinkedList<T> list, MutableLinkedListNode<T> beforeThis)
    {
        list.AddBefore(this, beforeThis);
    }

    /// <summary>
    /// Insert this node before another on.
    /// Both nodes must be in the same list, and this node must be the last on in the list.
    /// </summary>
    /// <param name="beforeThis">The node to add this before.</param>
    public void InsertBefore(MutableLinkedListNode<T> beforeThis)
    {
        List.InsertBefore(this, beforeThis);
    }

    /// <summary>
    /// Removes this node from the list it is in.
    /// </summary>
    public void RemoveFromList()
    {
        List.Remove(this);
    }

    #endregion

}
