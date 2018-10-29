using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bearded.Utilities.Graphs
{
    class AdjacencyListDiGraph<T> : IDirectedGraph<T> where T : IEquatable<T>
    {
        private readonly ImmutableHashSet<T> elements;
        private readonly ImmutableDictionary<T, ImmutableHashSet<T>> directSuccessors;
        private readonly ImmutableDictionary<T, ImmutableHashSet<T>> directPredecessors;

        public IEnumerable<T> Elements => elements;
        public int Count => elements.Count;

        public AdjacencyListDiGraph(
            ImmutableHashSet<T> elements,
            ImmutableDictionary<T, ImmutableHashSet<T>> directSuccessors,
            ImmutableDictionary<T, ImmutableHashSet<T>> directPredecessors)
        {
            this.elements = elements;
            this.directSuccessors = directSuccessors;
            this.directPredecessors = directPredecessors;
        }

        public IEnumerable<T> GetDirectSuccessorsOf(T element) => directSuccessors[element];
        public IEnumerable<T> GetDirectPredecessorsOf(T element) => directPredecessors[element];
    }
}
