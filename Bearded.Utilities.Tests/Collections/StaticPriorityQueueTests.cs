using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Xunit;

namespace Bearded.Utilities.Tests.Collections;

public class StaticPriorityQueueTests
{
    [Fact]
    public void TestEnqueueing()
    {
        var q = new StaticPriorityQueue<double, string>();
        q.Enqueue(2, "first item");
        q.Enqueue(3, "second item");
        q.Enqueue(1, "third item");
        Assert.Equal(3, q.Count);
    }

    [Fact]
    public void TestDequeueing()
    {
        var q = new StaticPriorityQueue<double, string>();
        q.Enqueue(2, "first item");
        q.Enqueue(3, "second item");
        q.Enqueue(1, "third item");

        var peek = q.Peek();
        Assert.Equal("third item", peek.Value);
        Assert.Equal(3, q.Count);

        var deq = q.Dequeue();
        Assert.Equal("third item", deq.Value);
        Assert.Equal(2, q.Count);

        deq = q.Dequeue();
        Assert.Equal("first item", deq.Value);
        Assert.Equal(1, q.Count);
    }

    [Fact]
    public void TestDequeueOnEmptyQueue()
    {
        Assert.Throws<InvalidOperationException>(() => new StaticPriorityQueue<double, string>().Dequeue());

    }

    [Fact]
    public void TestPeekOnEmptyQueue()
    {
        Assert.Throws<InvalidOperationException>(() => new StaticPriorityQueue<double, string>().Peek());
    }

    [Fact]
    public void TestGrowing()
    {
        var q = new StaticPriorityQueue<double, string>();
        for (int i = 0; i < 32; i++)
            q.Enqueue(i, i.ToString());
        Assert.Equal(32, q.Count);
        for (int i = 0; i < 32; i++)
        {
            var deq = q.Dequeue();
            Assert.Equal(i.ToString(), deq.Value);
        }
    }

    [Fact]
    public void TestInitialData()
    {
        var data = new[] { 5, 9, 2, 13, 1, 4, 7, 11, 2 };
        var q = new StaticPriorityQueue<int, int>(data.Select(i => new KeyValuePair<int, int>(i, i)));
        Assert.Equal(data.Length, q.Count);

        Array.Sort(data);
        for (int i = 0; i < data.Length; i++)
        {
            var deq = q.Dequeue();
            Assert.Equal(data[i], deq.Value);
        }
    }
}
