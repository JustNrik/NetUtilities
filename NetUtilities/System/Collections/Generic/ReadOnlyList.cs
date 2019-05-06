using System.Linq;

namespace System.Collections.Generic
{
    public class ReadOnlyList<T> : IList<T>, IReadOnlyList<T>
    {
        private readonly List<T> _list;

        public ReadOnlyList(IEnumerable<T> sequence)
        {
            if (sequence is null) throw new ArgumentNullException(nameof(sequence));
            _list = sequence is List<T> list ? list : sequence.ToList();
        }

        public T this[int index]
            => _list[index];

        T IList<T>.this[int index]
        {
            get => _list[index];
            set => throw new InvalidOperationException("This list is Read-Only");
        }

        public bool IsReadOnly
            => true;

        public int Count
            => _list.Count;

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _list.GetEnumerator();

        public int IndexOf(T item)
            => _list.IndexOf(item);

        public bool Contains(T item)
            => _list.Contains(item);

        public IEnumerator GetEnumerator()
            => _list.GetEnumerator();

        void ICollection<T>.Add(T item)
            => throw new InvalidOperationException("This list is Read-Only");

        void ICollection<T>.Clear()
            => throw new InvalidOperationException("This list is Read-Only");

        void ICollection<T>.CopyTo(T[] array, int arrayIndex)
            => throw new InvalidOperationException("This list is Read-Only");

        void IList<T>.Insert(int index, T item)
            => throw new InvalidOperationException("This list is Read-Only");

        bool ICollection<T>.Remove(T item)
            => throw new InvalidOperationException("This list is Read-Only");

        void IList<T>.RemoveAt(int index)
            => throw new InvalidOperationException("This list is Read-Only");
    }
}
