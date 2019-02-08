using System;
using System.Linq;
using Bearded.Utilities.Graphs;
using FsCheck;
using Random = System.Random;

namespace Bearded.Utilities.Tests.Generators
{
    static class DirectedAcyclicGraphGenerators
    {
        private static readonly Random random = new Random();

        public static class NoEdgesConnected<T> where T : IEquatable<T>
        {
            public static Arbitrary<IDirectedAcyclicGraph<T>> Graphs() => Arb.From(Helpers<T>.EmptyGraphGen);
        }

        public static class ApproximatelyHalfEdgesConnected<T> where T : IEquatable<T>
        {
            public static Arbitrary<IDirectedAcyclicGraph<T>> Graphs()
            {
                var graphGen = Helpers<T>.EmptyGraphGen;
                return Arb.From(graphGen.Select(graph =>
                {
                    var elements = graph.Elements.ToList();
                    var builder = DirectedGraphBuilder<T>.FromExistingGraph(graph);

                    for (var i = 0; i < elements.Count; i++)
                    {
                        for (var j = i + 1; j < elements.Count; j++)
                        {
                            if (random.NextBool())
                                builder.AddArrow(elements[i], elements[j]);
                        }
                    }

                    return builder.CreateAcyclicGraphUnsafe();
                }));
            }
        }

        private static class Helpers<T> where T : IEquatable<T>
        {
            private static Gen<T[]> arrayGen =>
                Gen.ArrayOf(Arb.Generate<T>().Where(e => e != null)).Where(arr => arr != null);

            internal static Gen<IDirectedAcyclicGraph<T>> EmptyGraphGen => arrayGen.Select(array =>
            {
                var builder = DirectedGraphBuilder<T>.NewBuilder();
                foreach (var element in array)
                {
                    builder.AddElement(element);
                }

                return builder.CreateAcyclicGraphUnsafe();
            });
        }
    }
}
