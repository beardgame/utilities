using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Bearded.Utilities.Linq;

namespace Bearded.Utilities.Collections
{
    /// <summary>
    /// An immutable prefix trie, useful for extending prefixes and getting all strings with a given prefix.
    /// </summary>
    sealed public class PrefixTrie : ICollection<string>
    {
        private struct Node
        {
            private readonly Dictionary<char, Node> values;
            private readonly string key;

            public string Key { get { return this.key; } }

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
                    this.key = s;
                    iMin++;
                }

                this.values = makeChildren(values, index, iMin, iMax);
            }

            private static Dictionary<char, Node> makeChildren(List<string> values, int index, int iMin, int iMax)
            {
                if (iMin >= iMax)
                    return null;

                var dict = new Dictionary<char, Node>();

                int index2 = index + 1;

                char c = values[iMin][index];
                for (int i = iMin + 1; i < iMax; i++)
                {
                    var c2 = values[i][index];
                    if (c2 != c)
                    {
                        dict.Add(c, new Node(values, index2, iMin, i));
                        iMin = i;
                        c = c2;
                    }
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
                        if (n.key != null)
                            yield return n.key;
                        if (n.values != null)
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
                    while (n.key == null && n.values.Count == 1)
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
                    if (this.values == null)
                        return null;
                    Node ret;
                    if (this.values.TryGetValue(character, out ret))
                        return ret;
                    return null;
                }
            }

            #endregion
        }

        private readonly Node root;
        private readonly int count;

        /// <summary>
        /// Gets the number of elements contained in the <see cref="T:System.Collections.Generic.ICollection`1"/>.
        /// </summary>
        public int Count { get { return this.count; } }

        /// <summary>
        /// Gets a value indicating whether the <see cref="T:System.Collections.Generic.ICollection`1"/> is read-only.
        /// </summary>
        public bool IsReadOnly { get { return true; } }

        /// <summary>
        /// Initialises a new prefix trie from a sequence of strings.
        /// Duplicate and null strings and will be ignored.
        /// </summary>
        /// <param name="values">The strings to build the trie from.</param>
        /// <exception cref="ArgumentNullException"><paramref name="values"/> is null.</exception>
        public PrefixTrie(IEnumerable<string> values)
        {
            if (values == null)
                throw new ArgumentNullException("values");

            var valueList = values.NotNull().Distinct()
                .OrderBy(s => s).ToList();

            this.count = valueList.Count;

            if (valueList.Count == 0)
                return;

            this.root = new Node(valueList);
        }

        private Node? getNode(string s)
        {
            if (this.count == 0)
                return null;

            if (s == string.Empty)
                return this.root;

            var node = root;
            foreach (char t in s)
            {
                var next = node[t];
                if (!next.HasValue)
                    return null;
                node = next.Value;
            }
            return node;
        }

        /// <summary>
        /// Determines whether the prefix tree contains a specific string.
        /// </summary>
        public bool Contains(string s)
        {
            if (s == null)
                throw new ArgumentNullException("s");

            var node = this.getNode(s);
            return node.HasValue && node.Value.Key != null;
        }

        /// <summary>
        /// Returns all contained strings with a given prefix.
        /// </summary>
        /// <returns>Empty sequence if prefix is not contained in tree.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="prefix"/> is null.</exception>
        public IEnumerable<string> AllKeys(string prefix)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");

            var node = this.getNode(prefix);
            return node.HasValue ? node.Value.AllKeys : Enumerable.Empty<string>();
        }

        /// <summary>
        /// Returns the maximum prefix that prefixes the same set of strings are the given one.
        /// </summary>
        /// <returns>Null if prefix is not contained in tree.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="prefix"/> is null.</exception>
        public string ExtendPrefix(string prefix)
        {
            if (prefix == null)
                throw new ArgumentNullException("prefix");

            var node = this.getNode(prefix);
            return node.HasValue ? prefix + node.Value.LongestPrefixAddition : null;
        }

        /// <summary>
        /// Returns an enumerator that iterates through the collection.
        /// </summary>
        public IEnumerator<string> GetEnumerator()
        {
            if (this.count == 0)
                yield break;

            foreach (var key in this.root.AllKeys)
                yield return key;
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return this.GetEnumerator();
        }

        /// <summary>
        /// Copies the elements of the <see cref="T:System.Collections.Generic.ICollection`1"/> to an <see cref="T:System.Array"/>, starting at a particular <see cref="T:System.Array"/> index.
        /// </summary>
        public void CopyTo(string[] array, int arrayIndex)
        {
            if (array == null)
                throw new ArgumentNullException("array");
            if (arrayIndex < 0)
                throw new ArgumentOutOfRangeException("arrayIndex");
            if (arrayIndex + this.count > array.Length)
                throw new ArgumentException("The array does not have enough available space.");

            if (this.count == 0)
                return;

            var i = arrayIndex;
            foreach (var key in this.root.AllKeys)
            {
                array[i++] = key;
            }
        }

        /// <summary>
        /// Not implemented. Will throw <see cref="NotSupportedException"/>.
        /// </summary>
        public void Add(string item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not implemented. Will throw <see cref="NotSupportedException"/>.
        /// </summary>
        public bool Remove(string item)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Not implemented. Will throw <see cref="NotSupportedException"/>.
        /// </summary>
        public void Clear()
        {
            throw new NotSupportedException();
        }

    }
}
