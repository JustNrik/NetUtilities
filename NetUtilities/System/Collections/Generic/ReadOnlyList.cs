using System.Linq;
using System.Runtime.CompilerServices;

namespace System.Collections.Generic
{
    public sealed class ReadOnlyList<T> : IReadOnlyList<T>
    {
        private readonly List<T> _list;

        public ReadOnlyList(IEnumerable<T> sequence)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));
            _list = sequence is List<T> list ? list : sequence.ToList();
        }
        public T this[int index]
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list [index];
        }

        public bool IsReadOnly
            => true;

        public int Count
        {
            [MethodImpl(MethodImplOptions.AggressiveInlining)]
            get => _list.Count;
        }

        public IEnumerator<T> GetEnumerator()
            => _list.GetEnumerator();

        public int IndexOf(T item)
            => IndexOf(item, 0, Count);

        public int IndexOf(T item, int index)
            => IndexOf(item, index, Count);

        public int IndexOf(T item, int index, int count)
            => _list.IndexOf(item, index, count);

        public bool Contains(T item)
            => _list.Contains(item);

        IEnumerator IEnumerable.GetEnumerator()
            => _list.GetEnumerator();

        public T Find(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int x = 0; x < Count; x++)
            {
                var element = this[x];
                if (predicate(element))
                    return element;
            }

            return default;
        }

        public T FindLast(Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));

            for (int x = Count - 1; x >= 0; x--)
            {
                var element = this[x];
                if (predicate(element))
                    return element;
            }

            return default;
        }

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

        public int FindIndex(Predicate<T> predicate)
            => FindIndex(0, Count, predicate);

        public int FindIndex(int startIndex, Predicate<T> predicate)
            => FindIndex(startIndex, Count, predicate);

        public int FindIndex(int startIndex, int count, Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (startIndex < 0 || startIndex >= Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count <= 0 || count > Count) throw new ArgumentOutOfRangeException(nameof(startIndex));

            var index = startIndex;

            for (int counter = 1; counter <= count; counter++)
            {
                var element = this[index];

                if (predicate(element))
                    return index;

                index++;
            }

            return -1;
        }

        public int FindLastIndex(Predicate<T> predicate)
            => FindLastIndex(0, Count, predicate);

        public int FindLastIndex(int startIndex, Predicate<T> predicate)
            => FindLastIndex(startIndex, Count, predicate);

        public int FindLastIndex(int startIndex, int count, Predicate<T> predicate)
        {
            if (predicate is null) throw new ArgumentNullException(nameof(predicate));
            if (startIndex < 0 || startIndex >= Count) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count <= 0 || count > Count) throw new ArgumentOutOfRangeException(nameof(count));

            var index = startIndex + count - 1;

            for (int counter = 1; counter <= count; counter++)
            {
                var element = this[index];

                if (predicate(element))
                    return index;

                index--;
            }

            return -1;
        }

        public int BinarySearch(T item)
            => _list.BinarySearch(item);

        public int BinarySearch(T item, IComparer<T> comparer)
            => _list.BinarySearch(item, comparer);

        public int BinarySearch(int index, int count, T item, IComparer<T> comparer)
            => _list.BinarySearch(index, count, item, comparer);

        public ReadOnlyList<TOut> ConvertAll<TOut>(Converter<T, TOut> converter)
            => new ReadOnlyList<TOut>(_list.ConvertAll(converter));

        public bool Exists(Predicate<T> predicate)
            => _list.Exists(predicate);

        public ReadOnlyList<T> GetRange(int index, int count)
        {
            var list = new List<T>(count);

            for (int counter = 1; counter <= count; counter++)
            {
                list.Add(this[index]);
                index++;
            }

            return list.ToReadOnlyList();
        }

        public void TrimExcess()
            => _list.TrimExcess();

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
