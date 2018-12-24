using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using Xunit;

namespace Bearded.Utilities.Tests.Collections
{
    public class PrefixTrieTests
    {
        #region construction

        [Fact]
        public void TestCount()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );
            Assert.Equal(3, trie.Count);

            trie = new PrefixTrie(
                new string[0]
                );
            Assert.Equal(0, trie.Count);
        }

        [Fact]
        public void TestIgnoreNull()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three", null }
                );
            Assert.Equal(3, trie.Count);
        }

        [Fact]
        public void TestIgnoreDouble()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three", "two" }
                );
            Assert.Equal(3, trie.Count);
        }

        #endregion

        #region Contains

        [Fact]
        public void TestContains()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );

            Assert.True(trie.Contains("one"));
            Assert.True(trie.Contains("two"));
            Assert.True(trie.Contains("three"));
            Assert.False(trie.Contains("four"));
            Assert.False(trie.Contains("threeAndSome"));
        }

        [Fact]
        public void TestContains_DoesNotContainPrefixes()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );

            Assert.False(trie.Contains(""));
            Assert.False(trie.Contains("t"));
            Assert.False(trie.Contains("tw"));
        }

        #endregion

        #region ExtendPrefix

        [Fact]
        public void TestExtendPrefix()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three", "threshing" }
                );
            Assert.Equal("one", trie.ExtendPrefix("o"));
            Assert.Equal("one", trie.ExtendPrefix("one"));

            Assert.Equal("t", trie.ExtendPrefix("t"));
            Assert.Equal("thre", trie.ExtendPrefix("th"));
        }

        [Fact]
        public void TestExtendPrefix_InvalidPrefix()
        {
            var trie = new PrefixTrie(
                new[] { "one", "two", "three" }
                );

            Assert.Null(trie.ExtendPrefix("a"));
            Assert.Null(trie.ExtendPrefix("twofold"));
        }

        #endregion

        #region AllKeys

        [Fact]
        public void TestAllKeys()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var withPrefixO = trie.AllKeys("o").ToList();

            Assert.Equal(1, withPrefixO.Count);
            Assert.Equal("one", withPrefixO[0]);

            var withPrefixTh = trie.AllKeys("th").ToList();

            Assert.Equal(2, withPrefixTh.Count);
            foreach (var word in words.Where(w => w.StartsWith("th")))
            {
                Assert.True(withPrefixTh.Contains(word));
            }
        }

        [Fact]
        public void TestAllKeys_EmptyPrefix()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var withPrefixEmptyString = trie.AllKeys("").ToList();

            Assert.Equal(words.Length, withPrefixEmptyString.Count);
            foreach (var word in words)
            {
                Assert.True(withPrefixEmptyString.Contains(word));
            }
        }

        [Fact]
        public void TestAllKeys_InvalidPrefix()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var withPrefixEmptyString = trie.AllKeys("twofold").ToList();

            Assert.Equal(0, withPrefixEmptyString.Count);
        }

        #endregion

        #region enumeration

        [Fact]
        public void TestGetEnumerator()
        {
            var words = new[] { "one", "two", "three", "threshing" };

            var trie = new PrefixTrie(words);

            var list = new List<string>(words.Length);

            using (var enumerator = trie.GetEnumerator())
            {
                for (int i = 0; i < words.Length; i++)
                {
                    Assert.True(enumerator.MoveNext());
                    list.Add(enumerator.Current);
                }
                
                Assert.False(enumerator.MoveNext());
            }

            foreach (var word in words)
            {
                Assert.True(list.Contains(word));
            }
        }

        #endregion
    }
}
