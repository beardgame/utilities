using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bearded.Utilities.Graphs;

class AdjacencyListDirectedGraph<T> : IDirectedGraph<T> where T : IEquatable<T>
{
    private readonly ImmutableList<T> elements;
    private readonly ImmutableDictionary<T, ImmutableList<T>> directSuccessors;
    private readonly ImmutableDictionary<T, ImmutableList<T>> directPredecessors;

    public IEnumerable<T> Elements => elements;
    public int Count => elements.Count;

    internal AdjacencyListDirectedGraph(
        ImmutableList<T> elements,
        ImmutableDictionary<T, ImmutableList<T>> directSuccessors,
        ImmutableDictionary<T, ImmutableList<T>> directPredecessors)
    {
        this.elements = elements;
        this.directSuccessors = directSuccessors;
        this.directPredecessors = directPredecessors;
    }

    public IEnumerable<T> GetDirectSuccessorsOf(T element) =>
        directSuccessors.TryGetValue(element, out var successor)
            ? successor
            : throw new ArgumentOutOfRangeException(nameof(element), "Element not found in graph.");
    public IEnumerable<T> GetDirectPredecessorsOf(T element) =>
        directPredecessors.TryGetValue(element, out var predecessor)
            ? predecessor
            : throw new ArgumentOutOfRangeException(nameof(element), "Element not found in graph.");
}
