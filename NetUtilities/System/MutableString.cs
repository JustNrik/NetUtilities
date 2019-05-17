﻿using System.Collections;
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

        public string this[Range range]
            => ToString()[range!];

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

        public MutableString(string value, int startIndex, int capacity, int length)
            => _builder = new StringBuilder(value, startIndex, length, capacity);
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
        public override string ToString()
            => _builder.ToString();

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

        public MutableString AppendLine(char item)
            => Append(item + Environment.NewLine);

        public MutableString AppendLine(string? item)
            => Append(item + Environment.NewLine);

        public MutableString AppendLine(object? item)
            => Append(item + Environment.NewLine);

        public bool Equals(string other)
            => other.Equals(this);

        public bool Equals(MutableString other)
            => ((string)other).Equals(this);

        public override bool Equals(object obj)
            => obj switch
            {
                string str => Equals(str),
                MutableString mutable => Equals(mutable),
                _ => false
            };

        public override int GetHashCode()
            => _builder.GetHashCode();

        public MutableString Insert(int index, object? value)
        {
            _builder.Insert(index, value);
            return this;
        }

        public MutableString Remove(int startIndex, int length)
        {
            _builder.Remove(startIndex, length);
            return this;
        }

        public MutableString Replace(char oldChar, char newChar)
            => Replace(oldChar, newChar, 0, _builder.Length);

        public MutableString Replace(char oldChar, char newChar, int startIndex, int count)
        {
            _builder.Replace(oldChar, newChar, startIndex, count);
            return this;
        }

        public MutableString Replace(string? oldStr, string? newStr)
            => Replace(oldStr, newStr, 0, _builder.Length);

        public MutableString Replace(string? oldStr, string? newStr, int startIndex, int count)
        {
            _builder.Replace(oldStr, newStr, startIndex, count);
            return this;
        }

        public MutableString AppendFormat(string? format, params object[]? args)
        {
            _builder.AppendFormat(format, args);
            return this;
        }

        public char[] ToCharArray() 
            => _builder.ToString().ToCharArray();

        public IEnumerable<char> AsEnumerable()
            => this;

        public MutableString RemoveAll()
        {
            _builder.Clear();
            return this;
        }

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

        public int IndexOf(char item)
            => IndexOf(item, 0, Length);

        public int IndexOf(char item, int startIndex)
            => IndexOf(item, startIndex, Length - startIndex);

        public int IndexOf(char item, int startIndex, int length)
        {
            if (startIndex >= Length || 
                startIndex + length > Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 0) return -1;

            int index = startIndex;

            for (int counter = 1; counter <= length; counter++, index++)
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

        public void CopyTo(char[]? array, int arrayIndex)
            => ((ICollection)this).CopyTo(array, arrayIndex);

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

        public int LastIndexOf(char item, int startIndex, int length)
        {
            if (startIndex >= Length || 
                startIndex + length >= Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (length < 0) throw new ArgumentOutOfRangeException(nameof(length));
            if (length == 0) return -1;

            int index = Length - 1;

            for (int counter = 1; counter <= length; counter++, index--)
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

        public bool StartsWith(char value)
            => value == this[0];
        public bool EndsWith(char value)
            => value == this[Length - 1];
        public bool StartsWith(string? value)
        {
            if (string.IsNullOrEmpty(value)) return IsNullOrEmpty(this);
            if (value.Length > Length) return false;
            if (value.Length > Length) return value == this;

            for (int index = 0; index < value.Length; index++)
            {
                if (value[index] != this[index])
                    return false;
            }

            return true;
        }

        public bool EndsWith(string? value)
        {
            if (string.IsNullOrEmpty(value)) return IsNullOrEmpty(this);
            if (value.Length > Length) return false;
            if (value.Length == Length) return value == this;

            for (int index = Length - value.Length; index < Length; index++)
            {
                if (value[index] != this[index])
                    return false;
            }

            return true;
        }

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

        public MutableString PadLeft(int totalWidth)
            => PadLeft(totalWidth, ' ');

        public MutableString PadLeft(int totalWidth, char paddingChar)
            => totalWidth <= Length ? this : Insert(0, new string(paddingChar, totalWidth - Length));

        public MutableString PadRight(int totalWidth)
            => PadRight(totalWidth, ' ');

        public MutableString PadRight(int totalWidth, char paddingChar)
            => totalWidth <= Length ? this : Insert(Length, new string(paddingChar, totalWidth - Length));
        #endregion
        #region static methods
        public static bool IsNullOrEmpty(MutableString? input)
            => input is null || input.Length == 0;
        #endregion
    }
}
