using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bearded.Utilities.Tests.Collections
{
    [TestClass]
    public sealed class StaticPriorityQueueTests
    {
        [TestMethod]
        public void TestEnqueueing()
        {
            var q = new StaticPriorityQueue<double, string>();
            q.Enqueue(2, "first item");
            q.Enqueue(3, "second item");
            q.Enqueue(1, "third item");
            Assert.AreEqual(3, q.Count, "The count of the priority queue should be equal to 3.");
        }

        [TestMethod]
        public void TestDequeueing()
        {
            var q = new StaticPriorityQueue<double, string>();
            q.Enqueue(2, "first item");
            q.Enqueue(3, "second item");
            q.Enqueue(1, "third item");

            var peek = q.Peek();
            Assert.AreEqual("third item", peek.Value, "Peek should return the lowest priority item.");
            Assert.AreEqual(3, q.Count, "The count of the priority queue should be equal to 3.");

            var deq = q.Dequeue();
            Assert.AreEqual("third item", deq.Value, "Dequeue should return the lowest priority item.");
            Assert.AreEqual(2, q.Count, "The count of the priority queue should be equal to 2.");

            deq = q.Dequeue();
            Assert.AreEqual("first item", deq.Value, "Dequeue should return the lowest priority item.");
            Assert.AreEqual(1, q.Count, "The count of the priority queue should be equal to 1.");
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestDequeueOnEmptyQueue()
        {
            new StaticPriorityQueue<double, string>().Dequeue();
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestPeekOnEmptyQueue()
        {
            new StaticPriorityQueue<double, string>().Peek();
        }

        [TestMethod]
        public void TestGrowing()
        {
            var q = new StaticPriorityQueue<double, string>();
            for (int i = 0; i < 32; i++)
                q.Enqueue(i, i.ToString());
            Assert.AreEqual(32, q.Count, "The count of the priority queue should be equal to 32.");
            for (int i = 0; i < 32; i++)
            {
                var deq = q.Dequeue();
                Assert.AreEqual(i.ToString(), deq.Value, "Problem with dequeueing item {0}", i);
            }
        }

        [TestMethod]
        public void TestInitialData()
        {
            var data = new[] { 5, 9, 2, 13, 1, 4, 7, 11, 2 };
            var q = new StaticPriorityQueue<int, int>(data.Select(i => new KeyValuePair<int, int>(i, i)));
            Assert.AreEqual(data.Length, q.Count, "The count of the priority queue should be equal to the initial data size.");

            Array.Sort(data);
            for (int i = 0; i < data.Length; i++)
            {
                var deq = q.Dequeue();
                Assert.AreEqual(data[i], deq.Value, "Dequeue should return the lowest priority item.");
            }
        }
    }
}