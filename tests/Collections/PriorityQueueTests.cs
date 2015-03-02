using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bearded.Utilities.Tests.Collections
{
    [TestClass]
    public sealed class PriorityQueueTests
    {
        [TestMethod]
        public void TestDecreasePriority()
        {
            var q = new PriorityQueue<double, string>();
            q.Enqueue(2, "first item");
            q.Enqueue(3, "second item");
            q.Enqueue(1, "third item");
            Assert.AreEqual("third item", q.Peek().Value, "peek should return lowest priority item.");
            q.DecreasePriority("first item", 0);
            Assert.AreEqual("first item", q.Peek().Value, "peek should return lowest priority item.");
            Assert.AreEqual(0, q.Peek().Key, "peek should return lowest priority.");
        }

        [TestMethod]
        public void TestDecreasePriorityInitialData()
        {
            var data = new[] { 5, 9, 2, 13, 1, 4, 7, 11 };
            var q = new PriorityQueue<int, int>(data.Select(i => new KeyValuePair<int, int>(i, i)));
            Assert.AreEqual(data.Length, q.Count, "The count of the priority queue should be equal to the initial data size.");
            q.DecreasePriority(5, 0);
            Assert.AreEqual(5, q.Dequeue().Value, "dequeue should return lowest priority item.");
            Assert.AreEqual(1, q.Dequeue().Value, "dequeue should return lowest priority item.");
            q.DecreasePriority(7, 1);
            Assert.AreEqual(7, q.Dequeue().Value);
        }

        [TestMethod]
        [ExpectedException(typeof (KeyNotFoundException))]
        public void TestDecreasePriorityEmptyQueue()
        {
            var q = new PriorityQueue<int, int>();
            q.DecreasePriority(0, 0);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestDecreasePriorityDequeuedElement()
        {
            var q = new PriorityQueue<double, string>();
            q.Enqueue(2, "first item");
            q.Enqueue(3, "second item");
            q.Enqueue(1, "third item");

            q.Dequeue();

            q.DecreasePriority("third item", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(KeyNotFoundException))]
        public void TestDecreasePriorityClearedQueue()
        {
            var q = new PriorityQueue<double, string>();
            q.Enqueue(2, "first item");
            q.Enqueue(3, "second item");
            q.Enqueue(1, "third item");

            q.Clear();

            q.DecreasePriority("third item", 0);
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestIncreasePriority()
        {
            var q = new PriorityQueue<double, string>();
            q.Enqueue(2, "item");
            q.DecreasePriority("item", 4);
        }
    }
}