using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
#nullable enable
namespace System
{
    /// <summary>
    /// This class is a handy wrapper of <see cref="StringBuilder"/> class for string manipulation with minimal cost.
    /// </summary>
    public sealed class MutableString :
        IEnumerable, IEnumerable<char>,
        ICollection, ICollection<char>,
        IComparable, IComparable<string>, IComparable<MutableString>,
        IEquatable<string>, IEquatable<MutableString>
    {
        #region fields and properties
        private readonly StringBuilder _builder;

        public int Length
        {
            get => _builder.Length;
            set => _builder.Length = value;
        }

        public int Capacity
        {
            get => _builder.Capacity;
            set => _builder.Capacity = value;
        }

        public char this[int index]
        {
            get => _builder[index];
            set => _builder[index] = value;
        }

        public char this[Index index]
        {
            get => _builder[index.IsFromEnd ? Length - index.Value : index.Value];
            set => _builder[index.IsFromEnd ? Length - index.Value : index.Value] = value;
        }

        private readonly object _lock = new object();

        public string this[Range range]
        {
            get
            {
                lock (_lock)
                {
                    (int Offset, int Length) tuple = range.GetOffsetAndLength(Length);
                    return Substring(tuple.Offset, tuple.Length);
                }
            }
            set
            {
                lock (_lock)
                {
                    (int Offset, int Length) tuple = range.GetOffsetAndLength(Length);

                    EnsureRange(tuple.Offset, tuple.Length);

                    for (int index = 0; index < tuple.Length; index++, tuple.Offset++)
                        this[tuple.Offset] = value[index];
                }
            }
        }

        public int MaxCapacity
            => _builder.MaxCapacity;
        #endregion
        #region constructors
        public MutableString()
            => _builder = new StringBuilder();

        public MutableString(string? value)
            => _builder = new StringBuilder(value);

        public MutableString(int capacity, int maxCapacity)
            => _builder = new StringBuilder(capacity, maxCapacity);

        public MutableString(int capacity)
            => _builder = new StringBuilder(capacity);

        public MutableString(string? value, int capacity)
            => _builder = new StringBuilder(value, capacity);

        public MutableString(StringBuilder? builder)
            => _builder = builder ?? new StringBuilder();

        public MutableString(string? value, int startIndex, int capacity, int count)
            => _builder = new StringBuilder(value, startIndex, count, capacity);
        #endregion
        #region explicit interface implementation
        int ICollection<char>.Count
            => Length;

        bool ICollection<char>.IsReadOnly
            => false;

        int ICollection.Count
            => Length;

        bool ICollection.IsSynchronized
            => false;

        object ICollection.SyncRoot
            => this;

        IEnumerator<char> IEnumerable<char>.GetEnumerator()
            => ToString().GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator()
            => ToString().GetEnumerator();

        int IComparable.CompareTo(object obj)
            => obj switch
        {
            string str => ((IComparable<string>)this).CompareTo(str),
            MutableString mutable => ((IComparable<string>)this).CompareTo(mutable),
            _ => throw new ArgumentException(nameof(obj))
        };

        int IComparable<string>.CompareTo(string other)
            => ToString().CompareTo(other);

        int IComparable<MutableString>.CompareTo(MutableString other)
            => ((IComparable<string>)this).CompareTo(other);

        void ICollection<char>.Add(char item)
            => _builder.Append(item);

        void ICollection<char>.Clear()
            => RemoveAll();

        bool ICollection<char>.Remove(char item)
            => TryRemove(item);

        void ICollection.CopyTo(Array array, int index)
            => ToCharArray().CopyTo(array, index);
        #endregion
        #region operators
        public static bool operator ==(MutableString left, MutableString right)
            => left.Equals(right);

        public static bool operator !=(MutableString left, MutableString right)
            => !left.Equals(right);
        public static MutableString operator +(MutableString mutableString, string value)
        {
            mutableString._builder.Append(value);
            return mutableString;
        }

        public static MutableString operator +(MutableString mutableString, char value)
        {
            mutableString._builder.Append(value);
            return mutableString;
        }

        public static string operator +(object value, MutableString mutableString)
            => mutableString.Insert(0, value);

        public static MutableString operator -(MutableString mutableString, char value)
        {
            mutableString.Remove(value);
            return mutableString;
        }

        public static MutableString operator -(MutableString mutableString, string value)
        {
            mutableString.Remove(value);
            return mutableString;
        }

        public static MutableString operator *(MutableString mutableString, int value)
        {
            if (value <= 1) return mutableString;

            string str = mutableString;
            for (int x = 1; x < value; x++)
                mutableString._builder.Append(str);

            return mutableString;
        }

        public static string[] operator /(MutableString mutableString, char value)
            => mutableString.ToString().Split(value);

        public static string[] operator /(MutableString mutableString, string value)
            => mutableString.ToString().Split(value);
        #endregion
        #region conversions
        public static implicit operator string(MutableString mutableString)
            => mutableString.ToString();

        public static implicit operator MutableString(string value)
            => new MutableString(value);

        public static implicit operator ReadOnlySpan<char>(MutableString mutable)
            => mutable.ToString();

        public static explicit operator Span<char>(MutableString mutable)
            => new Span<char>(mutable.ToCharArray());

        public static explicit operator ReadOnlyMemory<char>(MutableString mutable)
            => new ReadOnlyMemory<char>(mutable.ToCharArray());

        public static explicit operator Memory<char>(MutableString mutable)
            => new Memory<char>(mutable.ToCharArray());
        #endregion
        #region instace methods
        #region override from System.Object
        public override string ToString()
            => _builder.ToString();

        public override bool Equals(object obj)
            => obj switch
        {
            MutableString mutable => Equals(mutable),
            string immutable => Equals(immutable),
            _ => false
        };

        public override int GetHashCode()
            => _builder.GetHashCode();
        #endregion
        #region Append
        public MutableString Append(char item)
        {
            _builder.Append(item);
            return this;
        }

        public MutableString Append(string? item)
        {
            _builder.Append(item);
            return this;
        }

        public MutableString Append(object? item)
        {
            _builder.Append(item);
            return this;
        }

        public MutableString AppendFormat(string? format, params object[]? args)
        {
            _builder.AppendFormat(format, args);
            return this;
        }

        public MutableString AppendFormat(IFormatProvider provider, string? format, params object[]? args)
        {
            _builder.AppendFormat(provider, format, args);
            return this;
        }

        public MutableString AppendJoin(char separator, params object[] values)
        {
            _builder.AppendJoin(separator, values);
            return this;
        }

        public MutableString AppendJoin(char separator, params string[] values)
        {
            _builder.AppendJoin(separator, values);
            return this;
        }

        public MutableString AppendJoin(string separator, params object[] values)
        {
            _builder.AppendJoin(separator, values);
            return this;
        }

        public MutableString AppendJoin(string separator, params string[] values)
        {
            _builder.AppendJoin(separator, values);
            return this;
        }

        public MutableString AppendJoin<T>(char separator, IEnumerable<T> values)
        {
            _builder.AppendJoin(separator, values);
            return this;
        }

        public MutableString AppendJoin<T>(string separator, IEnumerable<T> values)
        {
            _builder.AppendJoin(separator, values);
            return this;
        }

        public MutableString AppendLine(char item)
            => Append(item + Environment.NewLine);

        public MutableString AppendLine(string? item)
            => Append(item + Environment.NewLine);

        public MutableString AppendLine(object? item)
            => Append(item + Environment.NewLine);
        #endregion
        #region Equals
        public bool Equals(string other)
            => other.Equals(this);

        public bool Equals(MutableString other)
            => other.Equals((string)this);
        #endregion
        #region Insert
        public MutableString Insert(int index, object? value)
        {
            _builder.Insert(index, value);
            return this;
        }
        #endregion
        #region Remove
        public MutableString Remove(int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            _builder.Remove(startIndex, count);
            return this;
        }

        public MutableString RemoveAll()
        {
            _builder.Clear();
            return this;
        }

        public MutableString Remove(char item)
        {
            int index = 0;

            while ((index = IndexOf(item, index)) != -1)
                _builder.Remove(index, 1);

            return this;
        }

        public MutableString Remove(params char[]? chars)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (chars.Length == 0) return this;

            foreach (var c in chars)
                Remove(c);

            return this;
        }

        public MutableString Remove(string? item)
        {
            if (string.IsNullOrEmpty(item)) return this;

            int index = 0;

            while ((index = IndexOf(item, index)) != -1)
                _builder.Remove(index, item.Length);


            return this;
        }

        public bool TryRemove(char c)
        {
            if (!Contains(c)) return false;
            Remove(c);
            return true;
        }

        public bool TryRemove(params char[]? chars)
        {
            if (chars is null || chars.Length == 0) return false;

            int count = 0;

            foreach (var c in chars)
            {
                if (!Contains(c))
                    continue;

                Remove(c);
                count++;
            }

            return count == 0;
        }

        public bool TryRemove(string? item)
        {
            if (!Contains(item))
                return false;

            Remove(item);
            return true;
        }
        #endregion
        #region Replace
        public MutableString Replace(char oldChar, char newChar)
            => Replace(oldChar, newChar, 0, _builder.Length);

        public MutableString Replace(char oldChar, char newChar, int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            _builder.Replace(oldChar, newChar, startIndex, count);
            return this;
        }

        public MutableString Replace(string? oldStr, string? newStr)
            => Replace(oldStr, newStr, 0, _builder.Length);

        public MutableString Replace(string? oldStr, string? newStr, int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            _builder.Replace(oldStr, newStr, startIndex, count);
            return this;
        }
        #endregion
        #region Others
        public char[] ToCharArray()
            => ToString().ToCharArray(); // Tested and this is faster than creating the array manually

        public IEnumerable<char> AsEnumerable()
            => this;

        public void CopyTo(char[]? array, int arrayIndex)
            => ((ICollection)this).CopyTo(array, arrayIndex);
        #endregion
        #region Contains
        public bool Contains(char item)
        {
            for (int x = 0; x < Length; x++)
            {
                if (this[x] == item)
                    return true;
            }

            return false;
        }

        public bool Contains(string? item)
            => IndexOf(item) != -1;
        #endregion
        #region IndexOf
        public int IndexOf(char item)
            => IndexOf(item, 0, Length);

        public int IndexOf(char item, int startIndex)
            => IndexOf(item, startIndex, Length - startIndex);

        public int IndexOf(char item, int startIndex, int count)
        {
            EnsureRange(startIndex, count);

            if (count == 0) return -1;

            int index = startIndex;

            for (int counter = 1; counter <= count; counter++, index++)
            {
                if (item == this[index])
                    return index;
            }

            return -1;
        }

        public int IndexOf(string? item)
            => IndexOf(item, 0, Length);

        public int IndexOf(string? item, int startIndex)
            => IndexOf(item, startIndex, Length - startIndex);
        public int IndexOf(string? item, int startIndex, int count)
            => ((string)this).IndexOf(item, startIndex, count);

        public int[] IndexesOf(char item)
            => IndexesOf(item, 0, Length);

        public int[] IndexesOf(char item, int startIndex)
            => IndexesOf(item, startIndex, Length - startIndex);

        public int[] IndexesOf(char value, int startIndex, int count)
        {
            EnsureRange(startIndex, count);

            if (count == 0) return new int[0];

            var currentIndex = IndexOf(value, startIndex, count);
            var result = new List<int>(Length);

            while (currentIndex != -1)
            {
                result.Add(currentIndex);
                currentIndex = IndexOf(value, currentIndex + 1);
            }

            return result.ToArray();
        }

        public int IndexOfAny(params char[]? chars)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (chars.Length == 0) return -1;

            foreach (var c in chars)
            {
                var index = IndexOf(c);
                if (index != -1) return index;
            }

            return -1;
        }


        public int LastIndexOf(char item)
            => LastIndexOf(item, 0, Length);

        public int LastIndexOf(char item, int startIndex)
            => LastIndexOf(item, startIndex, Length - startIndex);

        public int LastIndexOf(char item, int startIndex, int count)
        {
            EnsureRange(startIndex, count);

            if (count == 0) return -1;

            int index = Length - 1;

            for (int counter = 1; counter <= count; counter++, index--)
            {
                if (item == this[index])
                    return index;
            }

            return -1;
        }

        public int LastIndexOfAny(params char[]? chars)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (chars.Length == 0) return -1;

            foreach (var c in chars)
            {
                var index = LastIndexOf(c);
                if (index != -1) return index;
            }

            return -1;
        }
        #endregion
        #region StartsWith
        public bool StartsWith(char value)
            => value == this[0];

        public bool EndsWith(char value)
            => value == this[Length - 1];

        public bool StartsWith(string? value)
        {
            if (value is null || value.Length > Length) return false;
            if (value.Length == 0) return Length == 0;
            if (value.Length == Length) return value == this;

            for (int index = 0; index < value.Length; index++)
            {
                if (value[index] != this[index])
                    return false;
            }

            return true;
        }

        public bool EndsWith(string? value)
        {
            if (value is null || value.Length > Length) return false;
            if (value.Length == 0) return Length == 0;
            if (value.Length == Length) return value == this;

            for (int index = Length - value.Length; index < Length; index++)
            {
                if (value[index] != this[index])
                    return false;
            }

            return true;
        }
        #endregion
        #region Split
        public MutableString[] Split(char separator)
            => (this / separator).Select(SystemUtilities.ToMutable).ToArray();

        public MutableString[] Split(string separator)
            => (this / separator).Select(SystemUtilities.ToMutable).ToArray();

        public MutableString[] Split(char separator, StringSplitOptions options)
            => ((string)this).Split(separator, options).Select(SystemUtilities.ToMutable).ToArray();

        public MutableString[] Split(params char[] separator)
            => ((string)this).Split(separator).Select(SystemUtilities.ToMutable).ToArray();

        public MutableString[] Split(string[] separator, StringSplitOptions options)
            => ((string)this).Split(separator, options).Select(SystemUtilities.ToMutable).ToArray();
        #endregion
        #region Padding
        public MutableString PadLeft(int totalWidth)
            => PadLeft(totalWidth, ' ');

        public MutableString PadLeft(int totalWidth, char paddingChar)
            => totalWidth <= Length ? this : Insert(0, new string(paddingChar, totalWidth - Length));

        public MutableString PadRight(int totalWidth)
            => PadRight(totalWidth, ' ');

        public MutableString PadRight(int totalWidth, char paddingChar)
            => totalWidth <= Length ? this : Insert(Length, new string(paddingChar, totalWidth - Length));
        #endregion
        #region Substring
        public string Substring(int startIndex, int count)
        {
            EnsureRange(startIndex, count);

            var chars = new char[count];
            var index = 0;

            for (int counter = 1; counter <= count; counter++, index++, startIndex++)
                chars[index] = this[startIndex];

            return new string(chars);
        }
        #endregion
        #region Slice
        public MutableString Slice(int startIndex)
            => Slice(startIndex, Length - startIndex); // I don't want to needlessly call range overload

        public MutableString Slice(Range range)
        {
            (int Offset, int Length) tuple = range.GetOffsetAndLength(Length);
            return Slice(tuple.Offset, tuple.Length);
        }

        public MutableString Slice(int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            
            if (startIndex > 0) _builder.Remove(0, startIndex);
            _builder.Remove(count, Length - count);

            return this;
        }
        #endregion
        #endregion
        #region static methods
        public static bool IsNullOrEmpty(MutableString? input)
            => input is null || input.Length == 0;

        public static bool IsNullOrWhiteSpace(MutableString? input)
            => IsNullOrEmpty(input) || input.All(char.IsWhiteSpace);

        private void EnsureRange(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex > Length || startIndex + count > Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        }

        private void EnsureRange(Range range)
        {
            (int Offset, int Length) tuple = range.GetOffsetAndLength(Length);
            EnsureRange(tuple.Offset, tuple.Length);
        }
        #endregion
    }
}
