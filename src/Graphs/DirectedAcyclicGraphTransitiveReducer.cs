using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Graphs
{
    static class DirectedAcyclicGraphTransitiveReducer<T> where T : IEquatable<T>
    {
        public static IDirectedAcyclicGraph<T> ReduceGraph(IDirectedAcyclicGraph<T> graph)
        {
            // Prevent making collections more than once.
            var elements = graph.Elements.ToList();
            var successors = elements.ToDictionary(e => e, graph.GetDirectSuccessorsOf);
            
            var graphBuilder = DirectedGraphBuilder<T>.NewBuilder();

            foreach (var element in elements)
            {
                graphBuilder.AddElement(element);
            }

            foreach (var element in elements)
            {
                // Remove all outgoing edges that are reachable from element but not direct children.
                var children = successors[element];
                var reducedEdges = new HashSet<T>(successors[element]);
                foreach (var child in children)
                {
                    foreach (var descendant in traverseRecursively(successors, child))
                    {
                        if (descendant.Equals(child)) continue;
                        reducedEdges.Remove(descendant);
                    }
                }

                foreach (var neighbor in reducedEdges)
                {
                    graphBuilder.AddArrow(element, neighbor);
                }
            }

            return graphBuilder.CreateAcyclicGraphUnsafe();
        }

        private static IEnumerable<T> traverseRecursively(IReadOnlyDictionary<T, IEnumerable<T>> outgoingEdges, T start)
        {
            yield return start;
            foreach (var neighbor in outgoingEdges[start])
            foreach (var descendant in traverseRecursively(outgoingEdges, neighbor))
                yield return descendant;
        }
    }
}
