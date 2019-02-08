using Bearded.Utilities.Graphs;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Graphs
{
    public class DirectedAcyclicGraphTransitiveReducerTests
    {
        [Fact]
        public void GetTransitiveReduction_DoesNotRemoveElements()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("one")
                .AddElement("two")
                .AddElement("three")
                .AddArrow("one", "two")
                .AddArrow("two", "three")
                .AddArrow("one", "three")
                .CreateAcyclicGraphUnsafe();

            var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

            reducedGraph.Elements.Should().Contain(graph.Elements);
        }

        [Fact]
        public void GetTransitiveReduction_RemovesTransitiveEdge()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("one")
                .AddElement("two")
                .AddElement("three")
                .AddArrow("one", "two")
                .AddArrow("two", "three")
                .AddArrow("one", "three")
                .CreateAcyclicGraphUnsafe();

            var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

            reducedGraph.GetDirectSuccessorsOf("one").Should().NotContain("three");
            reducedGraph.GetDirectPredecessorsOf("three").Should().NotContain("one");
        }

        [Fact]
        public void GetTransitiveReduction_KeepsRequiredEdges()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("one")
                .AddElement("two")
                .AddElement("three")
                .AddArrow("one", "two")
                .AddArrow("two", "three")
                .AddArrow("one", "three")
                .CreateAcyclicGraphUnsafe();

            var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

            reducedGraph.GetDirectSuccessorsOf("one").Should().Contain("two");
            reducedGraph.GetDirectPredecessorsOf("two").Should().Contain("one");
            reducedGraph.GetDirectSuccessorsOf("two").Should().Contain("three");
            reducedGraph.GetDirectPredecessorsOf("three").Should().Contain("two");
        }
    }
}