using System;
using Bearded.Utilities.Graphs;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Graphs
{
    public class DirectedGraphBuilderTests
    {
        [Fact]
        public void NewBuilder_ReturnsEmptyBuilder()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder().CreateGraph();

            graph.Count.Should().Be(0);
            graph.Elements.Should().BeEmpty();
        }
        
        [Fact]
        public void FromExistingGraph_CopiesElements()
        {
            var original = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("element 1")
                .AddElement("element 2")
                .CreateGraph();

            var copyOfGraph = DirectedGraphBuilder<string>.FromExistingGraph(original).CreateGraph();

            copyOfGraph.Elements.Should().Contain(original.Elements);
        }
        
        [Fact]
        public void FromExistingGraph_CopiesDirectSuccessors()
        {
            var original = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("from")
                .AddElement("to")
                .AddArrow("from", "to")
                .CreateGraph();

            var copyOfGraph = DirectedGraphBuilder<string>.FromExistingGraph(original).CreateGraph();

            copyOfGraph.GetDirectSuccessorsOf("from").Should().Contain("to");
        }

        [Fact]
        public void FromExistingGraph_CopiesDirectPredecessors()
        {
            var original = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("from")
                .AddElement("to")
                .AddArrow("from", "to")
                .CreateGraph();

            var copyOfGraph = DirectedGraphBuilder<string>.FromExistingGraph(original).CreateGraph();

            copyOfGraph.GetDirectPredecessorsOf("to").Should().Contain("from");
        }

        [Fact]
        public void EmptyGraph_ReturnsEmptyGraph()
        {
            var graph = DirectedGraphBuilder<string>.EmptyGraph();

            graph.Count.Should().Be(0);
            graph.Elements.Should().BeEmpty();
        }

        [Fact]
        public void AddElement_AddsElement()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("element")
                .CreateGraph();

            graph.Elements.Should().Contain("element");
        }

        [Fact]
        public void AddElement_SetsCorrectCount()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("element")
                .CreateGraph();

            graph.Count.Should().Be(1);
        }

        [Fact]
        public void AddElement_ShouldThrowOnNull()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder();

            builder.Invoking(b => b.AddElement(null)).Should().Throw<ArgumentNullException>();
        }
        
        [Fact]
        public void AddElement_ShouldThrowOnDuplicate()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("element");

            builder.Invoking(b => b.AddElement("element")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddElement_DoesNotAddDirectSuccessors()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("element")
                .CreateGraph();

            graph.GetDirectSuccessorsOf("element").Should().BeEmpty();
        }

        [Fact]
        public void AddElement_DoesNotAddDirectPredecessor()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("element")
                .CreateGraph();

            graph.GetDirectPredecessorsOf("element").Should().BeEmpty();
        }

        [Fact]
        public void AddArrow_AddsDirectSuccessor()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("from")
                .AddElement("to")
                .AddArrow("from", "to")
                .CreateGraph();

            graph.GetDirectSuccessorsOf("from").Should().Contain("to");
        }

        [Fact]
        public void AddArrow_AddsDirectPredecessor()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("from")
                .AddElement("to")
                .AddArrow("from", "to")
                .CreateGraph();

            graph.GetDirectPredecessorsOf("to").Should().Contain("from");
        }

        [Fact]
        public void AddArrow_ThrowsOnMissingFrom()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("from");

            builder.Invoking(b => b.AddArrow("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddArrow_ThrowsOnMissingTo()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("to");

            builder.Invoking(b => b.AddArrow("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddArrow_ThrowsOnNullFrom()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("to");

            builder.Invoking(b => b.AddArrow(null, "to")).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddArrow_ThrowsOnNullTo()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("from");

            builder.Invoking(b => b.AddArrow("from", null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddArrow_ThrowsOnDuplicate()
        {
            var builder = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("from")
                .AddElement("to")
                .AddArrow("from", "to");

            builder.Invoking(b => b.AddArrow("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddArrow_AddsSelfEdges()
        {
            var graph = DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("from_and_to")
                .AddArrow("from_and_to", "from_and_to")
                .CreateGraph();

            graph.GetDirectSuccessorsOf("from_and_to").Should().Contain("from_and_to");
            graph.GetDirectPredecessorsOf("from_and_to").Should().Contain("from_and_to");
        }

        [Fact]
        public void CreateGraph_DoesNotThrowOnCycle()
        {
            var builder = createBuilderForTriangle();
            
            builder.Invoking(b => b.CreateGraph()).Should().NotThrow();
        }

        [Fact]
        public void CreateGraph_DoesNotThrowOnSelfEdge()
        {
            var builder = createBuilderForSelfEdge();
            
            builder.Invoking(b => b.CreateGraph()).Should().NotThrow();
        }

        [Fact]
        public void CreateAcyclicGraph_ThrowsOnCycle()
        {
            var builder = createBuilderForTriangle();

            builder.Invoking(b => b.CreateAcyclicGraph()).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CreateAcyclicGraph_ThrowsOnSelfEdge()
        {
            var builder = createBuilderForSelfEdge();

            builder.Invoking(b => b.CreateAcyclicGraph()).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void CreateAcyclicGraphUnsafe_DoesNotThrowOnCycle()
        {
            var builder = createBuilderForTriangle();
            
            builder.Invoking(b => b.CreateAcyclicGraphUnsafe()).Should().NotThrow();
        }

        [Fact]
        public void CreateAcyclicGraphUnsafe_DoesNotThrowOnSelfEdge()
        {
            var builder = createBuilderForSelfEdge();
            
            builder.Invoking(b => b.CreateAcyclicGraphUnsafe()).Should().NotThrow();
        }

        private static DirectedGraphBuilder<string> createBuilderForTriangle()
        {
            return DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("A")
                .AddElement("B")
                .AddElement("C")
                .AddArrow("A", "B")
                .AddArrow("B", "C")
                .AddArrow("C", "A");
        }

        private static DirectedGraphBuilder<string> createBuilderForSelfEdge()
        {
            return DirectedGraphBuilder<string>.NewBuilder()
                .AddElement("A")
                .AddArrow("A", "A");
        }
    }
}
