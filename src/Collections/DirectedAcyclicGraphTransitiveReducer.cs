using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Collections
{
    static class DirectedAcyclicGraphTransitiveReducer<T> where T : IEquatable<T>
    {
        public static DirectedAcyclicGraph<T> ReduceGraph(DirectedAcyclicGraph<T> graph)
        {
            // Prevent making collections more than once.
            var elements = graph.Elements;
            var outgoingEdges = elements.ToDictionary(e => e, graph.GetOutgoingEdgesFor);
            
            var reducedGraph = new DirectedAcyclicGraph<T>();

            foreach (var element in elements)
            {
                reducedGraph.AddElement(element);
            }

            foreach (var element in elements)
            {
                // Remove all outgoing edges that are reachable from element but not direct children.
                var children = outgoingEdges[element];
                var reducedEdges = new HashSet<T>(outgoingEdges[element]);
                foreach (var child in children)
                {
                    foreach (var descendant in traverseRecursively(outgoingEdges, child))
                    {
                        if (descendant.Equals(child)) continue;
                        reducedEdges.Remove(descendant);
                    }
                }

                foreach (var neighbor in reducedEdges)
                {
                    reducedGraph.AddEdge(element, neighbor);
                }
            }

            return reducedGraph;
        }

        private static IEnumerable<T> traverseRecursively(IReadOnlyDictionary<T, IReadOnlyCollection<T>> outgoingEdges, T start)
        {
            yield return start;
            foreach (var neighbor in outgoingEdges[start])
            foreach (var descendant in traverseRecursively(outgoingEdges, neighbor))
                yield return descendant;
        }
    }
}
