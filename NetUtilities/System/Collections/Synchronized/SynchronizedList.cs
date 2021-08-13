using System.Diagnostics.CodeAnalysis;
using NetUtilities;

namespace System.Collections.Synchronized
{
    /// <summary>
    ///     Provides a synchronized implementation of <see cref="List{T}"/>.
    /// </summary>
    public sealed class SynchronizedList<T> : IList<T>, ICloneable<SynchronizedList<T>>
    {
        internal readonly List<T> _list;

        /// <summary>
        ///     Gets or sets the element at the specified index.
        /// </summary>
        /// <param name="index">
        ///     The index.
        /// </param>
        /// <returns>
        ///     The element at the specified <paramref name="index"/>.
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Throw when the <paramref name="index"/> is negative or higher than or equal to <see cref="Count"/>.
        /// </exception>
        public T this[Index index]
        {
            [return: MaybeNull]
            get
            {
                lock (_list)
                    return _list[index];
            }
            set
            {
                lock (_list)
                    _list[index] = value!;
            }
        }

        /// <summary>
        ///     Creates a shallow copy of a range of elements in the source <see cref="SynchronizedList{T}"/>.
        /// </summary>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     A shallow copy of a range of elements in the source <see cref="SynchronizedList{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown when index is less than 0 or higher than or equal to <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when index and count do not denote a valid range of elements.
        /// </exception>
        [AllowNull]
        public SynchronizedList<T> this[Range range]
        {
            get
            {
                lock (_list)
                {
                    var (offset, length) = range.GetOffsetAndLength(Count);
                    return new SynchronizedList<T>(_list.GetRange(offset, length));
                }
            }
        }

        /// <inheritdoc/>
        public T this[int index]
        {
            [return: MaybeNull]
            get
            {
                lock (_list)
                    return _list[index];
            }
            set
            {
                lock (_list)
                    _list[index] = value!;
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                lock (_list)
                    return _list.Count;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
            => false;

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizedList{T}"/> class that is empty 
        ///     and has the default initial capacity.
        /// </summary>
        public SynchronizedList()
        {
            _list = new List<T>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizedList{T}"/> class that is empty 
        ///     and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">
        ///     The capacity.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when capacity is less than 0.
        /// </exception>
        public SynchronizedList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizedList{T}"/> class that 
        ///     references the provided <see cref="List{T}"/>.
        /// </summary>
        /// <param name="source">
        ///     The source list.
        /// </param>
        public SynchronizedList(List<T> source) : this(source, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="SynchronizedList{T}"/> class that 
        ///     optionally references the provided <see cref="List{T}"/>.
        /// </summary>
        /// <param name="source">
        ///     The source list.
        /// </param>
        /// <param name="keepReference">
        ///     Indicates if the provided list referenced should be kept.
        /// </param>
        public SynchronizedList(List<T> source, bool keepReference)
        {
            Ensure.NotNull(source);

            _list = keepReference
                ? source
                : source.ToList();
        }

        /// <summary>
        /// Creates a <see cref="SynchronizedList{T}"/> from the source <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source"></param>
        public SynchronizedList(IEnumerable<T> source)
        {
            Ensure.NotNull(source);

            _list = source.ToList();
        }

        /// <inheritdoc/>
        public void Add([AllowNull] T item)
        {
            lock (_list)
                _list.Add(item!);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            lock (_list)
                _list.Clear();
        }

        /// <inheritdoc/>
        public bool Contains([AllowNull] T item)
        {
            lock (_list)
                return _list.Contains(item!);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_list)
                _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="SynchronizedList{T}"/>.
        /// </summary>
        /// <returns> 
        ///     An enumerator that iterates through the <see cref="SynchronizedList{T}"/>.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            lock (_list)
                return new Enumerator(Clone());
        }

        /// <inheritdoc/>
        public int IndexOf([AllowNull] T item)
        {
            lock (_list)
                return _list.IndexOf(item!);
        }

        /// <inheritdoc/>
        public void Insert(int index, [AllowNull] T item)
        {
            lock (_list)
                _list.Insert(index, item!);
        }

        /// <inheritdoc/>
        public bool Remove([AllowNull] T item)
        {
            lock (_list)
                return _list.Remove(item!);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            lock (_list)
                _list.RemoveAt(index);
        }

        /// <inheritdoc/>
        public SynchronizedList<T> Clone()
        {
            lock (_list)
                return new SynchronizedList<T>(_list, false);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            lock (_list)
                return GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            lock (_list)
                return GetEnumerator();
        }

        /// <summary>
        ///     Enumerates the elements of a <see cref="SynchronizedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the <see cref="SynchronizedList{T}"/>.
        /// </typeparam>
        public readonly struct Enumerator : IEnumerator<T> // To keep thread safety, this struct requires a clone of the source list. Making it quite expensive.
        {
            private readonly List<T>.Enumerator _enumerator;

            /// <inheritdoc/>
            public T Current
                => _enumerator.Current;

            /// <summary>
            ///     Initializes a new instance of a <see cref="SynchronizedList{T}.Enumerator"/> 
            ///     with the provided <see cref="SynchronizedList{T}"/>.
            /// </summary>
            /// <param name="list">
            ///     The source list.
            /// </param>
            public Enumerator(SynchronizedList<T> list)
            {
                _enumerator = list._list.GetEnumerator();
            }

            /// <inheritdoc/>
            public bool MoveNext()
                => _enumerator.MoveNext();

            /// <inheritdoc/>
            public void Dispose()
                => _enumerator.Dispose();

            object? IEnumerator.Current
                => Current;

            void IEnumerator.Reset()
            {
            }
        }
    }
}
