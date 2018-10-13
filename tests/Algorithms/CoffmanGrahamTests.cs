using System.Linq;
using Bearded.Utilities.Algorithms;
using Bearded.Utilities.Collections;
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
            var instance = CoffmanGraham.InstanceForGraph(new DirectedAcyclicGraph<string>(), 1);

            instance.Solve().Should().BeEmpty();
        }

        [Fact]
        public void Solve_SingleInput_ReturnsSingleLayer()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");
            var instance = CoffmanGraham.InstanceForGraph(graph, 1);

            var solution = instance.Solve();

            solution.Should().HaveCount(1);
            solution[0].Should().Contain("element");
        }

        [Property(Arbitrary = new[]
            {typeof(DirectedAcyclicGraphGenerators.HalfEdgesConnected<int>)})]
        public void Solve_NeverCreatesLayersTooLarge(DirectedAcyclicGraph<int> graph, PositiveInt width)
        {
            var instance = CoffmanGraham.InstanceForGraph(graph, width.Get);

            var solution = instance.Solve();

            foreach (var layer in solution)
            {
                layer.Should().HaveCountLessOrEqualTo(width.Get);
            }
        }

        [Property(Arbitrary = new[]
            {typeof(DirectedAcyclicGraphGenerators.HalfEdgesConnected<int>)})]
        public void Solve_AlwaysIncludesAllElementsInLayers(DirectedAcyclicGraph<int> graph, PositiveInt width)
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
