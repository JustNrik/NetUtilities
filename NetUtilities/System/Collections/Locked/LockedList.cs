using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using NetUtilities;

namespace System.Collections.Locked
{
    /// <summary>
    ///     Provides a synchronized implementation of <see cref="List{T}"/>.
    /// </summary>
    public sealed class LockedList<T> : IList<T>, ICloneable<LockedList<T>>
    {
        internal readonly List<T> _list;
        private readonly object _lock = new object();

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
                lock (_lock)
                    return _list[index];
            }
            set
            {
                lock (_lock)
                    _list[index] = value!;
            }
        }

        /// <summary>
        ///     Creates a shallow copy of a range of elements in the source <see cref="LockedList{T}"/>.
        /// </summary>
        /// <param name="range">
        ///     The range.
        /// </param>
        /// <returns>
        ///     A shallow copy of a range of elements in the source <see cref="LockedList{T}"/>.
        /// </returns>
        /// <exception cref="ArgumentException">
        ///     Thrown when index is less than 0 or higher than or equal to <see cref="Count"/>.
        /// </exception>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when index and count do not denote a valid range of elements.
        /// </exception>
        [AllowNull]
        public LockedList<T> this[Range range]
        {
            get
            {
                lock (_lock)
                {
                    var (offset, length) = range.GetOffsetAndLength(Count);
                    return new LockedList<T>(_list.GetRange(offset, length));
                }
            }
        }

        /// <inheritdoc/>
        public T this[int index]
        {
            [return: MaybeNull]
            get
            {
                lock (_lock)
                    return _list[index];
            }
            set
            {
                lock (_lock)
                    _list[index] = value!;
            }
        }

        /// <inheritdoc/>
        public int Count
        {
            get
            {
                lock (_lock)
                    return _list.Count;
            }
        }

        /// <inheritdoc/>
        public bool IsReadOnly
            => false;

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockedList{T}"/> class that is empty 
        ///     and has the default initial capacity.
        /// </summary>
        public LockedList()
        {
            _list = new List<T>();
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockedList{T}"/> class that is empty 
        ///     and has the specified initial capacity.
        /// </summary>
        /// <param name="capacity">
        ///     The capacity.
        /// </param>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when capacity is less than 0.
        /// </exception>
        public LockedList(int capacity)
        {
            _list = new List<T>(capacity);
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockedList{T}"/> class that 
        ///     references the provided <see cref="List{T}"/>.
        /// </summary>
        /// <param name="source">
        ///     The source list.
        /// </param>
        public LockedList(List<T> source) : this(source, true)
        {
        }

        /// <summary>
        ///     Initializes a new instance of the <see cref="LockedList{T}"/> class that 
        ///     optionally references the provided <see cref="List{T}"/>.
        /// </summary>
        /// <param name="source">
        ///     The source list.
        /// </param>
        /// <param name="keepReference">
        ///     Indicates if the provided list referenced should be kept.
        /// </param>
        public LockedList(List<T> source, bool keepReference)
        {
            Ensure.NotNull(source);

            _list = keepReference 
                ? source 
                : source.ToList();
        }

        /// <summary>
        /// Creates a <see cref="LockedList{T}"/> from the source <see cref="IEnumerable{T}"/>.
        /// </summary>
        /// <param name="source"></param>
        public LockedList(IEnumerable<T> source)
        {
            Ensure.NotNull(source);

            _list = source.ToList();
        }

        /// <inheritdoc/>
        public void Add([AllowNull]T item)
        {
            lock (_lock)
                _list.Add(item!);
        }

        /// <inheritdoc/>
        public void Clear()
        {
            lock (_lock)
                _list.Clear();
        }

        /// <inheritdoc/>
        public bool Contains([AllowNull]T item)
        {
            lock (_lock)
                return _list.Contains(item!);
        }

        /// <inheritdoc/>
        public void CopyTo(T[] array, int arrayIndex)
        {
            lock (_lock)
                _list.CopyTo(array, arrayIndex);
        }

        /// <summary>
        ///     Returns an enumerator that iterates through the <see cref="LockedList{T}"/>.
        /// </summary>
        /// <returns> 
        ///     An enumerator that iterates through the <see cref="LockedList{T}"/>.
        /// </returns>
        public Enumerator GetEnumerator()
        {
            lock (_lock)
                return new Enumerator(Clone());
        }

        /// <inheritdoc/>
        public int IndexOf([AllowNull]T item)
        {
            lock (_lock)
                return _list.IndexOf(item!);
        }

        /// <inheritdoc/>
        public void Insert(int index, [AllowNull]T item)
        {
            lock (_lock)
                _list.Insert(index, item!);
        }

        /// <inheritdoc/>
        public bool Remove([AllowNull]T item)
        {
            lock (_lock)
                return _list.Remove(item!);
        }

        /// <inheritdoc/>
        public void RemoveAt(int index)
        {
            lock (_lock)
                _list.RemoveAt(index);
        }

        /// <inheritdoc/>
        public LockedList<T> Clone()
        {
            lock (_lock)
                return new LockedList<T>(_list, false);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
            => GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        /// <summary>
        ///     Enumerates the elements of a <see cref="LockedList{T}"/>.
        /// </summary>
        /// <typeparam name="T">
        ///     The underlying type of the <see cref="LockedList{T}"/>.
        /// </typeparam>
        public readonly struct Enumerator : IEnumerator<T> // To keep thread safety, this struct requires a clone of the source list. Making it quite expensive.
        {
            private readonly List<T>.Enumerator _enumerator;

            /// <inheritdoc/>
            public T Current
                => _enumerator.Current;

            /// <summary>
            ///     Initializes a new instance of a <see cref="LockedList{T}.Enumerator"/> 
            ///     with the provided <see cref="LockedList{T}"/>.
            /// </summary>
            /// <param name="list">
            ///     The source list.
            /// </param>
            public Enumerator(LockedList<T> list)
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
