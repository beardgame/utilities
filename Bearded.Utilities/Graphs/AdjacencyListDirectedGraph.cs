using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bearded.Utilities.Graphs;

class AdjacencyListDirectedGraph<T> : IDirectedGraph<T> where T : IEquatable<T>
{
    private readonly ImmutableArray<T> elements;
    private readonly ImmutableDictionary<T, ImmutableArray<T>> directSuccessors;
    private readonly ImmutableDictionary<T, ImmutableArray<T>> directPredecessors;

    public IEnumerable<T> Elements => elements;
    public int Count => elements.Length;

    internal AdjacencyListDirectedGraph(
        ImmutableArray<T> elements,
        ImmutableDictionary<T, ImmutableArray<T>> directSuccessors,
        ImmutableDictionary<T, ImmutableArray<T>> directPredecessors)
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
