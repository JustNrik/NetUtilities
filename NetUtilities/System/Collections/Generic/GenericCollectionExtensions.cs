using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Collections.Generic
{
    /// <summary>
    ///     Extension methods for generic collections.
    /// </summary>
    [EditorBrowsable(EditorBrowsableState.Never)]
    public static class GenericCollectionExtensions
    {
        private static class ZeroArray<T> where T : struct
        {
            // Array with a single zeroed item.
            public static readonly T[] Value = { default };
        }

        /// <summary>
        ///     Enumerates the provided collection with an index.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the collection.
        /// </typeparam>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <returns>
        ///     A collection enumerated by index.
        /// </returns>
        public static IEnumerable<(T Element, int Index)> AsIndexed<T>(this IEnumerable<T> source)
        {
            Ensure.NotNull(source);

            return source is IList<T> list
                ? IndexingListEnumerator(list)
                : IndexingEnumerator(source);

            static IEnumerable<(T, int)> IndexingEnumerator(IEnumerable<T> source)
            {
                var count = 0;
                foreach (var item in source)
                    yield return (item, count++);
            }

            static IEnumerable<(T, int)> IndexingListEnumerator(IList<T> source)
            {
                for (var index = 0; index < source.Count; index++)
                    yield return (source[index], index);
            }
        }

        /// <summary>
        ///     Shuffles the provided collection. 
        /// </summary>
        /// <remarks>
        ///     You can optionally provide the amount of iterations. 
        ///     High values may have a negative impact on performance.
        /// </remarks>
        /// <typeparam name="T">
        ///     The type.
        /// </typeparam>
        /// <param name="source">
        ///     The source.
        /// </param>
        /// <param name="iterations">
        ///     The amount of iterations to perform. The default amount is 4.
        /// </param>
        /// <param name="random">
        ///     The random instance to be used.
        ///     The default instance will be <see cref="Random.Shared"/> if <see langword="null"/> is provided.
        /// </param>
        public static void Shuffle<T>(this IList<T> source, byte iterations = 4, Random? random = null)
        {
            Ensure.NotNull(source);

            if (source.Count < 2)
                return;

            random ??= Random.Shared;

            for (var count = 0; count < iterations; count++)
            {
                for (var index = 0; index < source.Count; index++)
                {
                    var nextIndex = random.Next(source.Count);
                    var next = source[nextIndex];
                    var current = source[index];

                    source[index] = next;
                    source[nextIndex] = current;
                }
            }
        }

        /// <summary>
        ///     Adds an element to a <see cref="IList{T}"/> if it doesn't exists, otherwise it updates it.
        /// </summary>
        /// <typeparam name="T">
        ///     The type of the element to be added.
        /// </typeparam>
        /// <param name="source">
        ///     The source collection.
        /// </param>
        /// <param name="element">
        ///     The element to search.
        /// </param>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the source is <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if <see cref="ICollection{T}.IsReadOnly"/> returns true.
        /// </exception>
        public static void AddOrUpdate<T>(this IList<T> source, [AllowNull] T element)
        {
            Ensure.NotNull(source);
            Ensure.NotReadOnly(source.IsReadOnly, source.GetType().Name);

            var index = source.IndexOf(element!);

            if (index == -1)
                source.Add(element!);
            else
                source[index] = element!;
        }

        /// <summary>
        ///     Gets a value given the key, or adds it if it doesn't exists.
        /// </summary>
        /// <typeparam name="TKey">
        ///     The type of the key.
        /// </typeparam>
        /// <typeparam name="TValue">
        ///     The type of the value.
        /// </typeparam>
        /// <param name="source">
        ///     The source dictionary.
        /// </param>
        /// <param name="key">
        ///     The key.
        /// </param>
        /// <param name="value">
        ///     The value.
        /// </param>
        /// <returns>
        ///     The value associated to the provided key.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either the source or the key are <see langword="null"/>.
        /// </exception>
        /// <exception cref="InvalidOperationException">
        ///     Throw when <see cref="ICollection{T}.IsReadOnly"/> is <see langword="true"/>.
        /// </exception>
        [return: MaybeNull]
        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
            where TKey : notnull
        {
            Ensure.NotNull(source);
            Ensure.NotNull(key);
            Ensure.NotReadOnly(source.IsReadOnly, source.GetType().Name);

            if (source.TryGetValue(key, out var val))
                return val;

            source.Add(key, value);
            return value;
        }

        /// <summary>
        ///     Indicates if the array contains the provided sequence.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the array.
        /// </typeparam>
        /// <param name="array">
        ///     The array.
        /// </param>
        /// <param name="sequence">
        ///     The sequence.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the array contains the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when either <paramref name="array"/> or <paramref name="sequence"/> are <see langword="null"/>.
        /// </exception>
        public static bool ContainsSequence<T>(this T[] array, params T[] sequence)
            => array.ContainsSequence(sequence, 0, array.Length);

        /// <summary>
        ///     Indicates if the array contains the given sequence from the provided starting index.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the array.
        /// </typeparam>
        /// <param name="array">
        ///     The array.
        /// </param>
        /// <param name="sequence">
        ///     The sequence.
        /// </param>
        /// <param name="startIndex">
        ///     The start index.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the array contains the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="array"/> or <paramref name="sequence"/> are <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="startIndex"/> is negative, higher than or equal to the length of <paramref name="array"/>.
        /// </exception>
        public static bool ContainsSequence<T>(this T[] array, T[] sequence, int startIndex, IEqualityComparer<T>? comparer = null)
            => array.ContainsSequence(sequence, startIndex, array.Length - startIndex, comparer);

        /// <summary>
        ///     Indicates if the array contains the given sequence from the provided range.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the array.
        /// </typeparam>
        /// <param name="array">
        ///     The array.
        /// </param>
        /// <param name="sequence">
        ///     The sequence.
        /// </param>
        /// <param name="startIndex">
        ///     The start index.
        /// </param>
        /// <param name="count">
        ///     The count.
        /// </param>
        /// <returns>
        ///     <see langword="true"/> if the array contains the sequence; otherwise, <see langword="false"/>.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown when <paramref name="array"/> or <paramref name="sequence"/> are <see langword="null"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when <paramref name="startIndex"/> and <paramref name="count"/> do not represent a valid range in the array.
        /// </exception>
        public static bool ContainsSequence<T>(this T[] array, T[] sequence, int startIndex, int count, IEqualityComparer<T>? comparer = null)
        {
            Ensure.NotNull(array);
            Ensure.NotNull(sequence);
            Ensure.NotOutOfRange((uint)startIndex < array.Length, startIndex);
            Ensure.NotOutOfRange(count <= 0, count);
            Ensure.NotOutOfRange(startIndex + count > array.Length, startIndex + count);

            comparer ??= EqualityComparer<T>.Default;

            if (sequence.Length > array.Length)
                return false;

            if (array.Length == 0)
                return sequence.Length == 0;

            if (sequence.Length == 0)
                return true;

            if (sequence.Length == 1)
                return Array.IndexOf(array, sequence[0]) != -1;

            if (sequence.Length == array.Length)
            {
                for (var i = 0; i < array.Length; i++)
                {
                    if (!comparer.Equals(array[i], sequence[i]))
                        return false;
                }

                return true;
            }

            var firstElement = sequence[0];
            var offset = Array.IndexOf(array, firstElement, startIndex, count);

            while (offset != -1 && offset + sequence.Length <= count)
            {
                var areEqual = true;

                // break the loop to avoid unnecessary calls if it's already false
                for (var index = 1; areEqual && index < sequence.Length; index++)
                    areEqual = comparer.Equals(array[offset + index], sequence[index]);

                if (areEqual)
                    return true;

                offset = Array.IndexOf(array, firstElement, offset + 1);
            }

            return false;
        }

        public static T EnqueueDequeue<T>(this Queue<T> queue, T item)
        {
            queue.Enqueue(item);
            return queue.Dequeue();
        }

        public static T PopPush<T>(this Stack<T> stack, T item)
        {
            var poped = stack.Pop();

            stack.Push(item);
            return poped;
        }

        public static int FindSequenceIndex<T>(this T[] array, T[] sequence, int startIndex, IEqualityComparer<T>? comparer = null)
            => array.FindSequenceIndex(sequence, startIndex, array.Length - startIndex);

        public static int FindSequenceIndex<T>(this T[] array, T[] sequence, int startIndex, int count, IEqualityComparer<T>? comparer = null)
        {
            Ensure.NotNull(array);
            Ensure.NotNull(sequence);
            Ensure.NotOutOfRange((uint)startIndex < array.Length, startIndex);
            Ensure.NotOutOfRange(count <= 0, count);
            Ensure.NotOutOfRange(startIndex + count > array.Length, startIndex + count);

            comparer ??= EqualityComparer<T>.Default;

            if (sequence.Length > array.Length || array.Length == 0 || sequence.Length == 0)
                return -1;

            if (sequence.Length == 1)
                return Array.IndexOf(array, sequence[0]);

            if (array.Length == sequence.Length)
            {
                for (var index = 0; index < array.Length; index++)
                {
                    if (!comparer.Equals(array[index], sequence[index]))
                        return -1;
                }

                return 0;
            }

            var m = sequence.Length;
            var n = array.Length;
            var i = 0;
            var j = 0;
            Span<int> lps = stackalloc int[m];

            Compute(sequence, m, lps, comparer);

            while (i < n)
            {
                if (comparer.Equals(sequence[j], array[i]))
                {
                    j++;
                    i++;
                }

                if (j == m)
                    return i - j;

                else if (i < n && !comparer.Equals(sequence[j], array[i]))
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }

            return -1;
        }

        public static int[] FindSequenceIndexes<T>(this T[] array, T[] sequence, int startIndex, IEqualityComparer<T>? comparer = null)
            => array.FindSequenceIndexes(sequence, startIndex, array.Length - startIndex, comparer);

        public static int[] FindSequenceIndexes<T>(this T[] array, T[] sequence, int startIndex, int count, IEqualityComparer<T>? comparer = null)
        {
            Ensure.NotNull(array);
            Ensure.NotNull(sequence);
            Ensure.NotOutOfRange((uint)startIndex < array.Length, startIndex);
            Ensure.NotOutOfRange(count <= 0, count);
            Ensure.NotOutOfRange(startIndex + count > array.Length, startIndex + count);

            comparer ??= EqualityComparer<T>.Default;

            if (sequence.Length > array.Length || array.Length == 0 || sequence.Length == 0)
                return Array.Empty<int>();

            if (sequence.Length == 1)
                return new[] { Array.IndexOf(array, sequence[0]) };

            if (array.Length == sequence.Length)
            {
                for (var index = 0; index < array.Length; index++)
                {
                    if (!comparer.Equals(array[index], sequence[index]))
                        return Array.Empty<int>();
                }

                return ZeroArray<int>.Value;
            }

            var indexes = new List<int>();
            var m = sequence.Length;
            var n = array.Length;
            var i = 0;
            var j = 0;
            Span<int> lps = stackalloc int[m];

            Compute(sequence, m, lps, comparer);

            while (i < n)
            {
                if (comparer.Equals(sequence[j], array[i]))
                {
                    j++;
                    i++;
                }

                if (j == m)
                {
                    indexes.Add(i - j);
                    j = lps[j - 1];
                }

                else if (i < n && !comparer.Equals(sequence[j], array[i]))
                {
                    if (j != 0)
                        j = lps[j - 1];
                    else
                        i++;
                }
            }

            return indexes.ToArray();
        }

        private static void Compute<T>(T[] sequence, int m, Span<int> lps, IEqualityComparer<T> comparer)
        {
            var len = 0;
            var i = 1;

            while (i < m)
            {
                if (comparer.Equals(sequence[i], sequence[len]))
                    lps[i++] = ++len;
                else
                {
                    if (len != 0)
                        len = lps[len - 1];
                    else
                        lps[i++] = 0;
                }
            }
        }
    }
}
