using System.Linq;
using Bearded.Utilities.Algorithms;
using Bearded.Utilities.Graphs;
using FluentAssertions;
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
        
        [Fact]
        public void Solve_NoArrows_DoesNotLeaveLayersEmpty()
        {
            const int width = 2;
            const int numElements = 3;

            var graph = createGraphWithoutArrows(numElements);
            var instance = CoffmanGraham.InstanceForGraph(graph, width);

            var solution = instance.Solve();

            foreach (var layer in solution)
            {
                layer.Should().NotBeEmpty();
            }
        }
        
        [Fact]
        public void Solve_MoreChildrenThanMaxWidth_DoesNotLeaveLayersEmpty()
        {
            const int width = 3;
            const int numChildren = width * 3;
            
            var graph = createGraphWithManyChildren(numChildren);
            var instance = CoffmanGraham.InstanceForGraph(graph, width);

            var solution = instance.Solve();

            foreach (var layer in solution)
            {
                layer.Should().NotBeEmpty();
            }
        }

        [Fact]
        public void Solve_NoArrows_DoesNotCreatesLayersTooLarge()
        {
            const int width = 2;
            const int numElements = 3;

            var graph = createGraphWithoutArrows(numElements);
            var instance = CoffmanGraham.InstanceForGraph(graph, width);

            var solution = instance.Solve();

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
            var instance = CoffmanGraham.InstanceForGraph(graph, width);

            var solution = instance.Solve();

            foreach (var layer in solution)
            {
                layer.Should().HaveCountLessOrEqualTo(width);
            }
        }

        [Fact]
        public void Solve_NoArrows_IncludesAllElementsInLayers()
        {
            const int width = 3;
            const int numElements = 100;
            
            var graph = createGraphWithoutArrows(numElements);
            var instance = CoffmanGraham.InstanceForGraph(graph, width);

            var solution = instance.Solve();

            solution.SelectMany(s => s).Should().Contain(graph.Elements);
        }
        
        [Fact]
        public void Solve_MoreChildrenThanMaxWidth_IncludesAllElementsInLayers()
        {
            const int width = 3;
            const int numChildren = width * 3;
            
            var graph = createGraphWithManyChildren(numChildren);
            var instance = CoffmanGraham.InstanceForGraph(graph, width);

            var solution = instance.Solve();

            solution.SelectMany(s => s).Should().Contain(graph.Elements);
        }
        
        [Fact]
        public void Solve_PutsChildrenInSeparateLayers()
        {
            const int numElements = 10;
            
            var graph = createLine(numElements);
            var instance = CoffmanGraham.InstanceForGraph(graph, 100);

            var solution = instance.Solve();
            
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
}
