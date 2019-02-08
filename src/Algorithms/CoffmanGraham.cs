using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using Bearded.Utilities.Collections;
using Bearded.Utilities.Graphs;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities.Algorithms
{
    /// <summary>
    /// This class contains logic to take a partially ordered set of elements (in the form of a directed acyclic graph)
    /// and split it in a sequence of layers such that the following holds:
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
    /// The algorithm itself takes O(n^2), but requires an order that is transitively reduced. The runtime for the
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
                var elementToIndex = new Dictionary<T, int>();
                var predecessors = elements.ToImmutableDictionary(e => e, graph.GetDirectPredecessorsOf);

                while (ordering.Count < graph.Count)
                {
                    var next = elements
                        .Where(e => predecessors[e].All(n => elementToIndex.ContainsKey(n)))
                        .MinBy(e => DecreasingNumberSequence
                            .FromUnsortedNumbers(predecessors[e].Select(n => elementToIndex[n]).ToArray()));

                    elementToIndex.Add(next, ordering.Count);
                    ordering.Add(next);
                    elements.Remove(next);
                }

                return ordering;
            }

            private static ImmutableList<ImmutableHashSet<T>> createLayers(
                IDirectedAcyclicGraph<T> graph, IList<T> ordering, int maxLayerSize)
            {
                var layers = new List<HashSet<T>>();
                var elementToLayer = new Dictionary<T, int>();

                for (var i = ordering.Count - 1; i >= 0; i--)
                {
                    var outgoingEdges = graph.GetDirectSuccessorsOf(ordering[i]);
                    var lowestPossibleLevel =
                        !outgoingEdges.Any() ? 0 : outgoingEdges.Min(e => elementToLayer[e] + 1);

                    while (lowestPossibleLevel < layers.Count && layers[lowestPossibleLevel].Count == maxLayerSize)
                    {
                        lowestPossibleLevel++;
                    }
                    while (lowestPossibleLevel >= layers.Count)
                    {
                        layers.Add(new HashSet<T>());
                    }

                    layers[lowestPossibleLevel].Add(ordering[i]);
                    elementToLayer.Add(ordering[i], lowestPossibleLevel);
                }

                return ImmutableList.CreateRange(layers.Select(ImmutableHashSet.CreateRange));
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

            public int CompareTo(object obj)
            {
                switch (obj) {
                    case null:
                        return 1;
                    case DecreasingNumberSequence sequence:
                        return CompareTo(sequence);
                    default:
                        throw new ArgumentException("Not a number sequence", nameof(obj));
                }
            }

            public int CompareTo(DecreasingNumberSequence other)
            {
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
