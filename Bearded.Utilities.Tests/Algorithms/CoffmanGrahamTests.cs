using System.Linq;
using Bearded.Utilities.Algorithms;
using Bearded.Utilities.Graphs;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Algorithms;

public class CoffmanGrahamTests
{
    [Fact]
    public void Solve_EmptyInput_ReturnsEmptyOutput()
    {
        var graph = DirectedGraphBuilder<string>.EmptyGraph();
        var solver = CoffmanGraham.SolverForArbitraryGraphs(1);

        solver.Solve(graph).Should().BeEmpty();
    }

    [Fact]
    public void Solve_SingleInput_ReturnsSingleLayer()
    {
        var graph = DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("element")
            .CreateAcyclicGraphUnsafe();
        var solver = CoffmanGraham.SolverForArbitraryGraphs(1);

        var solution = solver.Solve(graph);

        solution.Should().HaveCount(1);
        solution[0].Should().Contain("element");
    }

    [Fact]
    public void Solve_NoArrows_DoesNotCreatesLayersTooLarge()
    {
        const int width = 2;
        const int numElements = 3;

        var graph = createGraphWithoutArrows(numElements);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(width);

        var solution = solver.Solve(graph);

        foreach (var layer in solution)
        {
            layer.Should().HaveCountLessOrEqualTo(width);
        }
    }

    [Fact]
    public void Solve_MoreChildrenThanMaxWidth_DoesNotCreateLayersTooLarge()
    {
        const int width = 3;
        const int numChildren = width * 3;

        var graph = createGraphWithManyChildren(numChildren);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(width);

        var solution = solver.Solve(graph);

        foreach (var layer in solution)
        {
            layer.Should().HaveCountLessOrEqualTo(width);
        }
    }

    [Fact]
    public void Solve_NoArrows_UsesTheMinimumNumberOfLayers()
    {
        const int width = 3;
        const int numElements = 18;
        const int expectedLayers = 6;

        var graph = createGraphWithoutArrows(numElements);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(width);

        var solution = solver.Solve(graph);

        solution.Should().HaveCount(expectedLayers);
    }

    [Fact]
    public void Solve_MoreChildrenThanMaxWidth_UsesTheMinimumNumberOfLayers()
    {
        const int width = 4;
        const int numChildren = 17;
        const int expectedLayers = 6; // 1 [root] + ceil(17 / 4) [children]

        var graph = createGraphWithManyChildren(numChildren);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(width);

        var solution = solver.Solve(graph);

        solution.Should().HaveCount(expectedLayers);
    }

    [Fact]
    public void Solve_NoArrows_IncludesAllElementsInLayers()
    {
        const int width = 3;
        const int numElements = 100;

        var graph = createGraphWithoutArrows(numElements);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(width);

        var solution = solver.Solve(graph);

        solution.SelectMany(s => s).Should().Contain(graph.Elements);
    }

    [Fact]
    public void Solve_MoreChildrenThanMaxWidth_IncludesAllElementsInLayers()
    {
        const int width = 3;
        const int numChildren = width * 3;

        var graph = createGraphWithManyChildren(numChildren);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(width);

        var solution = solver.Solve(graph);

        solution.SelectMany(s => s).Should().Contain(graph.Elements);
    }

    [Fact]
    public void Solve_PutsChildrenInSeparateLayers()
    {
        const int numElements = 10;

        var graph = createLine(numElements);
        var solver = CoffmanGraham.SolverForArbitraryGraphs(100);

        var solution = solver.Solve(graph);

        for (var i = 0; i < solution.Count; i++)
        {
            solution[i].Should().ContainSingle().Which.Should().Be(i);
        }
    }

    private static IDirectedAcyclicGraph<int> createGraphWithoutArrows(int numElements)
    {
        var builder = DirectedGraphBuilder<int>.NewBuilder();

        for (var i = 0; i < numElements; i++)
        {
            builder.AddElement(i);
        }

        var graph = builder.CreateAcyclicGraphUnsafe();
        return graph;
    }

    private static IDirectedAcyclicGraph<int> createGraphWithManyChildren(int numDirectSuccessors)
    {
        var builder = DirectedGraphBuilder<int>.NewBuilder().AddElement(-1);

        for (var i = 0; i < numDirectSuccessors; i++)
        {
            builder.AddElement(i).AddArrow(-1, i);
        }

        var graph = builder.CreateAcyclicGraphUnsafe();
        return graph;
    }

    private static IDirectedAcyclicGraph<int> createLine(int numElements)
    {
        var builder = DirectedGraphBuilder<int>.NewBuilder();

        for (var i = 0; i < numElements; i++)
        {
            builder.AddElement(i);
            if (i > 0) builder.AddArrow(i - 1, i);
        }

        var graph = builder.CreateAcyclicGraphUnsafe();
        return graph;
    }
}
