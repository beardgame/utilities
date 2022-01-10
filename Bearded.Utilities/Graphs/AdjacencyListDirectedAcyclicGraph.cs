using System;
using System.Collections.Immutable;

namespace Bearded.Utilities.Graphs;

sealed class AdjacencyListDirectedAcyclicGraph<T> : AdjacencyListDirectedGraph<T>, IDirectedAcyclicGraph<T>
    where T : IEquatable<T>
{
    internal AdjacencyListDirectedAcyclicGraph(
        ImmutableList<T> elements,
        ImmutableDictionary<T, ImmutableList<T>> directSuccessors,
        ImmutableDictionary<T, ImmutableList<T>> directPredecessors)
        : base(
            elements,
            directSuccessors,
            directPredecessors) { }
}
