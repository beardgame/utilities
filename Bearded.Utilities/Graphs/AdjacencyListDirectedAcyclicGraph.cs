using System;
using System.Collections.Immutable;

namespace Bearded.Utilities.Graphs;

sealed class AdjacencyListDirectedAcyclicGraph<T> : AdjacencyListDirectedGraph<T>, IDirectedAcyclicGraph<T>
    where T : IEquatable<T>
{
    internal AdjacencyListDirectedAcyclicGraph(
        ImmutableArray<T> elements,
        ImmutableDictionary<T, ImmutableArray<T>> directSuccessors,
        ImmutableDictionary<T, ImmutableArray<T>> directPredecessors)
        : base(
            elements,
            directSuccessors,
            directPredecessors) { }
}
