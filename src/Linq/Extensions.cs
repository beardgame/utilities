using System;
using System.Collections.Generic;
using System.Linq;

namespace Bearded.Utilities.Linq
{
    /// <summary>
    /// Contains useful Linq style and related extension methods.
    /// </summary>
    public static class Extensions
    {
        #region Simple

        /// <summary>
        /// Returns a sequence containing a single element.
        /// </summary>
        /// <param name="obj">The object to yield.</param>
        public static IEnumerable<T> Yield<T>(this T obj)
        {
            yield return obj;
        }

        /// <summary>
        /// Appends an item to the end of a sequence.
        /// </summary>
        /// <param name="target">The original sequence.</param>
        /// <param name="item">The item to append.</param>
        public static IEnumerable<T> Append<T>(this IEnumerable<T> target, T item)
        {
            foreach (T t in target) yield return t;
            yield return item;
        }

        /// <summary>
        /// Prepends an item to the beginning of a sequence.
        /// </summary>
        /// <param name="target">The original sequence.</param>
        /// <param name="item">The item to prepend.</param>
        public static IEnumerable<T> Prepend<T>(this IEnumerable<T> target, T item)
        {
            yield return item;
            foreach (T t in target) yield return t;
        }

        /// <summary>
        /// Filters a sequence, removing all elements that are null.
        /// </summary>
        public static IEnumerable<T> NotNull<T>(this IEnumerable<T> source)
               where T : class
        {
            return source.Where(arg => arg != null);
        }

        #endregion

        #region Aggregates

        /// <summary>
        /// Gets the element with minimum key in a sequence, as determined by a given selector function.
        /// </summary>
        /// <param name="source">The sequence.</param>
        /// <param name="selector">The function selecting the key.</param>
        public static TSource MinBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable
        {
            return source.Aggregate((a, b) => selector(a).CompareTo(selector(b)) < 0 ? a : b);
        }

        /// <summary>
        /// Gets the element with maximum key in a sequence, as determined by a given selector function.
        /// </summary>
        /// <param name="source">The sequence.</param>
        /// <param name="selector">The function selecting the key.</param>
        public static TSource MaxBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> selector)
            where TKey : IComparable
        {
            return source.Aggregate((a, b) => selector(a).CompareTo(selector(b)) > 0 ? a : b);
        }

        #endregion

        #region List

        /// <summary>
        /// Inserts an item into an already sorted list, maintaining the sort order and using the default comparer of the type.
        /// If the list already contains elements of the same sort value, the method inserts the new item at an any of the valid indices, not necessarily the first or last.
        /// If the given list is not sorted, the item will be inserted at an arbitrary index.
        /// This method is an O(log n) operation, where n is the number of elements in the list.
        /// </summary>
        /// <param name="list">The list to insert the item into.</param>
        /// <param name="item">The item to be inserted.</param>
        /// <returns>The index at which the item was inserted.</returns>
        /// <exception cref="ArgumentNullException">List is null.</exception>
        public static int AddSorted<T>( this List<T> list, T item)
            where T : IComparable<T>
        {
            return list.AddSorted(item, Comparer<T>.Default);
        }


        /// <summary>
        /// Inserts an item into an already sorted list, maintaining the sort order.
        /// If the list already contains elements of the same sort value, the method inserts the new item at an any of the valid indices, not necessarily the first or last.
        /// If the given list is not sorted, the item will be inserted at an arbitrary index.
        /// This method is an O(log n) operation, where n is the number of elements in the list.
        /// </summary>
        /// <param name="list">The list to insert the item into.</param>
        /// <param name="item">The item to be inserted.</param>
        /// <param name="comparer">Compared to use when determining item sort order.</param>
        /// <returns>The index at which the item was inserted.</returns>
        /// <exception cref="ArgumentNullException">List or comparer is null.</exception>
        public static int AddSorted<T>(this List<T> list, T item, IComparer<T> comparer)
        {
            if (list == null)
                throw new ArgumentNullException("list");
            if (comparer == null)
                throw new ArgumentNullException("comparer");

            if (list.Count == 0 || comparer.Compare(list[0], item) >= 0)
            {
                list.Insert(0, item);
                return 0;
            }

            var index = list.Count - 1;
            if (comparer.Compare(list[index], item) <= 0)
            {
                list.Add(item);
            }
            else
            {
                index = list.BinarySearch(item, comparer);
                if (index < 0)
                    index = ~index;

                list.Insert(index, item);
            }
            return index;
        }

        #endregion

        #region Dictionary

        /// <summary>
        /// Tries getting a value from a dictionary. If this succeeds further passes that value through a given function.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="key">The key to look up.</param>
        /// <param name="result">The resulting transformed value.</param>
        /// <param name="transform">The function transforming the value into the result.</param>
        /// <returns>True if the value was found, false otherwise.</returns>
        public static bool TryGetTransformedValue<TKey, TValue, TNewValue>(this Dictionary<TKey, TValue> dictionary, TKey key, out TNewValue result,
            Func<TValue, TNewValue> transform)
        {
            TValue value;
            if (dictionary.TryGetValue(key, out value))
            {
                result = transform(value);
                return true;
            }

            result = default(TNewValue);
            return false;
        }

        /// <summary>
        /// Adds a sequence of key value paris to a dictionary.
        /// </summary>
        /// <param name="dictionary">The dictionary.</param>
        /// <param name="other">KeyValuePairs to add.</param>
        public static void AddRange<TKey, TValue>(this IDictionary<TKey, TValue> dictionary,
            IEnumerable<KeyValuePair<TKey, TValue>> other)
        {
            foreach (var pair in other)
            {
                dictionary.Add(pair);
            }
        }

        public static TValue ValueOrDefault<TKey, TValue>(this Dictionary<TKey, TValue> dict, TKey key)
        {
            dict.TryGetValue(key, out var value);
            return value;
        }

        #endregion

        #region Random

        /// <summary>
        /// Selects a random element from a sequence.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="source">The sequance to choose a random element from.</param>
        /// <param name="random">An optional random object to be used. If none is given, StaticRandom is used instead.</param>
        /// <returns>A random element from the input.</returns>
        public static T RandomElement<T>(this IEnumerable<T> source, Random random = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (random == null)
                random = StaticRandom.Random;
            // optimisation for collections
            var asCollection = source as ICollection<T>;
            if (asCollection != null)
            {
                if (asCollection.Count == 0)
                    throw new InvalidOperationException("Sequence was empty.");
                return asCollection.ElementAt(random.Next(asCollection.Count));
            }

            T current = default(T);
            int count = 0;
            foreach (T element in source)
            {
                count++;
                if (random.Next(count) == 0)
                {
                    current = element;
                }
            }
            if (count == 0)
            {
                throw new InvalidOperationException("Sequence was empty.");
            }
            return current;
        }

        /// <summary>
        /// Efficiently (O(n) with n the size of the input) selects a random number of elements from an enumerable.
        /// Each element has an equal chance to be contained in the result. The order of the output is arbitrary.
        /// </summary>
        /// <typeparam name="T">Type of the elements.</typeparam>
        /// <param name="source">The enumerable that random elements are selected from.</param>
        /// <param name="count">The number of random elements to return. If this is smaller than the inputs size, the entire input is returned.</param>
        /// <param name="random">An optional random object to be used. If none is given, StaticRandom is used instead.</param>
        /// <returns>Random elements from the input.</returns>
        public static List<T> RandomSubset<T>(this IEnumerable<T> source, int count, Random random = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            if (count <= 0)
                return new List<T>();

            // optimisation for collections
            var asCollection = source as ICollection<T>;
            if (asCollection != null && count >= asCollection.Count)
                // if we take as much or more than we have, return input
                return source.ToList();

            if (random == null)
                random = StaticRandom.Random;

            var set = new List<T>(count);

            int i = 0;
            foreach (var item in source)
            {
                // copy first 'count' elements
                if (i < count)
                    set.Add(item);
                else
                {
                    // add others with decreasing likelyhood
                    int index = random.Next(i + 1);
                    if (index < count)
                        set[index] = item;
                }
                i++;
            }

            return set;
        }

        #endregion

        #region Shuffle

        /// <summary>
        /// Shuffles the list. This is a linear operation in the length of the list.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        /// <param name="random">An optional random object to be used. If none is given, StaticRandom is used instead.</param>
        public static void Shuffle<T>(this IList<T> list, Random random = null)
        {
            if (list == null)
                throw new ArgumentNullException("list");

            if (random == null)
                random = StaticRandom.Random;

            int c = list.Count;
            for (int i = 0; i < c; i++)
            {
                int j = random.Next(i, c);

                T temp = list[i];
                list[i] = list[j];
                list[j] = temp;
            }
        }

        /// <summary>
        /// Returns a new shuffled list with the elements from the given sequence.
        /// </summary>
        /// <param name="source">The sequence to shuffle.</param>
        /// <param name="random">An optional random object to be used. If none is given, StaticRandom is used instead.</param>
        public static IList<T> Shuffled<T>(this IEnumerable<T> source, Random random = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var list = source.ToList();
            list.Shuffle(random);
            return list;
        }

        /// <summary>
        /// Returns an enumerable that iterates the source in random order.
        /// Contrary to Shuffled() this method deferres shuffling until iteration.
        /// The first iteration step will take linear time, but all others are constant time.
        /// </summary>
        /// <remarks>
        /// This method is useful to include a shuffle operation in a deferred query,
        /// or when the number of randomly ordered elements needed is only made known when iterating them.
        /// For most other cases, Shuffled() is a simpler alternative.
        /// </remarks>
        /// <param name="source">The sequence to shuffle.</param>
        /// <param name="random">An optional random object to be used. If none is given, StaticRandom is used instead.</param>
        public static IEnumerable<T> ShuffledDeferred<T>(this IEnumerable<T> source, Random random = null)
        {
            if (source == null)
                throw new ArgumentNullException("source");

            var list = source.ToList();

            if (random == null)
                random = StaticRandom.Random;

            int c = list.Count;
            for (int i = 0; i < c; i++)
            {
                int j = random.Next(i, c);

                yield return list[j];

                list[j] = list[i];
            }
        }

        #endregion

    }
}
