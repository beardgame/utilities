using System;
using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Bearded.Utilities.Tests.Collections
{
    [TestClass]
    public sealed class PrefixTrieTests
    {
        #region construction

        [TestMethod]
        public void TestCount()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );
            Assert.AreEqual(3, trie.Count);

            trie = new PrefixTrie(
                new string[0]
                );
            Assert.AreEqual(0, trie.Count);
        }

        [TestMethod]
        public void TestIgnoreNull()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three", null }
                );
            Assert.AreEqual(3, trie.Count);
        }

        [TestMethod]
        public void TestIgnoreDouble()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three", "two" }
                );
            Assert.AreEqual(3, trie.Count);
        }

        #endregion

        #region Contains

        [TestMethod]
        public void TestContains()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );

            Assert.IsTrue(trie.Contains("one"));
            Assert.IsTrue(trie.Contains("two"));
            Assert.IsTrue(trie.Contains("three"));
            Assert.IsFalse(trie.Contains("four"));
            Assert.IsFalse(trie.Contains("threeAndSome"));
        }

        [TestMethod]
        public void TestContains_DoesNotContainPrefixes()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );

            Assert.IsFalse(trie.Contains(""));
            Assert.IsFalse(trie.Contains("t"));
            Assert.IsFalse(trie.Contains("tw"));
        }

        #endregion

        #region ExtendPrefix

        [TestMethod]
        public void TestExtendPrefix()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three", "threshing" }
                );
            Assert.AreEqual("one", trie.ExtendPrefix("o"));
            Assert.AreEqual("one", trie.ExtendPrefix("one"));

            Assert.AreEqual("t", trie.ExtendPrefix("t"));
            Assert.AreEqual("thre", trie.ExtendPrefix("th"));
        }

        [TestMethod]
        public void TestExtendPrefix_InvalidPrefix()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );

            Assert.IsNull(trie.ExtendPrefix("a"));
            Assert.IsNull(trie.ExtendPrefix("twofold"));
        }

        #endregion

        #region AllKeys

        [TestMethod]
        public void TestAllKeys()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var withPrefixO = trie.AllKeys("o").ToList();

            Assert.AreEqual(1, withPrefixO.Count);
            Assert.AreEqual("one", withPrefixO[0]);

            var withPrefixTh = trie.AllKeys("th").ToList();

            Assert.AreEqual(2, withPrefixTh.Count);
            foreach (var word in words.Where(w => w.StartsWith("th")))
            {
                Assert.IsTrue(withPrefixTh.Contains(word));
            }
        }

        [TestMethod]
        public void TestAllKeys_EmptyPrefix()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var withPrefixEmptyString = trie.AllKeys("").ToList();

            Assert.AreEqual(words.Length, withPrefixEmptyString.Count);
            foreach (var word in words)
            {
                Assert.IsTrue(withPrefixEmptyString.Contains(word));
            }
        }

        [TestMethod]
        public void TestAllKeys_InvalidPrefix()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var withPrefixEmptyString = trie.AllKeys("twofold").ToList();

            Assert.AreEqual(0, withPrefixEmptyString.Count);
        }

        #endregion

        #region enumeration

        [TestMethod]
        public void TestGetEnumerator()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var list = new List<string>(words.Length);

            using (var enumerator = trie.GetEnumerator())
            {
                for (int i = 0; i < words.Length; i++)
                {
                    Assert.IsTrue(enumerator.MoveNext());
                    list.Add(enumerator.Current);
                }
                
                Assert.IsFalse(enumerator.MoveNext());
            }

            foreach (var word in words)
            {
                Assert.IsTrue(list.Contains(word));
            }
        }

        #endregion
    }
}
