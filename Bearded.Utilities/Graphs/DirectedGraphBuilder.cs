using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;

namespace Bearded.Utilities.Graphs;

public sealed class DirectedGraphBuilder<T> where T : IEquatable<T>
{
    private readonly HashSet<T> elements = new HashSet<T>();
    private readonly HashSet<T> sources = new HashSet<T>();
    private readonly Dictionary<T, HashSet<T>> directSuccessors = new Dictionary<T, HashSet<T>>();
    private readonly Dictionary<T, HashSet<T>> directPredecessors = new Dictionary<T, HashSet<T>>();

    public static DirectedGraphBuilder<T> NewBuilder()
    {
        return new DirectedGraphBuilder<T>();
    }

    public static DirectedGraphBuilder<T> FromExistingGraph(IDirectedGraph<T> graph)
    {
        var builder = new DirectedGraphBuilder<T>();

        foreach (var element in graph.Elements)
        {
            builder.elements.Add(element);

            var successors = new HashSet<T>(graph.GetDirectSuccessorsOf(element));
            var predecessors = new HashSet<T>(graph.GetDirectPredecessorsOf(element));

            builder.directSuccessors.Add(element, successors);
            builder.directPredecessors.Add(element, predecessors);

            if (predecessors.Count == 0)
            {
                builder.sources.Add(element);
            }
        }

        return builder;
    }

    public static IDirectedAcyclicGraph<T> EmptyGraph()
    {
        return new DirectedGraphBuilder<T>().CreateAcyclicGraphUnsafe();
    }

    private DirectedGraphBuilder() { }

    public DirectedGraphBuilder<T> AddElement(T element)
    {
        if (element == null) throw new ArgumentNullException(nameof(element));
        if (!elements.Add(element)) throw new ArgumentException("Element already in graph.", nameof(element));

        sources.Add(element);
        directSuccessors.Add(element, new HashSet<T>());
        directPredecessors.Add(element, new HashSet<T>());

        return this;
    }

    public DirectedGraphBuilder<T> AddArrow(T from, T to)
    {
        if (from == null) throw new ArgumentNullException(nameof(from));
        if (to == null) throw new ArgumentNullException(nameof(to));
        if (!elements.Contains(from)) throw new ArgumentException("Element not in graph.", nameof(from));
        if (!elements.Contains(to)) throw new ArgumentException("Element not in graph.", nameof(to));
        if (directSuccessors[from].Contains(to)) throw new ArgumentException("Arrow already in graph.");

        directSuccessors[from].Add(to);
        directPredecessors[to].Add(from);

        sources.Remove(to);

        return this;
    }

    /// <summary>
    /// Creates an immutable directed graph instance.
    /// </summary>
    public IDirectedGraph<T> CreateGraph()
    {
        return new AdjacencyListDirectedGraph<T>(
            ImmutableList.CreateRange(elements),
            directSuccessors.ToImmutableDictionary(
                pair => pair.Key,
                pair => ImmutableList.CreateRange(pair.Value)),
            directPredecessors.ToImmutableDictionary(
                pair => pair.Key,
                pair => ImmutableList.CreateRange(pair.Value)));
    }

    /// <summary>
    /// Creates an immutable directed acyclic graph instance.
    ///
    /// <para>It will verify that no cycles exist.</para>
    /// </summary>
    public IDirectedAcyclicGraph<T> CreateAcyclicGraph()
    {
        if (isGraphCyclic())
        {
            throw new InvalidOperationException("Cannot create an directed acyclic graph with cycles.");
        }

        return CreateAcyclicGraphUnsafe();
    }

    private bool isGraphCyclic()
    {
        var visited = new HashSet<T>();
        var currentPath = new HashSet<T>();

        return elements.Any(leadsBackToCurrentPath);

        bool leadsBackToCurrentPath(T element)
        {
            if (visited.Contains(element))
            {
                return currentPath.Contains(element);
            }

            visited.Add(element);
            currentPath.Add(element);

            if (directSuccessors[element].Any(leadsBackToCurrentPath))
            {
                return true;
            }

            currentPath.Remove(element);

            return false;
        }
    }

    /// <summary>
    /// Creates an immutable directed acyclic graph instance without verifying that there are no cycles.
    /// </summary>
    public IDirectedAcyclicGraph<T> CreateAcyclicGraphUnsafe()
    {
        return new AdjacencyListDirectedAcyclicGraph<T>(
            ImmutableList.CreateRange(elements),
            directSuccessors.ToImmutableDictionary(
                pair => pair.Key,
                pair => ImmutableList.CreateRange(pair.Value)),
            directPredecessors.ToImmutableDictionary(
                pair => pair.Key,
                pair => ImmutableList.CreateRange(pair.Value)));
    }
}
