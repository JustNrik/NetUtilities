using NetUtilities;
using System.Linq;
#nullable enable
namespace System.Collections.Generic
{
    /// <summary>
    /// A true readonly generic List which provides most of <see cref="List{T}"/> methods.
    /// </summary>
    /// <typeparam name="T">The generic type of this instance</typeparam>
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _list;

        /// <summary>
        /// Creates a <see cref="ReadOnlyList{T}"/> from an <see cref="IEnumerable{T}"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="sequence"/>
        public ReadOnlyList(IEnumerable<T> sequence)
        {
            Ensure.NotNull(sequence, nameof(sequence));
            _list = sequence.ToList();
        }

        /// <summary>
        /// Returns the element stored in the given index.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <param name="index"/>
        /// <returns/>
        public T this[int index] 
            => _list[index];

        /// <summary>
        /// Returns true.
        /// </summary>
        /// <returns/>
        public bool IsReadOnly
            => true;

        /// <summary>
        /// Returns the amount of element in the current instace.
        /// </summary>
        /// <returns/>
        public int Count
            => _list.Count;

        /// <summary>
        /// Gets the enumerator for the current instance.
        /// </summary>
        /// <returns/>
        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Gets the index of the provided item, you can optionally provide as well the starting index and the count of indexes.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="item"/>
        /// <param name="index"/>
        /// <param name="count"/>
        /// <returns/>
        public int IndexOf(T item, int index = 0, int? count = null)
            => _list.IndexOf(item, index, count ?? Count);

        /// <summary>
        /// Returns true if the list contains the provided element. otherwise false.
        /// </summary>
        /// <param name="item"/>
        /// <returns/>
        public bool Contains(T item)
            => _list.Contains(item);

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        /// <summary>
        /// Finds the first element that matches the given predicate, or the default value if no matching element is found.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="predicate"/>
        /// <returns/>
        public T Find(Predicate<T> predicate)
            => _list.Find(predicate);

        /// <summary>
        /// Finds the first element that matches the given predicate, or the default value if no matching element is found.
        /// (This method iterates from last to first)
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="predicate"/>
        /// <returns/>
        public T FindLast(Predicate<T> predicate)
            => _list.FindLast(predicate);

        /// <summary>
        /// Finds all the elements that matches the given predicate. 
        /// You may as well limit the amount of elements that can be returned. 
        /// The number of elements in the returning collection is not guaranteed to be the same of maxCount if provided.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <param name="maxCount"></param>
        /// <returns></returns>
        public ReadOnlyList<T> FindAll(Predicate<T> predicate)
            => _list.FindAll(predicate).ToReadOnlyList();

        /// <summary>
        /// Finds the index of the first element that matches the given predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <param name="predicate"/>
        /// <returns/>
        public int FindIndex(Predicate<T> match)
            => _list.FindIndex(match);

        /// <summary>
        /// Finds the index of the first element that matches the given predicate starting from the given index.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="startIndex"/>
        /// <param name="predicate"/>
        /// <returns/>
        public int FindIndex(int startIndex, Predicate<T> predicate)
            => _list.FindIndex(startIndex, predicate);

        /// <summary>
        /// Finds the index of the first element that matches the given predicate starting from the given index and number of elements.
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="startIndex"/>
        /// <param name="count"/>
        /// <param name="predicate"/>
        /// <returns/>
        public int FindIndex(int startIndex, int count, Predicate<T> predicate)
            => _list.FindIndex(startIndex, count, predicate);

        /// <summary>
        /// Finds the index of the first element that matches the given predicate.
        /// (This method iterates from last to first)
        /// </summary>
        /// <exception cref="ArgumentNullException"/>
        /// <exception cref="ArgumentOutOfRangeException"/>
        /// <param name="predicate"/>
        /// <param name="startIndex"/>
        /// <param name="count"/>
        /// <returns/>
        public int FindLastIndex(Predicate<T> predicate, int startIndex = 0, int? count = null)
        {
            count ??= Count;

            Ensure.NotNull(predicate, nameof(predicate));
            Ensure.IndexInRange(startIndex, count.Value);
            Ensure.ValidCount(startIndex, count.Value, Count);

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
        /// <typeparam name="TOut">The output generic type of the <see cref="ReadOnlyList{T}"/></typeparam>
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
            Ensure.IndexInRange(index, count);
            Ensure.ValidCount(index, count, Count);

            var list = new List<T>(count);

            for (int counter = 1; counter <= count; counter++)
            {
                list.Add(this[index]);
                index++;
            }

            return list.ToReadOnlyList();
        }

        public ReadOnlyList<T> GetRange(Range range)
        {
            var (index, count) = range.GetOffsetAndLength(Count);
            return GetRange(index, count);
        }

        /// <summary>
        /// Returns true if any of the elements matches the predicate.
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public bool Any(Predicate<T> predicate)
        {
            Ensure.NotNull(predicate, nameof(predicate));

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
            Ensure.NotNull(predicate, nameof(predicate));

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
            Ensure.NotNull(action, nameof(action));

            for (int index = 0; index < Count; index++)
            {
                action(this[index]);
            }
        }
    }
}
