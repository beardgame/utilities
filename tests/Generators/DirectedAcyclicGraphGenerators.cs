using System;
using System.Linq;
using Bearded.Utilities.Collections;
using FsCheck;
using Random = System.Random;

namespace Bearded.Utilities.Tests.Generators
{
    static class DirectedAcyclicGraphGenerators
    {
        private static readonly Random random = new Random();

        public static class NoEdgesConnected<T> where T : IEquatable<T>
        {
            public static Arbitrary<DirectedAcyclicGraph<T>> Graphs() => Arb.From(Helpers<T>.EmptyGraphGen);
        }

        public static class HalfEdgesConnected<T> where T : IEquatable<T>
        {
            public static Arbitrary<DirectedAcyclicGraph<T>> Graphs()
            {
                var graphGen = Helpers<T>.EmptyGraphGen;
                return Arb.From(graphGen.Select(graph =>
                {
                    var elements = graph.Elements.ToList();

                    for (var i = 0; i < elements.Count; i++)
                    {
                        for (var j = i + 1; j < elements.Count; j++)
                        {
                            if (random.NextBool())
                                graph.AddEdge(elements[i], elements[j]);
                        }
                    }

                    return graph;
                }));
            }
        }

        private static class Helpers<T> where T : IEquatable<T>
        {
            private static Gen<T[]> arrayGen => Gen.ArrayOf(Arb.Generate<T>().Where(e => e != null)).Where(arr => arr != null);

            internal static Gen<DirectedAcyclicGraph<T>> EmptyGraphGen => arrayGen.Select(array =>
            {
                var graph = new DirectedAcyclicGraph<T>();
                foreach (var element in array)
                {
                    graph.AddElement(element);
                }

                return graph;
            });
        }
    }
}
