using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Utilities.Graphs;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities.Algorithms
{
    /// <summary>
    /// This class contains logic to take a partially ordered set of elements and split it in a sequence of layers such
    /// that the following holds:
    ///
    /// <list type="number">
    /// <item>
    /// <description>
    /// If an element is smaller than another in the partial ordering, it will be on a lower level.
    /// </description>
    /// </item>
    /// <item>
    /// Every layer contains at most a fixed number of W elements.
    /// </item>
    /// </list>
    ///
    /// <para>
    /// The input is given as a directed acyclic graph, such that there is an arrow from x to y if x is smaller than y.
    /// </para>
    ///
    /// <para>
    /// The algorithm itself takes O(n^2), but requires an order that is transitively reduced. If the provided graph is
    /// not representing a transitively reduced ordering, the algorithm will create one. The runtime for the
    /// transitive reduction of the directed acyclic graph is not included in the runtime, and is not known to be
    /// possible in O(n^2).
    /// </para>
    /// </summary>
    public static class CoffmanGraham
    {
        public class Instance<T> where T : IEquatable<T>
        {
            private readonly IDirectedAcyclicGraph<T> graph;
            private readonly int maxLayerSize;
            private readonly bool isGraphReduced;

            internal Instance(IDirectedAcyclicGraph<T> graph, int maxLayerSize, bool isGraphReduced)
            {
                this.graph = graph;
                this.maxLayerSize = maxLayerSize;
                this.isGraphReduced = isGraphReduced;
            }

            public ImmutableList<ImmutableHashSet<T>> Solve()
            {
                if (graph.Count == 0) return ImmutableList<ImmutableHashSet<T>>.Empty;

                var reducedGraph = isGraphReduced ? graph : DirectedAcyclicGraphTransitiveReducer<T>.ReduceGraph(graph);
                var ordering = createTopologicalOrdering(reducedGraph);
                return createLayers(reducedGraph, ordering, maxLayerSize);
            }

            // ReSharper disable once SuggestBaseTypeForParameter
            private static IList<T> createTopologicalOrdering(IDirectedAcyclicGraph<T> graph)
            {
                var ordering = new List<T>(graph.Count);

                var elements = new HashSet<T>(graph.Elements);
                var orderedElementIndices = new Dictionary<T, int>();

                while (ordering.Count < graph.Count)
                {
                    // All the remaining elements which have all their predecessors added to the topological ordering
                    // already. This includes elements which have no predecessors at all. This is equivalent to the set
                    // of sources in a directed acyclic graph where all already elements and adjacent arrows have been
                    // removed.
                    var sources = elements.Where(allPredecessorsHaveBeenOrdered);

                    // For each considered element, we look at the place of all their predecessors in the partial
                    // topological ordering we have constructed so far.
                    // * We first consider the most recently added predecessor of each element. That is, the predecessor
                    //   of said element that is the highest in the current partial topological ordering.
                    // * Of all these predecessors, we pick the predecessor that comes earliest in the ordering. If
                    //   there is only one element with said predecessor, that element is added to the ordering next.
                    // * Ties are broken by looking at the next highest ordered predecessor of all tied elements.
                    // * If all predecessors of 2 or more elements are the same, we pick the element with the least
                    //   predecessors.
                    // This selection process is implemented by representing the predecessors of an element as a
                    // decreasing number sequence of the predecessors' indices, and selecting the lowest of those in a
                    // reflected lexicographic ordering.
                    var next = sources.MinBy(createDecreasingNumberSequenceOfPredecessorIndices);

                    orderedElementIndices.Add(next, ordering.Count);
                    ordering.Add(next);
                    elements.Remove(next);
                }

                return ordering;

                bool allPredecessorsHaveBeenOrdered(T e) => graph.GetDirectPredecessorsOf(e)
                    .All(n => orderedElementIndices.ContainsKey(n));

                DecreasingNumberSequence createDecreasingNumberSequenceOfPredecessorIndices(T e)
                {
                    var predecessorIndices = graph.GetDirectPredecessorsOf(e).Select(n => orderedElementIndices[n]);
                    return DecreasingNumberSequence.FromUnsortedNumbers(predecessorIndices);
                }
            }

            private static ImmutableList<ImmutableHashSet<T>> createLayers(
                // ReSharper disable once SuggestBaseTypeForParameter
                IDirectedAcyclicGraph<T> graph, IList<T> ordering, int maxLayerSize)
            {
                var layersReversed = new List<ImmutableHashSet<T>.Builder>();
                var elementToLayer = new Dictionary<T, int>();

                for (var i = ordering.Count - 1; i >= 0; i--)
                {
                    // We fill in the layers back-to-front, always choosing the highest layer for the current element,
                    // such that all its successors are in a higher layer, and the layer has less than W elements.
                    // Since we are using a list where the right-most layer is 0, we are looking for the lowest k such
                    // that (1) each successor is in a layer lower than k, and (2) layer k has less than W elements.

                    // Requirement (1)
                    var highestSuccessorLevel = graph.GetDirectSuccessorsOf(ordering[i])
                        .Select(elmt => elementToLayer[elmt] + 1)
                        .Aggregate(0, Math.Max);

                    // Requirement (2)
                    var candidateLayer = highestSuccessorLevel;
                    while (
                        candidateLayer < layersReversed.Count && layersReversed[candidateLayer].Count == maxLayerSize)
                    {
                        candidateLayer++;
                    }

                    // Expand the list as needed
                    while (candidateLayer >= layersReversed.Count)
                    {
                        layersReversed.Add(ImmutableHashSet.CreateBuilder<T>());
                    }

                    // Add the element to the layer
                    layersReversed[candidateLayer].Add(ordering[i]);
                    elementToLayer.Add(ordering[i], candidateLayer);
                }

                return ImmutableList.CreateRange(layersReversed.Select(b => b.ToImmutable()).Reverse());
            }
        }

        public static Instance<T> InstanceForGraph<T>(IDirectedAcyclicGraph<T> graph, int maxLayerSize)
            where T : IEquatable<T>
        {
            return new Instance<T>(graph, maxLayerSize, isGraphReduced: false);
        }

        public static Instance<T> InstanceForAlreadyReducedGraph<T>(IDirectedAcyclicGraph<T> graph, int maxLayerSize)
            where T : IEquatable<T>
        {
            return new Instance<T>(graph, maxLayerSize, isGraphReduced: true);
        }

        private struct DecreasingNumberSequence : IComparable<DecreasingNumberSequence>, IComparable
        {
            private readonly int[] numbers;

            private DecreasingNumberSequence(int[] numbers)
            {
                this.numbers = numbers;
            }

            public int CompareTo(object obj) => CompareTo((DecreasingNumberSequence) obj);

            public int CompareTo(DecreasingNumberSequence other)
            {
                // The comparison between decreasing number sequences is implemented as a reflected lexicographic order.
                // That is, a number sequence n_0...n_i is considered smaller than a sequence m_0...m_j if either:
                // * there exists a s >=0 such that n_k = m_k for 0 <= k < s and n_s < m_s, or
                // * n_k = m_k for 0 <= k <= min(i, j) and i < j.
                for (var i = 0; i < Math.Min(numbers.Length, other.numbers.Length); i++)
                {
                    if (numbers[i] != other.numbers[i])
                    {
                        return numbers[i].CompareTo(other.numbers[i]);
                    }
                }

                return numbers.Length - other.numbers.Length;
            }

            public static DecreasingNumberSequence FromSortedNumbers(IEnumerable<int> numbers)
                => new DecreasingNumberSequence(numbers.ToArray());

            public static DecreasingNumberSequence FromUnsortedNumbers(IEnumerable<int> numbers)
                => new DecreasingNumberSequence(numbers.OrderByDescending(a => a).ToArray());
        }
    }
}
