using NetUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    /// <summary>
    /// A true readonly generic list which provides most of <see cref="List{T}"/> methods.
    /// </summary>
    /// <typeparam name="T">The generic type of this instance</typeparam>
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _list;

        /// <summary>
        /// Creates a <see cref="ReadOnlyList{T}"/> from an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">This exception is thrown the if argument is null</exception>
        /// <param name="source">The source to create the list.</param>
        public ReadOnlyList(IEnumerable<T> source)
        {
            _list = Ensure.NotNull(source).ToList();
        }

        /// <summary>
        /// Creates a <see cref="ReadOnlyList{T}"/> from a given <see cref="List{T}"/>.
        /// By default the reference of the source list is kept.
        /// </summary>
        /// <param name="source">The source to create the list.</param>
        public ReadOnlyList(List<T> source) : this (source, true)
        {
        }

        /// <summary>
        /// Creates a <see cref="ReadOnlyList{T}"/> from a given <see cref="List{T}"/>.
        /// </summary>
        /// <param name="source">The source list.</param>
        /// <param name="keepReference">Determines if the reference of the source list should be kept.</param>
        public ReadOnlyList(List<T> source, bool keepReference)
        {
            Ensure.NotNull(source);

            if (keepReference)
                _list = source;
            else
                _list = source.ToList();
        }

        /// <inheritdoc/>
        public T this[int index]
        {
            [return: MaybeNull]
            get => _list[index];
        }

        /// <inheritdoc/>
        public T this[Index index]
        {
            [return: MaybeNull]
            get => _list[index.GetOffset(Count)];
        }

        /// <summary>
        /// Returns all elements in the given range.
        /// </summary>
        /// <param name="range">The range where all elements will be taken.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is outside of the bounds of this list.</exception>
        /// <returns>A new <see cref="ReadOnlyList{T}"/> with all the elements in the given range.</returns>
        public ReadOnlyList<T> this[Range range]
        {
            [return: NotNull]
            get => Slice(range);
        }

        /// <inheritdoc/>
        public int Count
            => _list.Count;

        /// <summary>
        /// Gets the enumerator for the current instance.
        /// </summary>
        /// <returns>An enumerator for the current instance.</returns>
        public List<T>.Enumerator GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Finds the index of the given item.
        /// </summary>
        /// <param name="item">The item whose index will be searched.</param>
        /// <returns>The index of the given item, -1 if it's not found.</returns>
        public int IndexOf([AllowNull]T item)
            => IndexOf(item, 0, Count);

        /// <summary>
        /// Finds the index of the given item starting from the index given.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid. (negative or >= Count)</exception>
        /// <param name="item">The item whose index will be searched.</param>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <returns>The index of the given item, -1 if it's not found.</returns>
        public int IndexOf([AllowNull]T item, int startIndex)
            => IndexOf(item, startIndex, Count);

        /// <summary>
        /// Finds the index of the given item starting from the index given.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid. (negative or >= Count)</exception>
        /// <param name="item">The item whose index will be searched.</param>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <param name="count">The amount of items that will be searched.</param>
        /// <returns>The index of the given item, -1 if it's not found.</returns>
        /// <inheritdoc/>
        public int IndexOf([AllowNull]T item, int startIndex, int count)
            => _list.IndexOf(item!, startIndex, count);

        /// <summary>
        /// Returns true if the list contains the provided element. otherwise false.
        /// </summary>
        /// <param name="item">The item that will be searched.</param>
        /// <returns>true if the items is found, otherwise false.</returns>
        public bool Contains([AllowNull]T item)
            => _list.Contains(item!);

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Finds the first element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The first item that matches the predicate. otherwise default.</returns>
        public T Find([NotNull]Predicate<T> predicate)
            => _list.Find(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds the last element that matches the given predicate
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The first item that matches the predicate. otherwise default.</returns>
        public T FindLast([NotNull]Predicate<T> predicate)
            => _list.FindLast(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds all the elements that matches the given predicate. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>A <see cref="ReadOnlyList{T}"/> with all the elements that matches the predicate.</returns>
        public ReadOnlyList<T> FindAll([NotNull]Predicate<T> predicate)
            => _list.FindAll(Ensure.NotNull(predicate, nameof(predicate))).ToReadOnlyList();

        /// <summary>
        /// Finds the index of the first element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the first element that matches the given predicate.</returns>
        public int FindIndex([NotNull]Predicate<T> predicate)
            => _list.FindIndex(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds the index of the first element that matches the given predicate starting from the given index.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid. (negative or >= Count)</exception>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the first element that matches the given predicate.</returns>
        public int FindIndex(int startIndex, [NotNull]Predicate<T> predicate)
            => _list.FindIndex(startIndex, Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds the index of the first element that matches the given predicate starting from the given index.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid. (negative or >= Count)</exception>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <param name="count">The amount of items that will be searched.</param>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the first element that matches the given predicate.</returns>
        public int FindIndex(int startIndex, int count, [NotNull]Predicate<T> predicate)
            => _list.FindIndex(startIndex, count, Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds the index of the last element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the last element that matches the given predicate.</returns>
        public int FindLastIndex([NotNull]Predicate<T> predicate)
            => FindLastIndex(0, Count, predicate);

        /// <summary>
        /// Finds the index of the last element that matches the given predicate starting from the given index.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid (negative or >= Count)</exception>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the last element that matches the given predicate.</returns>
        public int FindLastIndex(int startIndex, [NotNull]Predicate<T> predicate)
            => FindLastIndex(startIndex, Count, predicate);

        /// <summary>
        /// Finds the index of the last element that matches the given predicate starting from the given index.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid (negative or >= Count)</exception>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <param name="count">The amount of items to be searched.</param>
        /// <returns>The index of the last element that matches the given predicate.</returns>
        public int FindLastIndex(int startIndex, int count, [NotNull]Predicate<T> predicate)
            => _list.FindLastIndex(startIndex, count, predicate);

        /// <summary>
        /// Searches for the index of the given item using <see cref="EqualityComparer{T}.Default"/>
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>The index of the given item, -1 if not found.</returns>
        public int BinarySearch([AllowNull]T item)
            => _list.BinarySearch(item!);

        /// <summary>
        /// Searches for the index of the given item using the comparer provided.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the comparer is null.</exception>
        /// <param name="item">The item to be searched.</param>
        /// <param name="comparer">The comparer to be used in order to search the item.</param>
        /// <returns>The index of the given item, -1 if not found.</returns>
        public int BinarySearch([AllowNull]T item, [NotNull]IComparer<T> comparer)
            => _list.BinarySearch(item!, Ensure.NotNull(comparer, nameof(comparer)));

        /// <summary>
        /// Searches for the index of the given item using the comparer provided.
        /// </summary>
        /// <param name="startIndex">The index where the search will start.</param>
        /// <param name="count">The amount of elements to be searched.</param>
        /// <param name="item">The item to be searched.</param>
        /// <param name="comparer">The comparer to be used in order to search the item.</param>
        /// <returns>The index of the given item, -1 if not found.</returns>
        public int BinarySearch(int startIndex, int count, [AllowNull]T item, [NotNull]IComparer<T> comparer)
            => _list.BinarySearch(startIndex, count, item!, Ensure.NotNull(comparer, nameof(comparer)));

        /// <summary>
        /// Returns a <see cref="ReadOnlyList{TOut}"/> with all members of the current list converted into the target type.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the converter is null.</exception>
        /// <typeparam name="TOut">The output generic type of the <see cref="ReadOnlyList{T}"/></typeparam>
        /// <param name="converter">The delegate used to convert the items</param>
        /// <returns>Returns a <see cref="ReadOnlyList{TOut}"/></returns>
        public ReadOnlyList<TOut> ConvertAll<TOut>([NotNull]Converter<T, TOut> converter)
            => _list.ConvertAll(Ensure.NotNull(converter, nameof(converter))).ToReadOnlyList();

        /// <summary>
        /// Retrieves a new <see cref="ReadOnlyList{T}"/> with the elements found starting from the given index.
        /// </summary>
        /// <param name="startIndex">The index where elements will start to get taken from.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is outside of the bounds of this list.</exception>
        /// <returns>
        /// Returns a new <see cref="ReadOnlyList{T}"/> with all the elements from the provided index.
        /// </returns>
        public ReadOnlyList<T> Slice(int startIndex)
            => Slice(startIndex..Count);

        /// <summary>
        /// Retrieves a new <see cref="ReadOnlyList{T}"/> with the elements found 
        /// starting from the given index with the given amount of elements.
        /// </summary>
        /// <param name="startIndex">The index where elements will start to get taken from.</param>
        /// <param name="count">The amount of elements to be taken.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is outside of the bounds of this list.</exception>
        /// <returns>
        /// Returns a new <see cref="ReadOnlyList{T}"/> with all the elements from the provided index.
        /// </returns>
        public ReadOnlyList<T> Slice(int startIndex, int count)
            => Slice(startIndex..count);

        /// <summary>
        /// Retrieves a new <see cref="ReadOnlyList{T}"/> with the elements found 
        /// starting from the given index with the given amount of elements.
        /// </summary>
        /// <param name="range">The range.</param>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the range is outside of the bounds of this list.</exception>
        /// <returns>
        /// Returns a new <see cref="ReadOnlyList{T}"/> with all the elements from the provided index.
        /// </returns>
        public ReadOnlyList<T> Slice(Range range)
        {
            var (offset, length) = range.GetOffsetAndLength(Count);
            var list = new List<T>(length);

            for (var count = 0; count < length; count++)
                list.Add(this[offset + count]);

            return new ReadOnlyList<T>(list);
        }

        /// <summary>
        /// Checks if any element in the current instance matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The delegate used to filter the items.</param>
        /// <returns>True if any item matches the predicate. Otherwise false.</returns>
        public bool Any([NotNull]Predicate<T> predicate)
            => _list.Exists(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Checks if all elements in the current instance match the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The delegate used to filter the items.</param>
        /// <returns>True if all items match the predicate. Otherwise false.</returns>
        public bool All([NotNull]Predicate<T> predicate)
            => _list.TrueForAll(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Executes an action for each element in the list.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the action is null.</exception>
        /// <param name="action">The action delegate to be executed for each item in the list</param>
        public void ForEach([NotNull]Action<T> action)
            => _list.ForEach(Ensure.NotNull(action, nameof(action)));
    }
}
