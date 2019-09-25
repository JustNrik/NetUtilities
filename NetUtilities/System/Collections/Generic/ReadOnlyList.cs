using NetUtilities;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Runtime.CompilerServices;
using MethodImplementation = System.Runtime.CompilerServices.MethodImplAttribute;

namespace System.Collections.Generic
{
    /// <summary>
    /// A true readonly generic List which provides most of <see cref="List{T}"/> methods.
    /// </summary>
    /// <typeparam name="T">The generic type of this instance</typeparam>
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        private const MethodImplOptions Inlined = MethodImplOptions.AggressiveInlining;
        private readonly List<T> _list;

        /// <summary>
        /// Creates a <see cref="ReadOnlyList{T}"/> from an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException">This exception is thrown the if argument is null</exception>
        /// <param name="source">The source to create the list.</param>
        public ReadOnlyList([NotNull]IEnumerable<T> source)
            => _list = Ensure.NotNull(source, nameof(source)).ToList();

        /// <summary>
        /// Returns the element stored in the given index.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">This exception is throw if the index is out of the bounds of this list</exception>
        /// <param name="index">The index of the element that will be taken.</param>
        /// <returns>The element in the given index.</returns>
        public T this[int index]
        {
            [MethodImplementation(Inlined)]
            [return: MaybeNull]
            get => _list[index];
        }

        /// <summary>
        /// Returns the element stored in the given index.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">This exception is throw if the index is out of the bounds of this list.</exception>
        /// <param name="index">The index of the element that will be taken.</param>
        /// <returns>The element in the given index.</returns>
        public T this[Index index]
        {
            [MethodImplementation(Inlined)]
            [return: MaybeNull]
            get
            {
                var indice = index.GetOffset(Count);
                return _list[indice];
            }
        }

        /// <summary>
        /// Returns all elements in the given range.
        /// </summary>
        /// <param name="range">The range where all elements will be taken.</param>
        /// <returns>A new <see cref="ReadOnlyList{T}"/> with all the elements in the given range.</returns>
        public ReadOnlyList<T> this[Range range]
        {
            [MethodImplementation(Inlined)]
            [return: NotNull]
            get
            {
                var (startIndex, count) = range.GetOffsetAndLength(_list.Count);
                return Slice(startIndex, count);
            }
        }

        /// <summary>
        /// Returns true.
        /// </summary>
        /// <returns>Returns true.</returns>
        public bool IsReadOnly
        {
            [MethodImplementation(Inlined)]
            get => true;
        }

        /// <summary>
        /// Returns the amount of element in the current instace.
        /// </summary>
        /// <returns>The amount of elements in the current instance.</returns>
        public int Count
        {
            [MethodImplementation(Inlined)]
            get => _list.Count;
        }

        /// <summary>
        /// Gets the enumerator for the current instance.
        /// </summary>
        /// <returns>An enumerator for the current instance.</returns>
        [MethodImplementation(Inlined)]
        [return: NotNull]
        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Finds the index of the given item.
        /// </summary>
        /// <param name="item">The item whose index will be searched.</param>
        /// <returns>The index of the given item, -1 if it's not found.</returns>
        [MethodImplementation(Inlined)]
        public int IndexOf([AllowNull]T item)
            => IndexOf(item, 0, Count);

        /// <summary>
        /// Finds the index of the given item starting from the index given.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid. (negative or >= Count)</exception>
        /// <param name="item">The item whose index will be searched.</param>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <returns>The index of the given item, -1 if it's not found.</returns>
        [MethodImplementation(Inlined)]
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
        [MethodImplementation(Inlined)]
        public int IndexOf([AllowNull]T item, int startIndex, int count)
        {
            Ensure.ValidCount(startIndex, count, Count);
            return _list.IndexOf(item, startIndex, count);
        }

        /// <summary>
        /// Returns true if the list contains the provided element. otherwise false.
        /// </summary>
        /// <param name="item">The item that will be searched.</param>
        /// <returns>true if the items is found, otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public bool Contains([AllowNull]T item)
            => _list.Contains(item);

        [MethodImplementation(Inlined)]
        [return: NotNull]
        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Finds the first element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The first item that matches the predicate. otherwise default.</returns>
        [MethodImplementation(Inlined)]
        [return: MaybeNull]
        public T Find([NotNull]Predicate<T> predicate)
            => _list.Find(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds the last element that matches the given predicate
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The first item that matches the predicate. otherwise default.</returns>
        [MethodImplementation(Inlined)]
        [return: MaybeNull]
        public T FindLast([NotNull]Predicate<T> predicate)
            => _list.FindLast(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Finds all the elements that matches the given predicate. 
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>A <see cref="ReadOnlyList{T}"/> with all the elements that matches the predicate.</returns>
        [MethodImplementation(Inlined)]
        [return: NotNull]
        public ReadOnlyList<T> FindAll([NotNull]Predicate<T> predicate)
            => _list.FindAll(Ensure.NotNull(predicate, nameof(predicate))).ToReadOnlyList();

        /// <summary>
        /// Finds the index of the first element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the first element that matches the given predicate.</returns>
        [MethodImplementation(Inlined)]
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
        [MethodImplementation(Inlined)]
        public int FindIndex(int startIndex, [NotNull]Predicate<T> predicate)
        {
            Ensure.IndexInRange(startIndex, Count);
            return _list.FindIndex(startIndex, Ensure.NotNull(predicate, nameof(predicate)));
        }

        /// <summary>
        /// Finds the index of the first element that matches the given predicate starting from the given index.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when the start index is not valid. (negative or >= Count)</exception>
        /// <param name="startIndex">The starting index where the search will begin.</param>
        /// <param name="count">The amount of items that will be searched.</param>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the first element that matches the given predicate.</returns>
        [MethodImplementation(Inlined)]
        public int FindIndex(int startIndex, int count, [NotNull]Predicate<T> predicate)
        {
            Ensure.ValidCount(startIndex, count, Count);
            return _list.FindIndex(startIndex, count, Ensure.NotNull(predicate, nameof(predicate)));
        }

        /// <summary>
        /// Finds the index of the last element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The predicate used to find a matching item.</param>
        /// <returns>The index of the last element that matches the given predicate.</returns>
        [MethodImplementation(Inlined)]
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
        [MethodImplementation(Inlined)]
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
        [MethodImplementation(Inlined)]
        public int FindLastIndex(int startIndex, int count, [NotNull]Predicate<T> predicate)
        {
            Ensure.IndexInRange(startIndex, count);
            return _list.FindLastIndex(startIndex, count, predicate);
        }

        /// <summary>
        /// Searches for the index of the given item using <see cref="EqualityComparer{T}.Default"/>
        /// </summary>
        /// <param name="item">The item to be searched</param>
        /// <returns>The index of the given item, -1 if not found.</returns>
        [MethodImplementation(Inlined)]
        public int BinarySearch([AllowNull]T item)
            => _list.BinarySearch(item);

        /// <summary>
        /// Searches for the index of the given item using the comparer provided.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the comparer is null.</exception>
        /// <param name="item">The item to be searched.</param>
        /// <param name="comparer">The comparer to be used in order to search the item.</param>
        /// <returns>The index of the given item, -1 if not found.</returns>
        [MethodImplementation(Inlined)]
        public int BinarySearch([AllowNull]T item, [NotNull]IComparer<T> comparer)
            => _list.BinarySearch(item, Ensure.NotNull(comparer, nameof(comparer)));

        /// <summary>
        /// Searches for the index of the given item using the comparer provided.
        /// </summary>
        /// <param name="startIndex">The index where the search will start.</param>
        /// <param name="count">The amount of elements to be searched.</param>
        /// <param name="item">The item to be searched.</param>
        /// <param name="comparer">The comparer to be used in order to search the item.</param>
        /// <returns>The index of the given item, -1 if not found.</returns>
        [MethodImplementation(Inlined)]
        public int BinarySearch(int startIndex, int count, [AllowNull]T item, [NotNull]IComparer<T> comparer)
        {
            Ensure.ValidCount(startIndex, count, Count);
            Ensure.NotNull(comparer, nameof(comparer));
            return _list.BinarySearch(startIndex, count, item, comparer);
        }

        /// <summary>
        /// Returns a <see cref="ReadOnlyList{TOut}"/> with all members of the current list converted into the target type.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when the converter is null.</exception>
        /// <typeparam name="TOut">The output generic type of the <see cref="ReadOnlyList{T}"/></typeparam>
        /// <param name="converter">The delegate used to convert the items</param>
        /// <returns>Returns a <see cref="ReadOnlyList{TOut}"/></returns>
        [MethodImplementation(Inlined)]
        [return: NotNull]
        public ReadOnlyList<TOut> ConvertAll<TOut>([NotNull]Converter<T, TOut> converter)
            => _list.ConvertAll(Ensure.NotNull(converter, nameof(converter))).ToReadOnlyList();

        /// <summary>
        /// Retrieves a new <see cref="ReadOnlyList{T}"/> with the elements found starting from the given index.
        /// </summary>
        /// <param name="startIndex">The index where elements will start to get taken from.</param>
        /// <returns>
        /// Returns a new <see cref="ReadOnlyList{T}"/> with all the elements from the provided index.
        /// </returns>
        [MethodImplementation(Inlined)]
        [return: NotNull]
        public ReadOnlyList<T> Slice(int startIndex)
            => Slice(startIndex, Count - startIndex - 1);

        /// <summary>
        /// Retrieves a new <see cref="ReadOnlyList{T}"/> with the elements found 
        /// starting from the given index with the given amount of elements.
        /// </summary>
        /// <param name="startIndex">The index where elements will start to get taken from.</param>
        /// <param name="count">The amount of elements to be taken.</param>
        /// <returns>
        /// Returns a new <see cref="ReadOnlyList{T}"/> with all the elements from the provided index.
        /// </returns>
        [MethodImplementation(Inlined)]
        [return: NotNull]
        public ReadOnlyList<T> Slice(int startIndex, int count)
        {
            Ensure.ValidCount(startIndex, count, Count);

            if (startIndex == 0 && count == _list.Count - 1) return this;

            var array = new T[count];
            var index = startIndex;

            for (var currentCount = 1; currentCount <= count; currentCount++, index++)
                array[index] = _list[index];

            return new ReadOnlyList<T>(array);
        }

        /// <summary>
        /// Checks if any element in the current instance matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The delegate used to filter the items.</param>
        /// <returns>True if any item matches the predicate. Otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public bool Exists([NotNull]Predicate<T> predicate)
            => _list.Exists(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Checks if all elements in the current instance match the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the predicate is null.</exception>
        /// <param name="predicate">The delegate used to filter the items.</param>
        /// <returns>True if all items match the predicate. Otherwise false.</returns>
        [MethodImplementation(Inlined)]
        public bool TrueForAll([NotNull]Predicate<T> predicate)
            => _list.TrueForAll(Ensure.NotNull(predicate, nameof(predicate)));

        /// <summary>
        /// Executes an action for each element in the list.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown if the action is null.</exception>
        /// <param name="action">The action delegate to be executed for each item in the list</param>
        [MethodImplementation(Inlined)]
        public void ForEach([NotNull]Action<T> action)
            => _list.ForEach(Ensure.NotNull(action, nameof(action)));
    }
}
