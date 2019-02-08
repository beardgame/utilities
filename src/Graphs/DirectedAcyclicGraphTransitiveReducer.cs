using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Graphs
{
    public static class DirectedAcyclicGraphTransitiveReducer<T> where T : IEquatable<T>
    {
        public static IDirectedAcyclicGraph<T> ReduceGraph(IDirectedAcyclicGraph<T> graph)
        {
            var elements = graph.Elements as IList<T> ?? graph.Elements.ToList();
            var graphBuilder = DirectedGraphBuilder<T>.NewBuilder();

            foreach (var element in elements)
            {
                graphBuilder.AddElement(element);
            }

            foreach (var element in elements)
            {
                foreach (var neighbor in getOnlyDirectChildren(graph, element))
                {
                    graphBuilder.AddArrow(element, neighbor);
                }
            }

            return graphBuilder.CreateAcyclicGraphUnsafe();
        }

        private static IEnumerable<T> getOnlyDirectChildren(IDirectedGraph<T> graph, T element)
        {
            var children = graph.GetDirectSuccessorsOf(element) as IList<T>
                ?? graph.GetDirectSuccessorsOf(element).ToList();
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

        private static IEnumerable<T> getAllSuccessorsRecursively(IDirectedGraph<T> graph, T start)
        {
            yield return start;
            foreach (var neighbor in graph.GetDirectSuccessorsOf(start))
            {
                foreach (var descendant in getAllSuccessorsRecursively(graph, neighbor))
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
}
