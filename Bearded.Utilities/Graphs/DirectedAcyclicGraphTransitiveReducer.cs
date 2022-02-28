using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Graphs;

public static class DirectedAcyclicGraphTransitiveReducer<T> where T : IEquatable<T>
{
    public static IDirectedAcyclicGraph<T> ReduceGraph(IDirectedAcyclicGraph<T> graph)
    {
        var elementsEnumerable = graph.Elements;
        var elements = elementsEnumerable as IList<T> ?? elementsEnumerable.ToList();
        var graphBuilder = DirectedGraphBuilder<T>.NewBuilder();

        foreach (var element in elements)
        {
            graphBuilder.AddElement(element);
        }

        foreach (var element in elements)
        {
            foreach (var child in getOnlyChildrenWithoutIndirectPath(graph, element))
            {
                graphBuilder.AddArrow(element, child);
            }
        }

        return graphBuilder.CreateAcyclicGraphUnsafe();
    }

    private static IEnumerable<T> getOnlyChildrenWithoutIndirectPath(IDirectedAcyclicGraph<T> graph, T element)
    {
        var childrenEnumerable = graph.GetDirectSuccessorsOf(element);
        var children = childrenEnumerable as IList<T>
            ?? childrenEnumerable.ToList();
        var reducedEdges = new HashSet<T>(children);

        foreach (var child in children)
        {
            foreach (var descendant in getAllSuccessorsRecursively(graph, child))
            {
                if (descendant.Equals(child))
                {
                    continue;
                }

                // Every arrow to a successor of our direct child is already reachable.
                reducedEdges.Remove(descendant);
            }
        }

        return reducedEdges;
    }

    // ReSharper disable once SuggestBaseTypeForParameter
    private static IEnumerable<T> getAllSuccessorsRecursively(IDirectedAcyclicGraph<T> graph, T start)
    {
        yield return start;
        foreach (var child in graph.GetDirectSuccessorsOf(start))
        {
            foreach (var descendant in getAllSuccessorsRecursively(graph, child))
            {
                yield return descendant;
            }
        }
    }
}

public static class DirectedAcyclicGraphTransitiveReducer
{
    public static IDirectedAcyclicGraph<T> ReduceGraph<T>(IDirectedAcyclicGraph<T> graph)
        where T : IEquatable<T>
        => DirectedAcyclicGraphTransitiveReducer<T>.ReduceGraph(graph);
}
