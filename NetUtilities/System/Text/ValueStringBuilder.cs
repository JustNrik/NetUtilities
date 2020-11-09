using System;
using System.Buffers;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace System.Text
{
    public ref struct ValueStringBuilder
    {
        private char[]? _arrayToReturnToPool;
        private Span<char> _span;
        private int _index;

        public int Length
        {
            get => _index;
            set => _index = value;
        }

        public int Capacity
            => _span.Length;

        public ValueStringBuilder(Span<char> initialBuffer)
        {
            _arrayToReturnToPool = null;
            _span = initialBuffer;
            _index = 0;
        }

        public ValueStringBuilder(int initialCapacity)
        {
            _arrayToReturnToPool = ArrayPool<char>.Shared.Rent(initialCapacity);
            _span = _arrayToReturnToPool;
            _index = 0;
        }

        public void EnsureCapacity(int capacity)
        {
            if (capacity > _span.Length)
                Grow(capacity - _index);
        }

        public ref char GetPinnableReference()
            => ref MemoryMarshal.GetReference(_span);

        public ref char GetPinnableReference(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _span[Length] = '\0';
            }

            return ref MemoryMarshal.GetReference(_span);
        }

        public ref char this[int index]
            => ref _span[index];

        public override string ToString()
        {
            var result = _span[.._index].ToString();

            Dispose();
            return result;
        }

        public Span<char> RawChars 
            => _span;

        public ReadOnlySpan<char> AsSpan(bool terminate)
        {
            if (terminate)
            {
                EnsureCapacity(Length + 1);
                _span[Length] = '\0';
            }

            return _span[.._index];
        }

        public ReadOnlySpan<char> AsSpan() 
            => _span[.._index];

        public ReadOnlySpan<char> AsSpan(int start) 
            => _span[start.._index];

        public ReadOnlySpan<char> AsSpan(int start, int length) 
            => _span.Slice(start, length);

        public bool TryCopyTo(Span<char> destination, out int charsWritten)
        {
            if (_span[.._index].TryCopyTo(destination))
            {
                charsWritten = _index;
                Dispose();
                return true;
            }
            else
            {
                charsWritten = 0;
                Dispose();
                return false;
            }
        }

        public void Insert(int index, char value, int count)
        {
            if (_index > _span.Length - count)
                Grow(count);

            _span[index.._index].CopyTo(_span[(index + count)..]);
            _span.Slice(index, count).Fill(value);
            _index += count;
        }

        public void Insert(int index, string? s)
        {
            if (s is null)
                return;

            var count = s.Length;

            if (_index > (_span.Length - count))
                Grow(count);

            _span[index.._index].CopyTo(_span[(index + count)..]);
            s.AsSpan().CopyTo(_span[index..]);
            _index += count;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(char c)
        {
            var pos = _index;

            if ((uint)pos < (uint)_span.Length)
            {
                _span[pos] = c;
                _index = pos + 1;
            }
            else
                GrowAndAppend(c);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Append(string? s)
        {
            if (s is null)
                return;

            var pos = _index;

            if (s.Length == 1 && (uint)pos < (uint)_span.Length)
            {
                _span[pos] = s[0];
                _index = pos + 1;
            }
            else
                AppendSlow(s);
        }

        private void AppendSlow(string s)
        {
            var pos = _index;

            if (pos > _span.Length - s.Length)
                Grow(s.Length);

            s.AsSpan().CopyTo(_span[pos..]);
            _index += s.Length;
        }

        public void Append(char c, int count)
        {
            if (_index > _span.Length - count)
                Grow(count);

            var dst = _span.Slice(_index, count);

            for (int i = 0; i < dst.Length; i++)
                dst[i] = c;

            _index += count;
        }

        public unsafe void Append(char* value, int length)
        {
            var pos = _index;

            if (pos > _span.Length - length)
                Grow(length);

            var dst = _span.Slice(_index, length);

            for (int i = 0; i < dst.Length; i++)
                dst[i] = *value++;

            _index += length;
        }

        public void Append(ReadOnlySpan<char> value)
        {
            var pos = _index;

            if (pos > _span.Length - value.Length)
                Grow(value.Length);

            value.CopyTo(_span[_index..]);
            _index += value.Length;
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public Span<char> AppendSpan(int length)
        {
            var origPos = _index;

            if (origPos > _span.Length - length)
                Grow(length);

            _index = origPos + length;
            return _span.Slice(origPos, length);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void GrowAndAppend(char c)
        {
            Grow(1);
            Append(c);
        }

        [MethodImpl(MethodImplOptions.NoInlining)]
        private void Grow(int additionalCapacityBeyondPos)
        {
            var poolArray = ArrayPool<char>.Shared.Rent(Math.Max(_index + additionalCapacityBeyondPos, _span.Length * 2));

            _span[.._index].CopyTo(poolArray);

            var toReturn = _arrayToReturnToPool;

            _span = _arrayToReturnToPool = poolArray;

            if (toReturn != null)
                ArrayPool<char>.Shared.Return(toReturn);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Dispose()
        {
            var toReturn = _arrayToReturnToPool;
            this = default;

            if (toReturn is not null)
                ArrayPool<char>.Shared.Return(toReturn);
        }
    }
}
