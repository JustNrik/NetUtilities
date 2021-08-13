using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Generic
{
    public sealed class CyclingQueue<T> : IReadOnlyCollection<T>
    {
        private readonly Queue<T> _queue;

        public int Count
            => _queue.Count;

        public CyclingQueue()
        {
            _queue = new();
        }

        public CyclingQueue(int capacity)
        {
            _queue = new(capacity);
        }

        public CyclingQueue(IEnumerable<T> source)
        {
            _queue = new(source);
        }

        public void Enqueue(T item)
            => _queue.Enqueue(item);

        public T Dequeue()
        {
            var item = _queue.Dequeue();

            _queue.Enqueue(item);
            return item;
        }

        public void Clear()
            => _queue.Clear();

        public T Peek()
            => _queue.Peek();

        public bool TryDequeue([MaybeNullWhen(false)] out T value)
        {
            var dequeued = _queue.TryDequeue(out value);

            if (dequeued)
                _queue.Enqueue(value!);

            return dequeued;
        }

        public bool TryPeek([MaybeNullWhen(false)] out T value)
            => _queue.TryPeek(out value);

        public void TrimExcess()
            => _queue.TrimExcess();

        public Queue<T>.Enumerator GetEnumerator()
            => _queue.GetEnumerator();

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => _queue.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => _queue.GetEnumerator();
    }
}
