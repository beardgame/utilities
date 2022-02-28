using System;
using Bearded.Utilities.Graphs;
using FluentAssertions;
using Xunit;
// ReSharper disable ConvertToLocalFunction

namespace Bearded.Utilities.Tests.Graphs;

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

        Action addNullElement = () => builder.AddElement(null);
            
        addNullElement.Should().Throw<ArgumentNullException>();
    }
        
    [Fact]
    public void AddElement_ShouldThrowOnDuplicate()
    {
        var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("element");

        Action addDuplicateElement = () => builder.AddElement("element");
            
        addDuplicateElement.Should().Throw<ArgumentException>();
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

        Action addArrowFromFromToTo = () => builder.AddArrow("from", "to");
            
        addArrowFromFromToTo.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddArrow_ThrowsOnMissingTo()
    {
        var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("to");

        Action addArrowFromFromToTo = () => builder.AddArrow("from", "to");
            
        addArrowFromFromToTo.Should().Throw<ArgumentException>();
    }

    [Fact]
    public void AddArrow_ThrowsOnNullFrom()
    {
        var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("to");

        Action addArrowFromNullToTo = () => builder.AddArrow(null, "to");
            
        addArrowFromNullToTo.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddArrow_ThrowsOnNullTo()
    {
        var builder = DirectedGraphBuilder<string>.NewBuilder().AddElement("from");

        Action addArrowFromFromToNull = () => builder.AddArrow("from", null);
            
        addArrowFromFromToNull.Should().Throw<ArgumentNullException>();
    }

    [Fact]
    public void AddArrow_ThrowsOnDuplicate()
    {
        var builder = DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("from")
            .AddElement("to")
            .AddArrow("from", "to");

        Action addDuplicateArrow = () => builder.AddArrow("from", "to");
            
        addDuplicateArrow.Should().Throw<ArgumentException>();
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
    public void CreateGraph_DoesNotThrowOnAcyclicGraph()
    {
        var builder = createBuilderForAcyclicGraph();

        Action createGraph = () => builder.CreateGraph();
            
        createGraph.Should().NotThrow();
    }

    [Fact]
    public void CreateGraph_DoesNotThrowOnCycle()
    {
        var builder = createBuilderForTriangle();

        Action createGraph = () => builder.CreateGraph();
            
        createGraph.Should().NotThrow();
    }

    [Fact]
    public void CreateGraph_DoesNotThrowOnSelfEdge()
    {
        var builder = createBuilderForSelfEdge();
            
        Action createGraph = () => builder.CreateGraph();
            
        createGraph.Should().NotThrow();
    }
        
    [Fact]
    public void CreateAcyclicGraph_DoesNotThrowOnAcyclicGraph()
    {
        var builder = createBuilderForAcyclicGraph();

        Action createAcyclicGraph = () => builder.CreateAcyclicGraph();
            
        createAcyclicGraph.Should().NotThrow();
    }

    [Fact]
    public void CreateAcyclicGraph_ThrowsOnCycle()
    {
        var builder = createBuilderForTriangle();

        Action createAcyclicGraph = () => builder.CreateAcyclicGraph();
            
        createAcyclicGraph.Should().Throw<InvalidOperationException>();
    }

    [Fact]
    public void CreateAcyclicGraph_ThrowsOnSelfEdge()
    {
        var builder = createBuilderForSelfEdge();

        Action createAcyclicGraph = () => builder.CreateAcyclicGraph();
            
        createAcyclicGraph.Should().Throw<InvalidOperationException>();
    }
        
    [Fact]
    public void CreateAcyclicGraphUnsafe_DoesNotThrowOnAcyclicGraph()
    {
        var builder = createBuilderForAcyclicGraph();

        Action createAcyclicGraphUnsafe = () => builder.CreateAcyclicGraphUnsafe();
            
        createAcyclicGraphUnsafe.Should().NotThrow();
    }

    [Fact]
    public void CreateAcyclicGraphUnsafe_DoesNotThrowOnCycle()
    {
        var builder = createBuilderForTriangle();
            
        Action createAcyclicGraphUnsafe = () => builder.CreateAcyclicGraphUnsafe();
            
        createAcyclicGraphUnsafe.Should().NotThrow();
    }

    [Fact]
    public void CreateAcyclicGraphUnsafe_DoesNotThrowOnSelfEdge()
    {
        var builder = createBuilderForSelfEdge();
            
        Action createAcyclicGraphUnsafe = () => builder.CreateAcyclicGraphUnsafe();
            
        createAcyclicGraphUnsafe.Should().NotThrow();
    }
        
    private static DirectedGraphBuilder<string> createBuilderForAcyclicGraph()
    {
        return DirectedGraphBuilder<string>.NewBuilder()
            .AddElement("A")
            .AddElement("B")
            .AddArrow("A", "B");
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
