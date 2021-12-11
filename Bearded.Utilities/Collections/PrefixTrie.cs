using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities.Collections;

public sealed class PrefixTrie : ICollection<string>
{
    private struct Node
    {
        private readonly Dictionary<char, Node>? values;

        public string Key { get; }

        #region creating

        // expects and requires sorted list of unique strings
        public Node(List<string> values)
            : this(values, 0, 0, values.Count)
        {

        }

        private Node(List<string> values, int index, int iMin, int iMax)
            : this()
        {
            var s = values[iMin];

            if (s.Length == index)
            {
                Key = s;
                iMin++;
            }

            this.values = makeChildren(values, index, iMin, iMax);
        }

        private static Dictionary<char, Node>? makeChildren(List<string> values, int index, int iMin, int iMax)
        {
            if (iMin >= iMax)
                return null;

            var dict = new Dictionary<char, Node>();

            var index2 = index + 1;

            var c = values[iMin][index];
            for (var i = iMin + 1; i < iMax; i++)
            {
                var c2 = values[i][index];

                if (c2 == c)
                    continue;

                dict.Add(c, new Node(values, index2, iMin, i));
                iMin = i;
                c = c2;
            }
            dict.Add(c, new Node(values, index2, iMin, iMax));

            return dict;
        }

        #endregion

        #region accessing

        public IEnumerable<string> AllKeys
        {
            get
            {
                var stack = new Stack<Node>();
                stack.Push(this);
                do
                {
                    var n = stack.Pop();
                    if (n.Key != null)
                        yield return n.Key;
                    if (n.values == null) continue;
                    foreach (var n2 in n.values.Values)
                        stack.Push(n2);
                } while (stack.Count > 0);
            }
        }

        public string LongestPrefixAddition
        {
            get
            {
                var builder = new StringBuilder();
                var n = this;
                while (n.values?.Count == 1)
                {
                    var pair = n.values.First();
                    builder.Append(pair.Key);
                    n = pair.Value;
                }
                return builder.ToString();
            }
        }

        public Node? this[char character]
        {
            get
            {
                if (values == null)
                    return null;
                if (values.TryGetValue(character, out var node))
                    return node;
                return null;
            }
        }

        #endregion
    }

    private readonly Node root;

    public int Count { get; }

    public bool IsReadOnly => true;

    /// <summary>
    /// Initialises a new prefix trie from a sequence of strings.
    /// Duplicate and null strings will be ignored.
    /// </summary>
    /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
    public PrefixTrie(IEnumerable<string> values)
    {
        if (values == null)
            throw new ArgumentNullException(nameof(values));

        var valueList = values.NotNull().Distinct()
            .OrderBy(s => s).ToList();

        Count = valueList.Count;

        if (valueList.Count == 0)
            return;

        root = new Node(valueList);
    }

    private Node? getNode(string s)
    {
        if (Count == 0)
            return null;

        if (s == string.Empty)
            return root;

        var node = root;
        foreach (var t in s)
        {
            var next = node[t];
            if (!next.HasValue)
                return null;
            node = next.Value;
        }
        return node;
    }

    public bool Contains(string s)
    {
        if (s == null)
            throw new ArgumentNullException(nameof(s));

        return getNode(s)?.Key != null;
    }

    /// <summary>
    /// Returns all contained strings with a given prefix.
    /// </summary>
    /// <returns>Empty sequence if prefix is not contained in tree.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prefix"/> is null.</exception>
    public IEnumerable<string> AllKeys(string prefix)
    {
        if (prefix == null)
            throw new ArgumentNullException(nameof(prefix));

        return getNode(prefix)?.AllKeys ?? Enumerable.Empty<string>();
    }

    /// <summary>
    /// Returns the maximum prefix that prefixes the same set of strings are the given one.
    /// </summary>
    /// <returns>Null if prefix is not contained in tree.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="prefix"/> is null.</exception>
    public string? ExtendPrefix(string prefix)
    {
        if (prefix == null)
            throw new ArgumentNullException(nameof(prefix));

        var node = getNode(prefix);
        return node.HasValue ? prefix + node.Value.LongestPrefixAddition : null;
    }

    public IEnumerator<string> GetEnumerator()
    {
        if (Count == 0)
            yield break;

        foreach (var key in root.AllKeys)
            yield return key;
    }

    IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();

    public void CopyTo(string[] array, int arrayIndex)
    {
        if (array == null)
            throw new ArgumentNullException(nameof(array));
        if (arrayIndex < 0)
            throw new ArgumentOutOfRangeException(nameof(arrayIndex));
        if (arrayIndex + Count > array.Length)
            throw new ArgumentException("The array does not have enough available space.");

        if (Count == 0)
            return;

        var i = arrayIndex;
        foreach (var key in root.AllKeys)
        {
            array[i++] = key;
        }
    }

    /// <summary>
    /// Not implemented. Will throw <see cref="NotSupportedException"/>.
    /// </summary>
    public void Add(string item) => throw new NotSupportedException();

    /// <summary>
    /// Not implemented. Will throw <see cref="NotSupportedException"/>.
    /// </summary>
    public bool Remove(string item) => throw new NotSupportedException();

    /// <summary>
    /// Not implemented. Will throw <see cref="NotSupportedException"/>.
    /// </summary>
    public void Clear() => throw new NotSupportedException();
}
