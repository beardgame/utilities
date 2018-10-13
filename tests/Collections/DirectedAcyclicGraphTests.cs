using System;
using Bearded.Utilities.Collections;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Collections
{
    public class DirectedAcyclicGraphTests
    {
        [Fact]
        public void AddElement_AddsElement()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.AddElement("element");

            graph.Elements.Should().Contain("element");
        }

        [Fact]
        public void AddElement_IncreasesCount()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.AddElement("element");

            graph.Count.Should().Be(1);
        }

        [Fact]
        public void AddElement_ShouldThrowOnNull()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.Invoking(g => g.AddElement(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddElement_DoesNotAddOutgoingEdges()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.AddElement("element");

            graph.GetOutgoingEdgesFor("element").Should().BeEmpty();
        }

        [Fact]
        public void AddElement_DoesNotAddIncomingEdges()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.AddElement("element");

            graph.GetIncomingEdgesFor("element").Should().BeEmpty();
        }

        [Fact]
        public void AddEdge_AddsOutgoingEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");
            graph.AddElement("to");

            graph.AddEdge("from", "to");

            graph.GetOutgoingEdgesFor("from").Should().Contain("to");
        }

        [Fact]
        public void AddEdge_AddsIncomingEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");
            graph.AddElement("to");

            graph.AddEdge("from", "to");

            graph.GetIncomingEdgesFor("to").Should().Contain("from");
        }

        [Fact]
        public void AddEdge_ThrowsOnMissingFrom()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("to");

            graph.Invoking(g => g.AddEdge("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddEdge_ThrowsOnMissingTo()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");

            graph.Invoking(g => g.AddEdge("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void AddEdge_ThrowsOnNullFrom()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("to");

            graph.Invoking(g => g.AddEdge(null, "to")).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddEdge_ThrowsOnNullTo()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");

            graph.Invoking(g => g.AddEdge("from", null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void AddEdge_ThrowsOnFromAndToEqual()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");
            
            graph.Invoking(g => g.AddEdge("element", "element")).Should().Throw<InvalidOperationException>();
        }

        [Fact]
        public void RemoveElement_RemovesElement()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");

            graph.RemoveElement("element");

            graph.Elements.Should().NotContain("element");
        }
        
        [Fact]
        public void RemoveElement_ReducesCount()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");

            graph.RemoveElement("element");

            graph.Count.Should().Be(0);
        }
        
        [Fact]
        public void RemoveElement_ReturnsTrueForExisting()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");

            graph.RemoveElement("element").Should().BeTrue();
        }
        
        
        [Fact]
        public void RemoveElement_ReturnsFalseForNonExisting()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.RemoveElement("element").Should().BeFalse();
        }

        [Fact]
        public void RemoveElement_ThrowsOnNull()
        {
            var graph = new DirectedAcyclicGraph<string>();

            graph.Invoking(g => g.RemoveElement(null)).Should().Throw<ArgumentNullException>();
        }

        [Fact]
        public void RemoveElement_RemovesItAsOutgoingEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");
            graph.AddElement("incoming");
            graph.AddEdge("incoming", "element");
            
            graph.RemoveElement("element");

            graph.GetOutgoingEdgesFor("incoming").Should().NotContain("element");
        }
        
        [Fact]
        public void RemoveElement_RemovesItAsIncomingEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("element");
            graph.AddElement("outgoing");
            graph.AddEdge("element", "outgoing");
            
            graph.RemoveElement("element");

            graph.GetIncomingEdgesFor("outgoing").Should().NotContain("element");
        }

        [Fact]
        public void RemoveEdge_RemovesOutgoingEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");
            graph.AddElement("to");
            graph.AddEdge("from", "to");

            graph.RemoveEdge("from", "to");

            graph.GetOutgoingEdgesFor("from").Should().NotContain("to");
        }

        [Fact]
        public void RemoveEdge_RemovesIncomingEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");
            graph.AddElement("to");
            graph.AddEdge("from", "to");
            
            graph.RemoveEdge("from", "to");

            graph.GetIncomingEdgesFor("to").Should().NotContain("from");
        }

        [Fact]
        public void RemoveEdge_ReturnsTrueOnExisting()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");
            graph.AddElement("to");
            graph.AddEdge("from", "to");
            
            graph.RemoveEdge("from", "to").Should().BeTrue();
        }

        [Fact]
        public void RemoveEdge_ReturnsFalseOnNonExisting()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");
            graph.AddElement("to");
            
            graph.RemoveEdge("from", "to").Should().BeFalse();
        }

        [Fact]
        public void RemoveEdge_ThrowsOnFromMissing()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("to");

            graph.Invoking(g => g.RemoveEdge("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveEdge_ThrowsOnToMissing()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");

            graph.Invoking(g => g.RemoveEdge("from", "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveEdge_ThrowsOnFromNull()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("to");

            graph.Invoking(g => g.RemoveEdge(null, "to")).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void RemoveEdge_ThrowsOnToNull()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("from");

            graph.Invoking(g => g.RemoveEdge("from", null)).Should().Throw<ArgumentException>();
        }

        [Fact]
        public void GetTransitiveReduction_DoesNotRemoveElements()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("one");
            graph.AddElement("two");
            graph.AddElement("three");
            graph.AddEdge("one", "two");
            graph.AddEdge("two", "three");
            graph.AddEdge("one", "three");
            
            var reducedGraph = graph.GetTransitiveReduction();

            reducedGraph.Elements.Should().Contain(graph.Elements);
        }

        [Fact]
        public void GetTransitiveReduction_RemovesTransitiveEdge()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("one");
            graph.AddElement("two");
            graph.AddElement("three");
            graph.AddEdge("one", "two");
            graph.AddEdge("two", "three");
            graph.AddEdge("one", "three");

            var reducedGraph = graph.GetTransitiveReduction();

            reducedGraph.GetOutgoingEdgesFor("one").Should().NotContain("three");
            reducedGraph.GetIncomingEdgesFor("three").Should().NotContain("one");
        }

        [Fact]
        public void GetTransitiveReduction_KeepsRequiredEdges()
        {
            var graph = new DirectedAcyclicGraph<string>();
            graph.AddElement("one");
            graph.AddElement("two");
            graph.AddElement("three");
            graph.AddEdge("one", "two");
            graph.AddEdge("two", "three");
            graph.AddEdge("one", "three");

            var reducedGraph = graph.GetTransitiveReduction();

            reducedGraph.GetOutgoingEdgesFor("one").Should().Contain("two");
            reducedGraph.GetIncomingEdgesFor("two").Should().Contain("one");
            reducedGraph.GetOutgoingEdgesFor("two").Should().Contain("three");
            reducedGraph.GetIncomingEdgesFor("three").Should().Contain("two");
        }
    }
}
