using System.Linq;
#nullable enable
namespace System.Collections.Generic
{
    /// <summary>
    /// A true readonly generic List which provides most of <see cref="List{T}"/> methods.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _list;

        /// <summary>
        /// Creates a <see cref="ReadOnlyList{T}"/> from an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="sequence"></param>
        public ReadOnlyList(IEnumerable<T> sequence)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));
            _list = sequence.ToList();
        }

        /// <summary>
        /// Returns the element stored in the given index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"></exception>
        /// <param name="index"></param>
        /// <returns></returns>
        public T this[int index] 
            => _list[index];

        /// <summary>
        /// Returns true.
        /// </summary>
        public bool IsReadOnly
            => true;

        /// <summary>
        /// Returns the amount of element in the current instace.
        /// </summary>
        public int Count
            => _list.Count;

        /// <summary>
        /// Gets the enumerator for the current instance.
        /// </summary>
        /// <returns></returns>
        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Gets the index of the provided item, you can optionally provide as well the starting index and the count of indexes.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="item"></param>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int IndexOf(T item, int index = 0, int? count = null)
            => _list.IndexOf(item, index, count ?? Count);

        /// <summary>
        /// Returns true if the list contains the provided element. otherwise false.
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool Contains(T item)
            => _list.Contains(item);

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Finds the first element that matches the given predicate, or the default value if no matching element is found.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T Find(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int x = 0; x < Count; x++)
            {
                var element = this[x];
                if (predicate(element))
                    return element;
            }

            return default!;
        }

        /// <summary>
        /// Finds the first element that matches the given predicate, or the default value if no matching element is found.
        /// (This method iterates from last to first)
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public T FindLast(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int x = Count - 1; x >= 0; x--)
            {
                var element = this[x];
                if (predicate(element))
                    return element;
            }

            return default!;
        }

        /// <summary>
        /// Finds all the elements that matches the given predicate. 
        /// You may as well limit the amount of elements that can be returned. 
        /// The number of elements in the returning collection is not guaranteed to be the same of maxCount if provided.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public ReadOnlyList<T> FindAll(Predicate<T> predicate, int? maxCount = null)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            var capacity = maxCount ?? Count;
            var list = new List<T>(capacity);

            for (int x = 0; x < Count; x++)
            {
                var element = this[x];

                if (predicate(element))
                {
                    list.Add(element);
                    if (list.Count == capacity)
                        break;
                }
            }

            return list.ToReadOnlyList();
        }

        /// <summary>
        /// Finds the index of the first element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="predicate"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int FindIndex(Predicate<T> predicate, int startIndex = 0, int? count = null)
        {
            count ??= Count;

            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (startIndex < 0 || startIndex >= Count || count <= 0 || count > Count)
                throw new ArgumentOutOfRangeException(nameof(startIndex));

            var index = startIndex;

            for (int counter = 1; counter <= count; counter++, index++)
            {
                var element = this[index];

                if (predicate(element))
                    return index;
            }

            return -1;
        }

        /// <summary>
        /// Finds the index of the first element that matches the given predicate.
        /// (This method iterates from last to first)
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="predicate"></param>
        /// <param name="startIndex"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public int FindLastIndex(Predicate<T> predicate, int startIndex = 0, int? count = null)
        {
            count ??= Count;

            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (startIndex < 0 || startIndex >= Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count <= 0 || count > Count) throw new ArgumentOutOfRangeException(nameof(count));

            var index = startIndex + count.Value - 1;

            for (int counter = 1; counter <= count; counter++, index--)
            {
                var element = this[index];

                if (predicate(element))
                    return index;
            }

            return -1;
        }

        /// <inheritdoc />
        public int BinarySearch(T item)
            => _list.BinarySearch(item);

        /// <inheritdoc />
        public int BinarySearch(T item, IComparer<T> comparer)
            => _list.BinarySearch(item, comparer);

        /// <inheritdoc />
        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
            => _list.BinarySearch(index, count, item, comparer);

        /// <summary>
        /// Returns a <see cref="ReadOnlyList{TOut}"/> with all members of the current list converted into the target type.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <typeparam name="TOut"></typeparam>
        /// <param name="converter"></param>
        /// <returns></returns>
        public ReadOnlyList<TOut> ConvertAll<TOut>(Converter<T, TOut> converter)
            => new ReadOnlyList<TOut>(_list.ConvertAll(converter));

        /// <summary>
        /// Returns true if an element of the list matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Exists(Predicate<T> predicate)
            => _list.Exists(predicate);

        /// <summary>
        /// Gets the given amount of members from the given index
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        /// <param name="index"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public ReadOnlyList<T> GetRange(int index, int count)
        {
            if (index < 0 || index >= Count) throw new ArgumentOutOfRangeException(nameof(index));
            if (index + count >= Count) throw new ArgumentOutOfRangeException(nameof(count));

            var list = new List<T>(count);

            for (int counter = 1; counter <= count; counter++)
            {
                list.Add(this[index]);
                index++;
            }

            return list.ToReadOnlyList();
        }

        /// <summary>
        /// Returns true if any of the elements matches the predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int index = 0; index < Count; index++)
            {
                if (predicate(this[index]))
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Returns true if all elements matches the predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool All(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int index = 0; index < Count; index++)
            {
                if (!predicate(this[index]))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Executes an action for each element in the list.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="action"></param>
        public void ForEach(Action<T> action)
        {
            if (action is null) throw new ArgumentNullException(nameof(action));

            for (int index = 0; index < Count; index++)
            {
                action(this[index]);
            }
        }
    }
}
