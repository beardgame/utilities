using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Linq;
using FluentAssertions;
using FsCheck.Xunit;
using Xunit;

namespace Bearded.Utilities.Tests.Collections;

using Bearded.Utilities.Collections;

public class PriorityQueueTests
{
    [Fact]
    public void TestDecreasePriority()
    {
        var q = new PriorityQueue<double, string>();
        q.Enqueue(2, "first item");
        q.Enqueue(3, "second item");
        q.Enqueue(1, "third item");
        Assert.Equal("third item", q.Peek().Value);
        q.DecreasePriority("first item", 0);
        Assert.Equal("first item", q.Peek().Value);
        Assert.Equal(0, q.Peek().Key);
    }

    [Fact]
    public void TestDecreasePriorityInitialData()
    {
        var data = new[] { 5, 9, 2, 13, 1, 4, 7, 11 };
        var q = new PriorityQueue<int, int>(data.Select(i => new KeyValuePair<int, int>(i, i)));
        Assert.Equal(data.Length, q.Count);
        q.DecreasePriority(5, 0);
        Assert.Equal(5, q.Dequeue().Value);
        Assert.Equal(1, q.Dequeue().Value);
        q.DecreasePriority(7, 1);
        Assert.Equal(7, q.Dequeue().Value);
    }

    [Fact]
    public void TestDecreasePriorityEmptyQueue()
    {
        var q = new PriorityQueue<int, int>();
        Assert.Throws<KeyNotFoundException>(() => q.DecreasePriority(0, 0));
    }

    [Fact]
    public void TestDecreasePriorityDequeuedElement()
    {
        var q = new PriorityQueue<double, string>();
        q.Enqueue(2, "first item");
        q.Enqueue(3, "second item");
        q.Enqueue(1, "third item");

        q.Dequeue();

        Assert.Throws<KeyNotFoundException>(() => q.DecreasePriority("third item", 0));
    }

    [Fact]
    public void TestDecreasePriorityClearedQueue()
    {
        var q = new PriorityQueue<double, string>();
        q.Enqueue(2, "first item");
        q.Enqueue(3, "second item");
        q.Enqueue(1, "third item");

        q.Clear();

        Assert.Throws<KeyNotFoundException>(() => q.DecreasePriority("third item", 0));
    }

    [Fact]
    public void TestIncreasePriority()
    {
        var q = new PriorityQueue<double, string>();
        q.Enqueue(2, "item");
        Assert.Throws<InvalidOperationException>(() => q.DecreasePriority("item", 4));
    }

    [Property]
    public void TestEnumerationContainsCorrectItems(int seed, byte itemsToEnumerate, byte otherItems)
    {
        var random = new System.Random(seed);
        var totalItems = itemsToEnumerate + otherItems;
        var q = new PriorityQueue<double, int>();
        var items = Enumerable
            .Range(0, totalItems)
            .Select(i => KeyValuePair.Create(random.NextDouble(), i))
            .Shuffled();
        foreach (var (priority, value) in items)
        {
            q.Enqueue(priority, value);
        }
        for (var i = 0; i < otherItems; i++)
        {
            q.Dequeue();
        }

        // ReSharper disable once RedundantCast to make sure we're not using a ToList() that we may add to the queue
        var enumerated = ((IEnumerable<KeyValuePair<double, int>>)q).ToList();

        enumerated.Should().HaveCount(itemsToEnumerate).And.BeSubsetOf(items);
        while (q.Count > 0)
        {
            var dequeued = q.Dequeue();
            enumerated.Should().Contain(dequeued);
        }
    }
}

