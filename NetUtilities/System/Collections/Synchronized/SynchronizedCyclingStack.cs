using System.Diagnostics.CodeAnalysis;

namespace System.Collections.Synchronized
{
    public sealed class SynchronizedCyclingQueue<T> : IReadOnlyCollection<T>
    {
        private readonly Queue<T> _queue;

        public int Count
        {
            get
            {
                lock (_queue)
                    return _queue.Count;
            }
        }

        public SynchronizedCyclingQueue()
        {
            _queue = new();
        }

        public SynchronizedCyclingQueue(int capacity)
        {
            _queue = new(capacity);
        }

        public SynchronizedCyclingQueue(IEnumerable<T> source)
        {
            _queue = new(source);
        }

        public void Enqueue(T item)
        {
            lock (_queue)
                _queue.Enqueue(item);
        }

        public T Dequeue()
        {
            lock (_queue)
            {
                var item = _queue.Dequeue();

                _queue.Enqueue(item);
                return item;
            }
        }

        public void Clear()
        {
            lock (_queue)
                _queue.Clear();
        }

        public T Peek()
        {
            lock (_queue)
                return _queue.Peek();
        }

        public bool TryDequeue([MaybeNullWhen(false)] out T value)
        {
            lock (_queue)
            {
                var dequeued = _queue.TryDequeue(out value);

                if (dequeued)
                    _queue.Enqueue(value!);

                return dequeued;
            }
        }

        public bool TryPeek([MaybeNullWhen(false)] out T value)
        {
            lock (_queue)
                return  _queue.TryPeek(out value);
            
        }

        public void TrimExcess()
        {
            lock (_queue)
                _queue.TrimExcess(); 
        }

        public Queue<T>.Enumerator GetEnumerator()
        {
            lock (_queue)
                return _queue.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();
    }
}
