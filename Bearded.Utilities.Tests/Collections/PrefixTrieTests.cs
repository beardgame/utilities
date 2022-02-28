using System.Collections.Generic;
using System.Linq;
using Bearded.Utilities.Collections;
using FluentAssertions;
using Xunit;

namespace Bearded.Utilities.Tests.Collections;

public class PrefixTrieTests
{
    #region construction

    [Fact]
    public void TestCount()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three" }
        );
        trie.Count.Should().Be(3);

        trie = new PrefixTrie(
            new string[0]
        );
        trie.Count.Should().Be(0);
    }

    [Fact]
    public void TestIgnoreNull()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three", null }
        );
        trie.Should().HaveCount(3);
    }

    [Fact]
    public void TestIgnoreDouble()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three", "two" }
        );
        trie.Should().HaveCount(3);
    }

    #endregion

    #region Contains

    [Fact]
    public void TestContains()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three" }
        );

        foreach (var contained in new[] {"one", "two", "three"})
        {
            trie.Contains(contained).Should().BeTrue();
        }

        foreach (var notContained in new[] {"four", "threeAndSome"})
        {
            trie.Contains(notContained).Should().BeFalse();
        }
    }

    [Fact]
    public void TestContains_DoesNotContainPrefixes()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three" }
        );

        foreach (var notContained in new[] {"", "t", "tw"})
        {
            trie.Contains(notContained).Should().BeFalse();
        }
    }

    #endregion

    #region ExtendPrefix

    [Fact]
    public void TestExtendPrefix()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three", "threshing" }
        );

        trie.ExtendPrefix("o").Should().Be("one");
        trie.ExtendPrefix("one").Should().Be("one");

        trie.ExtendPrefix("t").Should().Be("t");
        trie.ExtendPrefix("th").Should().Be("thre");
    }

    [Fact]
    public void TestExtendPrefix_InvalidPrefix()
    {
        var trie = new PrefixTrie(
            new[] { "one", "two", "three" }
        );

        trie.ExtendPrefix("a").Should().BeNull();
        trie.ExtendPrefix("twofold").Should().BeNull();
    }

    #endregion

    #region AllKeys

    [Fact]
    public void TestAllKeys()
    {
        var words = new[] { "one", "two", "three", "threshing" };

        var trie = new PrefixTrie(words);

        var withPrefixO = trie.AllKeys("o").ToList();

        withPrefixO.Should().BeEquivalentTo(new[] { "one" });

        var withPrefixTh = trie.AllKeys("th").ToList();

        withPrefixTh.Should().BeEquivalentTo(words.Where(w => w.StartsWith("th")));
    }

    [Fact]
    public void TestAllKeys_EmptyPrefix()
    {
        var words = new[] { "one", "two", "three", "threshing" };

        var trie = new PrefixTrie(words);

        var withPrefixEmptyString = trie.AllKeys("").ToList();

        withPrefixEmptyString.Should().BeEquivalentTo(words);
    }

    [Fact]
    public void TestAllKeys_InvalidPrefix()
    {
        var words = new[] { "one", "two", "three", "threshing" };

        var trie = new PrefixTrie(words);

        var withPrefixEmptyString = trie.AllKeys("twofold").ToList();

        withPrefixEmptyString.Should().BeEmpty();
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
                enumerator.MoveNext().Should().BeTrue();
                list.Add(enumerator.Current);
            }

            enumerator.MoveNext().Should().BeFalse();
        }

        list.Should().Contain(words);
    }

    #endregion
}
