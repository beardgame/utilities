using System.Linq;
using Bearded.Utilities.Algorithms;
using Bearded.Utilities.Graphs;
using Bearded.Utilities.Tests.Generators;
using FluentAssertions;
using FsCheck;
using FsCheck.Xunit;
using Xunit;

namespace Bearded.Utilities.Tests.Algorithms
{
    public class CoffmanGrahamTests
    {
        [Fact]
        public void Solve_EmptyInput_ReturnsEmptyOutput()
        {
            var instance = CoffmanGraham.InstanceForGraph(DirectedGraphBuilder<string>.EmptyGraph(), 1);

            instance.Solve().Should().BeEmpty();
        }

        [Fact]
        public void Solve_SingleInput_ReturnsSingleLayer()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("element")
                .CreateAcyclicGraphUnsafe();
            var instance = CoffmanGraham.InstanceForGraph(graph, 1);

            var solution = instance.Solve();

            solution.Should().HaveCount(1);
            solution[0].Should().Contain("element");
        }

        [Property(Arbitrary = new[]
            {typeof(DirectedAcyclicGraphGenerators.ApproximatelyHalfEdgesConnected<int>)})]
        public void Solve_NeverCreatesLayersTooLarge(IDirectedAcyclicGraph<int> graph, PositiveInt width)
        {
            var instance = CoffmanGraham.InstanceForGraph(graph, width.Get);

            var solution = instance.Solve();

            foreach (var layer in solution)
            {
                layer.Should().HaveCountLessOrEqualTo(width.Get);
            }
        }

        [Property(Arbitrary = new[]
            {typeof(DirectedAcyclicGraphGenerators.ApproximatelyHalfEdgesConnected<int>)})]
        public void Solve_AlwaysIncludesAllElementsInLayers(IDirectedAcyclicGraph<int> graph, PositiveInt width)
        {
            var instance = CoffmanGraham.InstanceForGraph(graph, width.Get);

            var solution = instance.Solve();

            if (graph.Count == 0)
            {
                solution.Should().HaveCount(0);
            }
            else
            {
                solution.SelectMany(s => s).Should().Contain(graph.Elements);
            }
        }
    }
}
