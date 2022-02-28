using Bearded.Utilities.Graphs;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Graphs;

public class DirectedAcyclicGraphTransitiveReducerTests
{
    [Fact]
    public void GetTransitiveReduction_DoesNotRemoveElements()
    {
        /*
         * ----------|
         * |         V
         * 1 -> 2 -> 3
         */
        var graph = DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("one")
            .AddElement("two")
            .AddElement("three")
            .AddArrow("one", "two")
            .AddArrow("two", "three")
            .AddArrow("one", "three")
            .CreateAcyclicGraphUnsafe();

        /*
         * 1 -> 2 -> 3
         */
        var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

        reducedGraph.Elements.Should().Contain(graph.Elements);
    }

    [Fact]
    public void GetTransitiveReduction_RemovesTransitiveEdge()
    {
        /*
         * ----------|
         * |         V
         * 1 -> 2 -> 3
         */
        var graph = DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("one")
            .AddElement("two")
            .AddElement("three")
            .AddArrow("one", "two")
            .AddArrow("two", "three")
            .AddArrow("one", "three")
            .CreateAcyclicGraphUnsafe();

        /*
         * 1 -> 2 -> 3
         */
        var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

        reducedGraph.GetDirectSuccessorsOf("one").Should().NotContain("three");
        reducedGraph.GetDirectPredecessorsOf("three").Should().NotContain("one");
    }

    [Fact]
    public void GetTransitiveReduction_KeepsRequiredEdges()
    {
        /*
         * 1 -> 2 -> 3
         */
        var graph = DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("one")
            .AddElement("two")
            .AddElement("three")
            .AddArrow("one", "two")
            .AddArrow("two", "three")
            .AddArrow("one", "three")
            .CreateAcyclicGraphUnsafe();

        /*
         * 1 -> 2 -> 3
         */
        var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

        reducedGraph.GetDirectSuccessorsOf("one").Should().Contain("two");
        reducedGraph.GetDirectPredecessorsOf("two").Should().Contain("one");
        reducedGraph.GetDirectSuccessorsOf("two").Should().Contain("three");
        reducedGraph.GetDirectPredecessorsOf("three").Should().Contain("two");
    }

    [Fact]
    public void GetTransitiveReduction_HandlesCasesWithMultiplePaths()
    {
        /*
         * ----------|
         * |         V
         * 1 -> 2 -> 3 -> 4
         *      |         ^
         *      ----------|
         */
        var graph = DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("one")
            .AddElement("two")
            .AddElement("three")
            .AddElement("four")
            .AddArrow("one", "two")
            .AddArrow("two", "three")
            .AddArrow("three", "four")
            .AddArrow("one", "three")
            .AddArrow("two", "four")
            .CreateAcyclicGraphUnsafe();
            
        /*
         * 1 -> 2 -> 3 -> 4
         */
        var reducedGraph = DirectedAcyclicGraphTransitiveReducer.ReduceGraph(graph);

        reducedGraph.GetDirectSuccessorsOf("one").Should().Contain("two");
        reducedGraph.GetDirectPredecessorsOf("two").Should().Contain("one");
        reducedGraph.GetDirectSuccessorsOf("two").Should().Contain("three");
        reducedGraph.GetDirectPredecessorsOf("three").Should().Contain("two");
        reducedGraph.GetDirectSuccessorsOf("three").Should().Contain("four");
        reducedGraph.GetDirectPredecessorsOf("four").Should().Contain("three");
    }
}
