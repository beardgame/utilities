using System;
using System.Collections.Immutable;

namespace Bearded.Utilities.Graphs
{
    sealed class AdjacencyListDag<T> : AdjacencyListDiGraph<T>, IDirectedAcyclicGraph<T> where T : IEquatable<T>
    {
        public AdjacencyListDag(
            ImmutableHashSet<T> elements,
            ImmutableDictionary<T, ImmutableHashSet<T>> directSuccessors,
            ImmutableDictionary<T, ImmutableHashSet<T>> directPredecessors)
            : base(
                elements,
                directSuccessors,
                directPredecessors) { }

        public IDirectedAcyclicGraph<T> GetTransitiveReduction()
            => DirectedAcyclicGraphTransitiveReducer<T>.ReduceGraph(this);
    }
}
