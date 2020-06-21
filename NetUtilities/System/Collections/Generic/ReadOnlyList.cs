using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetUtilities;

namespace System.Collections.Generic
{
    /// <summary>
    /// A true readonly generic list which provides most of <see cref="List{T}"/> methods.
    /// </summary>
    /// <typeparam name="T">The generic type of this instance</typeparam>
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        internal static readonly ReadOnlyList<T> Empty = new ReadOnlyList<T>(Array.Empty<T>());

        private readonly List<T> _list;

        /// <inheritdoc/>
        public T this[int index]
            => _list[index];

        /// <inheritdoc/>
        public T this[Index index]
            => _list[index.GetOffset(Count)];

        /// <summary>
        ///     Gets a new <see cref="ReadOnlyList{T}"/> containing all elements in the provided range.
        /// </summary>
        /// <param name="range">
        ///    The range where all elements will be taken.
        /// </param>
        /// <returns>
        ///     A new <see cref="ReadOnlyList{T}"/> containing all elements in the provided range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the range is outside of the bounds of this list.
        /// </exception>
        public ReadOnlyList<T> this[Range range]
            => GetRange(range);

        /// <inheritdoc/>
        public int Count
            => _list.Count;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ReadOnlyList{T}"/> <see langword="class"/>
        ///     with the provided enumerable source.
        /// </summary>
        /// <param name="source">
        ///     The source.
        /// </param>
        public ReadOnlyList(IEnumerable<T> source)
        {
            _list = Ensure.NotNull(source).ToList();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lookup{TKey, TValue}"/> <see langword="class"/> 
        ///     with the provided list.
        /// </summary>
        /// <param name="source">
        ///     The source list.
        /// </param>
        public ReadOnlyList(List<T> source) : this(source, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="Lookup{TKey, TValue}"/> <see langword="class"/>
        ///     with the provided list. Optionally you can indicate if the reference of the list should be kept.
        /// </summary>
        /// <param name="source">
        ///     The source list.
        /// </param>
        /// <param name="keepReference">
        ///     Indicates if the reference of the list should be kept.
        /// </param>
        public ReadOnlyList(List<T> source, bool keepReference)
        {
            Ensure.NotNull(source);

            if (keepReference)
                _list = source;
            else
                _list = source.ToList();
        }


        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <returns>
        ///     A <see cref="List{T}.Enumerator"/> for the <see cref="ReadOnlyList{T}"/>.
        /// </returns>
        public List<T>.Enumerator GetEnumerator()
            => _list.GetEnumerator();

        /// <inheritdoc cref="List{T}.IndexOf(T)"/>
        public int IndexOf(T item)
            => IndexOf(item, 0, Count);

        /// <inheritdoc cref="List{T}.IndexOf(T, int)"/>
        public int IndexOf(T item, int startIndex)
            => IndexOf(item, startIndex, Count);

        /// <inheritdoc cref="List{T}.IndexOf(T, int, int)"/>
        public int IndexOf(T item, int startIndex, int count)
            => _list.IndexOf(item, startIndex, count);

        /// <inheritdoc cref="List{T}.Contains(T)"/>
        public bool Contains(T item)
            => _list.Contains(item);

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _list.GetEnumerator();

        /// <inheritdoc cref="List{T}.Find(Predicate{T})"/>
        [return: MaybeNull]
        public T Find(Predicate<T> predicate)
            => _list.Find(Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.FindLast(Predicate{T})"/>
        [return: MaybeNull]
        public T FindLast(Predicate<T> predicate)
            => _list.FindLast(Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.FindAll(Predicate{T})"/>
        public ReadOnlyList<T> FindAll(Predicate<T> predicate)
            => _list.FindAll(Ensure.NotNull(predicate)).ToReadOnlyList();

        /// <inheritdoc cref="List{T}.FindIndex(Predicate{T})"/>
        public int FindIndex(Predicate<T> predicate)
            => _list.FindIndex(Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.FindIndex(int, Predicate{T})"/>
        public int FindIndex(int startIndex, Predicate<T> predicate)
            => _list.FindIndex(startIndex, Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.FindIndex(int, int, Predicate{T})"/>
        public int FindIndex(int startIndex, int count, Predicate<T> predicate)
            => _list.FindIndex(startIndex, count, Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.FindLastIndex(Predicate{T})"/>
        public int FindLastIndex(Predicate<T> predicate)
            => FindLastIndex(0, Count, predicate);

        /// <inheritdoc cref="List{T}.FindLastIndex(int, Predicate{T})"/>
        public int FindLastIndex(int startIndex, [NotNull]Predicate<T> predicate)
            => FindLastIndex(startIndex, Count, predicate);

        /// <inheritdoc cref="List{T}.FindLastIndex(int, int, Predicate{T})"/>
        public int FindLastIndex(int startIndex, int count, Predicate<T> predicate)
            => _list.FindLastIndex(startIndex, count, predicate);

        /// <inheritdoc cref="List{T}.BinarySearch(T)"/>
        public int BinarySearch(T item)
            => _list.BinarySearch(item);

        /// <inheritdoc cref="List{T}.BinarySearch(T, IComparer{T})"/>
        public int BinarySearch([AllowNull]T item, IComparer<T>? comparer)
            => _list.BinarySearch(item, comparer);

        /// <inheritdoc cref="List{T}.BinarySearch(int, int, T, IComparer{T})"/>
        public int BinarySearch(int startIndex, int count, T item, IComparer<T>? comparer)
            => _list.BinarySearch(startIndex, count, item, comparer);

        /// <inheritdoc cref="List{T}.ConvertAll{TOutput}(Converter{T, TOutput})"/>
        public ReadOnlyList<TOut> ConvertAll<TOut>(Converter<T, TOut> converter)
            => _list.ConvertAll(Ensure.NotNull(converter)).ToReadOnlyList();

        /// <summary>
        ///     Gets a new <see cref="ReadOnlyList{T}"/> containing all elements starting from the provided index.
        /// </summary>
        /// <param name="startIndex">
        ///     The zero-based <see cref="ReadOnlyList{T}"/> index at which the range starts.
        /// </param>
        /// <returns>
        ///     A new <see cref="ReadOnlyList{T}"/> containing all elements in the provided range.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when the range is outside of the bounds of this list.
        /// </exception>
        public ReadOnlyList<T> GetRange(int startIndex)
            => GetRange(startIndex..);

        /// <inheritdoc cref="List{T}.GetRange(int, int)"/>
        public ReadOnlyList<T> GetRange(int startIndex, int count)
            => GetRange(startIndex..count);

        /// <summary>
        ///     Creates a shallow copy of a range of elements in the source <see cref="ReadOnlyList{T}"/>.
        /// </summary>
        /// <param name="range">
        ///    The range where all elements will be taken.
        /// </param>
        /// <returns>
        ///     A shallow copy of a range of elements in the source <see cref="ReadOnlyList{T}".
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     index and count do not denote a valid range of elements in the <see cref="ReadOnlyList{T}"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     index is less than 0. -or- count is less than 0.
        /// </exception>
        public ReadOnlyList<T> GetRange(Range range)
        {
            var (offset, length) = range.GetOffsetAndLength(Count);
            return new ReadOnlyList<T>(_list.GetRange(offset, length));
        }

        /// <inheritdoc cref="List{T}.Exists(Predicate{T})"/>
        public bool Exists(Predicate<T> predicate)
            => _list.Exists(Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.TrueForAll(Predicate{T})"/>
        public bool TrueForAll(Predicate<T> predicate)
            => _list.TrueForAll(Ensure.NotNull(predicate));

        /// <inheritdoc cref="List{T}.ForEach(Action{T})"/>
        public void ForEach(Action<T> action)
            => _list.ForEach(Ensure.NotNull(action));
    }
}
