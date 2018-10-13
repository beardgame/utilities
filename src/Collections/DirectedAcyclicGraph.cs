using System;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace Bearded.Utilities.Collections
{
    /// <summary>
    /// Data structure for storing directed acyclic graphs (DAGs).
    ///
    /// <para>Note that no validation is in place to verify there are no cycles.</para>
    /// </summary>
    public sealed class DirectedAcyclicGraph<T> where T : IEquatable<T>
    {
        private readonly HashSet<T> elements = new HashSet<T>();
        private readonly HashSet<T> roots = new HashSet<T>();
        private readonly Dictionary<T, HashSet<T>> outgoingEdges = new Dictionary<T, HashSet<T>>();
        private readonly Dictionary<T, HashSet<T>> incomingEdges = new Dictionary<T, HashSet<T>>();

        public IReadOnlyCollection<T> Elements => ImmutableHashSet.CreateRange(elements);

        public int Count => elements.Count;

        public void AddElement(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (!elements.Add(element))
            {
                return;
            }
            roots.Add(element);
            outgoingEdges.Add(element, new HashSet<T>());
            incomingEdges.Add(element, new HashSet<T>());
        }

        public void AddEdge(T from, T to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (!elements.Contains(from)) throw new ArgumentException("Element not in graph.", nameof(from));
            if (!elements.Contains(to)) throw new ArgumentException("Element not in graph.", nameof(to));
            if (from.Equals(to))
            {
                throw new InvalidOperationException("Cannot make self-edges in directed acyclic graph.");
            }

            outgoingEdges[from].Add(to);
            incomingEdges[to].Add(from);

            if (roots.Contains(to))
            {
                roots.Remove(to);
            }
        }

        public bool RemoveElement(T element)
        {
            if (element == null) throw new ArgumentNullException(nameof(element));

            if (!elements.Remove(element))
            {
                return false;
            }
            roots.Remove(element);

            foreach (var to in outgoingEdges[element])
            {
                incomingEdges[to].Remove(element);
                if (incomingEdges[to].Count == 0)
                {
                    roots.Add(to);
                }
            }
            foreach (var from in incomingEdges[element])
            {
                outgoingEdges[from].Remove(element);
            }

            return true;
        }

        public bool RemoveEdge(T from, T to)
        {
            if (from == null) throw new ArgumentNullException(nameof(from));
            if (to == null) throw new ArgumentNullException(nameof(to));
            if (!elements.Contains(from)) throw new ArgumentException("Element not in graph.", nameof(from));
            if (!elements.Contains(to)) throw new ArgumentException("Element not in graph.", nameof(to));

            if (!outgoingEdges[from].Remove(to))
            {
                return false;
            }

            incomingEdges[to].Remove(from);
            if (incomingEdges[to].Count == 0)
            {
                roots.Add(to);
            }

            return true;
        }

        public IReadOnlyCollection<T> GetOutgoingEdgesFor(T element)
        {
            if (!elements.Contains(element))
            {
                throw new ArgumentException("Element does not exist in this graph.", nameof(element));
            }

            return ImmutableHashSet.CreateRange(outgoingEdges[element]);
        }
        
        public IReadOnlyCollection<T> GetIncomingEdgesFor(T element)
        {
            if (!elements.Contains(element))
            {
                throw new ArgumentException("Element does not exist in this graph.", nameof(element));
            }

            return ImmutableHashSet.CreateRange(incomingEdges[element]);
        }

        /// <summary>
        /// Returns the transitive reduction of this graph. That is, this method returns the sub-graph with a minimal
        /// number of edges such that if there exists a path from A to B in this graph, there will be one in the
        /// sub-graph.
        ///
        /// <para>
        /// Specifically, if there is an edge (A, B) and there exists a different path A -> ... -> B, the edge will be
        /// removed.
        /// </para>
        /// </summary>
        public DirectedAcyclicGraph<T> GetTransitiveReduction()
        {
            return DirectedAcyclicGraphTransitiveReducer<T>.ReduceGraph(this);
        }
    }
}
