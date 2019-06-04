using JetBrains.Annotations;
using NetUtilities;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
#nullable enable
namespace System
{
    /// <summary>
    /// This class is a handy wrapper of <see cref="StringBuilder"/> class for string manipulation with minimal cost.
    /// </summary>
    public sealed class MutableString :
        IEnumerable, IEnumerable<char>,
        ICollection, ICollection<char>,
        IList<char>,
        IComparable, IComparable<string>, IComparable<MutableString>,
        IEquatable<string>, IEquatable<MutableString>
    {
        #region fields and properties
        private readonly StringBuilder _builder;
        private string _current = string.Empty;
        private bool _isModified = true;

        /// <summary>
        /// Gets or sets the Length of the builder in the current instance.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"/>
        public int Length
        {
            get => _builder.Length;
            set
            {
                if (value == _builder.Length)
                    return;

                _builder.Length = value;
                _isModified = true;
            }
        }

        /// <summary>
        /// Gets or sets the Capacity of the builder in the current instance.
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException"/>
        public int Capacity
        {
            get => _builder.Capacity;
            set => _builder.Capacity = value;
        }

        /// <summary>
        /// Gets or sets a <see cref="char"/> in the specified <see cref="Index"/>.
        /// </summary>
        /// <param name="index"/>
        /// <returns/>
        public char this[Index index]
        {
            get => _builder[index.GetOffset(Length)];
            set
            {
                _builder[index.GetOffset(Length)] = value;
                _isModified = true;
            }
        }

        /// <summary>
        /// Gets or sets a <see cref="char"/> in the specified index.
        /// </summary>
        /// <param name="index"/>
        /// <returns/>
        public char this[int index]
        {
            get => _builder[index];
            set => _builder[index] = value;
        }

        /// <summary>
        /// Gets or sets a <see cref="string"/> in the specified <see cref="Range"/>.
        /// The input string must have the same length of the string you want to edit, otherwise an <see cref="IndexOutOfRangeException"/> will be thrown.
        /// </summary>
        /// <exception cref="IndexOutOfRangeException"/>
        /// <param name="range"/>
        /// <returns/>
        public string this[Range range]
        {
            get => Substring(range);
            set
            {
                var (startIndex, count) = range.GetOffsetAndLength(Length);

                EnsureRange(startIndex, count);
                EnsureLength(value.Length, count);

                for (int index = 0; index < count; index++, startIndex++)
                    this[startIndex] = value[index];

                _isModified = true;
            }
        }

        private static void EnsureLength(int length, int count)
        {
            if (length != count)
                throw new InvalidOperationException("You cannot assign a string with a length bigger than the range you provided. Use Insert instead.");
        }

        /// <summary>
        /// Returns a <see cref="MatchCollection"/> with the provided pattern.
        /// </summary>
        /// <param name="pattern"/>
        /// <param name="options"/>
        /// <returns/>
        public MatchCollection this[[RegexPattern]string? pattern, RegexOptions options = RegexOptions.None]
            => GetMatches(pattern, options);

        /// <summary>
        /// Returns a <see cref="string"/> with the replacement applied on the pattern given. This method doesn't mutate the current instance.
        /// </summary>
        /// <param name="pattern"/>
        /// <param name="replacement"/>
        /// <param name="options"/>
        /// <returns/>
        public string this[[RegexPattern]string pattern, string replacement, RegexOptions options = RegexOptions.None]
            => Regex.Replace(this, pattern, replacement, options);

        /// <summary>
        /// Returns a <see cref="string"/>[] delimited with the characters provided. An empty array if not found. 
        /// You can optionally provide the start index and whether to include the bounds or not.
        /// </summary>
        /// <param name="leftBound"/>
        /// <param name="rightBound"/>
        /// <param name="startIndex"/>
        /// <param name="includeBounds"/>
        /// <returns/>
        public string[] this[char leftBound, char rightBound, int startIndex = 0, bool includeBounds = false]
            => this.SubstringsBetween(leftBound, rightBound, startIndex, includeBounds);

        /// <summary>
        /// Returns the maximun capacity of the builder.
        /// </summary>
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
            => new MutableStringEnumerator(this);

        IEnumerator IEnumerable.GetEnumerator()
            => new MutableStringEnumerator(this);

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

        void IList<char>.Insert(int index, char item)
            => Insert(index, item);

        void IList<char>.RemoveAt(int index)
            => RemoveAt(index);
        #endregion
        #region operators
        public static bool operator ==(MutableString left, MutableString right)
            => left.Equals(right); // I know this will throw null ref, I won't fix it. use is null instead like a sane person.

        public static bool operator !=(MutableString left, MutableString right)
            => !left.Equals(right); // I know this will throw null ref, I won't fix it. use is null instead like a sane person.

        public static MutableString operator +(MutableString mutableString, string value)
            => mutableString.Append(value);

        public static MutableString operator +(MutableString mutableString, char value)
            => mutableString.Append(value);

        public static string operator +(object value, MutableString mutableString)
            => mutableString.Insert(0, value);

        public static MutableString operator -(MutableString mutableString, char value)
             => mutableString.Remove(value);


        public static MutableString operator -(MutableString mutableString, string value)
             => mutableString.Remove(value);

        public static MutableString operator *(MutableString mutableString, int value)
        {
            if (value <= 1) return mutableString;

            string str = mutableString;
            for (int x = 1; x < value; x++)
                mutableString.Append(str);

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
        {
            if (_isModified)
            {
                _current = _builder.ToString();
                _isModified = false;
            }

            return _current;
        }

        public override bool Equals(object? obj)
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
            _isModified = true;
            return this;
        }

        public MutableString Append(string? item)
        {
            _builder.Append(item);
            _isModified = true;
            return this;
        }

        public MutableString Append(object? item)
        {
            _builder.Append(item);
            _isModified = true;
            return this;
        }

        public MutableString AppendFormat(string? format, params object[]? args)
        {
            _builder.AppendFormat(format, args);
            _isModified = true;
            return this;
        }

        public MutableString AppendFormat(IFormatProvider? provider, string? format, params object[]? args)
        {
            _builder.AppendFormat(provider, format, args);
            _isModified = true;
            return this;
        }

        public MutableString AppendJoin(char separator, params object[]? values)
        {
            _builder.AppendJoin(separator, values);
            _isModified = true;
            return this;
        }

        public MutableString AppendJoin(char separator, params string[]? values)
        {
            _builder.AppendJoin(separator, values);
            _isModified = true;
            return this;
        }

        public MutableString AppendJoin(string separator, params object[]? values)
        {
            _builder.AppendJoin(separator, values);
            _isModified = true;
            return this;
        }

        public MutableString AppendJoin(string separator, params string[]? values)
        {
            _builder.AppendJoin(separator, values);
            _isModified = true;
            return this;
        }

        public MutableString AppendJoin<T>(char separator, IEnumerable<T>? values)
        {
            _builder.AppendJoin(separator, values);
            _isModified = true;
            return this;
        }

        public MutableString AppendJoin<T>(string? separator, IEnumerable<T>? values)
        {
            _builder.AppendJoin(separator, values);
            _isModified = true;
            return this;
        }

        public MutableString AppendLine(char item)
        {
            _isModified = true;
            return Append(item + Environment.NewLine);
        }

        public MutableString AppendLine(string? item)
        {
            _isModified = true;
            return Append(item + Environment.NewLine);
        }

        public MutableString AppendLine(object? item)
        {
            _isModified = true;
            return Append(item + Environment.NewLine);
        }
        #endregion
        #region Equals
        public bool Equals(string other)
            => other?.Equals(this) ?? false;

        public bool Equals(MutableString other)
            => ((string)other)?.Equals(this) ?? false;
        #endregion
        #region Insert
        public MutableString Insert(int index, object? value)
        {
            _builder.Insert(index, value);
            _isModified = true;
            return this;
        }

        public MutableString Insert(int index, string? value)
        {
            _builder.Insert(index, value);
            _isModified = true;
            return this;
        }

        public MutableString Insert(int index, char value)
        {
            _builder.Insert(index, value);
            _isModified = true;
            return this;
        }

        public MutableString Insert(int index, ReadOnlySpan<char> value)
        {
            _builder.Insert(index, value);
            _isModified = true;
            return this;
        }

        public MutableString Insert(int index, string? value, int count)
        {
            _builder.Insert(index, value, count);
            _isModified = true;
            return this;
        }

        public MutableString Insert(int index, char[]? value, int startIndex, int count)
        {
            _builder.Insert(index, value, startIndex, count);
            _isModified = true;
            return this;
        }
        #endregion
        #region Remove
        public MutableString Remove(Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Remove(startIndex, count);
        }

        public MutableString RemoveAt(int index)
            => Remove(index, 1);

        public MutableString RemoveAt(Index index)
            => Remove(index.GetOffset(Length), 1);

        public MutableString Remove(int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            _builder.Remove(startIndex, count);
            _isModified = true;
            return this;
        }

        public MutableString RemoveAll()
        {
            _builder.Clear();
            _isModified = true;
            return this;
        }

        public MutableString Remove(char item)
        {
            int index = 0;

            while ((index = IndexOf(item, index)) != -1)
                _builder.Remove(index, 1);

            _isModified = true;

            return this;
        }

        public MutableString Remove(params char[]? chars)
        {
            Ensure.NotNull(chars, nameof(chars));
            if (chars!.Length == 0) return this;

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

            _isModified = true;

            return this;
        }

        public bool TryRemove(char c)
        {
            Remove(c);
            return _isModified;
        }

        public bool TryRemove(params char[]? chars)
        {
            if (chars is null || chars.Length == 0) return false;

            foreach (var c in chars)
                Remove(c);

            return _isModified;
        }

        public bool TryRemove(string? item)
        {
            Remove(item);
            return _isModified;
        }
        #endregion
        #region Replace
        public MutableString Replace(char oldChar, char newChar)
            => Replace(oldChar, newChar, 0, _builder.Length);

        public MutableString Replace(char oldChar, char newChar, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Replace(oldChar, newChar, startIndex, count);
        }

        public MutableString Replace(char oldChar, char newChar, int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            _builder.Replace(oldChar, newChar, startIndex, count);
            _isModified = true;
            return this;
        }

        public MutableString Replace(string? oldStr, string? newStr)
            => Replace(oldStr, newStr, 0, _builder.Length);

        public MutableString Replace(string? oldStr, string? newStr, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Replace(oldStr, newStr, startIndex, count);
        }

        public MutableString Replace(string? oldStr, string? newStr, int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            _builder.Replace(oldStr, newStr, startIndex, count);
            _isModified = true;
            return this;
        }
        #endregion
        #region Reverse
        /// <summary>
        /// Reverse the current <see cref="MutableString"/>
        /// </summary>
        /// <returns></returns>
        public MutableString Reverse()
            => Reverse(0, Length - 1);

        public MutableString Reverse(int startIndex)
            => Reverse(startIndex, Length - startIndex - 1);

        public MutableString Reverse(int startIndex, int count)
        {
            EnsureRange(startIndex, count);

            for (int begin = startIndex, end = startIndex + count; begin < end; begin++, end--)
            {
                var beginChar = this[begin];
                var endChar = this[end];
                this[begin] = endChar;
                this[end] = beginChar;
            }

            return this;
        }

        public MutableString Reverse(Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Reverse(startIndex, count);
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
            => Contains(item, 0, Length);

        public bool Contains(char item, int startIndex)
            => Contains(item, startIndex..Length);

        public bool Contains(char item, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Contains(item, startIndex, count);
        }

        public bool Contains(char item, int startIndex, int count)
        {
            EnsureRange(startIndex, count);

            for (int x = 1; x <= count; x++, startIndex++)
            {
                if (this[startIndex] == item)
                    return true;
            }

            return false;
        }

        public bool Contains(string? item)
            => IndexOf(item, 0, Length) != -1;

        public bool Contains(string? item, int startIndex)
            => IndexOf(item, startIndex, Length) != -1;

        public bool Contains(string? item, int startIndex, int count)
            => IndexOf(item, startIndex, count) != -1;

        public bool Contains(string? item, Range range)
            => IndexOf(item, range) != -1;
        #endregion
        #region IndexOf
        public int IndexOf(char item)
            => IndexOf(item, 0, Length);

        public int IndexOf(char item, int startIndex)
            => IndexOf(item, startIndex..Length);

        public int IndexOf(char item, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return IndexOf(item, startIndex, count);
        }

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

        public int IndexOf(string? item, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return IndexOf(item, startIndex, count);
        }

        public int IndexOf(string? item, int startIndex, int count)
            => ((string)this).IndexOf(item, startIndex, count);

        public int[] IndexesOf(char item)
            => IndexesOf(item, 0, Length);

        public int[] IndexesOf(char item, int startIndex)
            => IndexesOf(item, startIndex..Length);

        public int[] IndexesOf(char item, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return IndexesOf(item, startIndex, count);
        }

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
            Ensure.NotNull(chars, nameof(chars));
            if (chars!.Length == 0) return -1;

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
            => LastIndexOf(item, startIndex..Length);

        public int LastIndexOf(char item, Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return LastIndexOf(item, startIndex, count);
        }

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
            Ensure.NotNull(chars, nameof(chars));
            if (chars!.Length == 0) return -1;

            foreach (var c in chars)
            {
                var index = LastIndexOf(c);
                if (index != -1) return index;
            }

            return -1;
        }
        #endregion
        #region Indent
        /// <summary>
        /// Jumps to the next line and adds many amount of chars in the beginning of the line.
        /// </summary>
        /// <param name="count"></param>
        /// <returns></returns>
        public MutableString Indent(int count, char indent = ' ')
        {
            Ensure.ZeroOrPositive(count, nameof(count));

            _builder.AppendLine();
            _builder.Append(new string(indent, count));
            _isModified = true;
            return this;
        }
        #endregion
        #region RangeOf
        /// <summary>
        /// Returns the range in which the matching input is found. Returns null if not found.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public Range? RangeOf(string? input)
        {
            if (input is null || input.Length == 0) return null;
            var startIndex = input.Length == 1 ? IndexOf(input[0]) : IndexOf(input);
            if (startIndex == -1) return null;
            return new Range(startIndex, startIndex + input.Length);
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
        public string[] Split(char separator)
            => this / separator;

        public string[] Split(string separator)
            => this / separator;

        public string[] Split(char separator, StringSplitOptions options)
            => ((string)this).Split(separator, options);

        public string[] Split(params char[] separator)
            => ((string)this).Split(separator);

        public string[] Split(string[] separator, StringSplitOptions options)
            => ((string)this).Split(separator, options);
        #endregion
        #region Padding
        public MutableString PadLeft(int totalWidth)
            => PadLeft(totalWidth, ' ');

        public MutableString PadLeft(int totalWidth, char paddingChar)
        {
            if (totalWidth <= Length)
                return this;

            _isModified = true;
            return Insert(0, new string(paddingChar, totalWidth - Length).AsSpan());
        }

        public MutableString PadRight(int totalWidth)
            => PadRight(totalWidth, ' ');

        public MutableString PadRight(int totalWidth, char paddingChar)
        {
            if (totalWidth <= Length)
                return this;

            _isModified = true;
            return Insert(Length, new string(paddingChar, totalWidth - Length).AsSpan());
        }
        #endregion
        #region Regex
        public bool ContainsPattern([RegexPattern]string? pattern, RegexOptions options = RegexOptions.None)
        {
            Ensure.NotNull(pattern, nameof(pattern));
            return Regex.IsMatch(this, pattern, options);
        }

        public MutableString ReplacePattern([RegexPattern]string? pattern, string? replacement, RegexOptions options = RegexOptions.None)
        {
            Ensure.NotNull(pattern, nameof(pattern));

            replacement ??= string.Empty;

            if (_isModified)
                _current = _builder.ToString();

            _current = Regex.Replace(_current, pattern, replacement, options);
            _isModified = false;
            return this;
        }

        public Match GetMatch([RegexPattern]string? pattern, RegexOptions options = RegexOptions.None)
        {
            Ensure.NotNull(pattern, nameof(pattern));
            return Regex.Match(this, pattern, options);
        }

        public MatchCollection GetMatches([RegexPattern]string? pattern, RegexOptions options = RegexOptions.None)
        {
            Ensure.NotNull(pattern, nameof(pattern));
            return Regex.Matches(this, pattern, options);
        }

        public string[] SplitPattern([RegexPattern]string? pattern, RegexOptions options = RegexOptions.None)
        {
            Ensure.NotNull(pattern, nameof(pattern));
            return Regex.Split(this, pattern, options);
        }
        #endregion
        #region Substring
        public string Substring(int startIndex)
            => Substring(startIndex..Length);

        public string Substring(int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            return _builder.ToString(startIndex, count);
        }

        public string Substring(Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Substring(startIndex, count);
        }
        #endregion
        #region Slice
        public MutableString Slice(int startIndex)
            => Slice(startIndex..Length);

        public MutableString Slice(Range range)
        {
            var (startIndex, count) = range.GetOffsetAndLength(Length);
            return Slice(startIndex, count);
        }

        public MutableString Slice(int startIndex, int count)
        {
            EnsureRange(startIndex, count);
            
            if (startIndex > 0) _builder.Remove(0, startIndex);
            _builder.Remove(count, Length - count);

            _isModified = true;

            return this;
        }
        #endregion
        #region Trim
        public MutableString Trim()
            => Trim(' ');

        public MutableString Trim(char trimChar)
        {
            TrimStart(trimChar);
            return TrimEnd(trimChar);
        }

        public MutableString Trim(params char[]? chars)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (chars.Length == 0) return this;

            foreach (var c in chars)
                Trim(c);

            return this;
        }

        public MutableString TrimStart()
            => TrimStart(' ');

        public MutableString TrimStart(char trimChar)
        {
            var index = 0;

            while (this[index] == trimChar)
                index++;

            if (index > 0) Remove(0, index);

            _isModified = true;

            return this;
        }

        public MutableString TrimStart(params char[]? chars)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (chars.Length == 0) return this;

            foreach (var c in chars)
                TrimStart(c);

            return this;
        }

        public MutableString TrimEnd()
            => TrimEnd(' ');

        public MutableString TrimEnd(char trimChar)
        {
            var index = Length - 1;

            while (this[index] == trimChar)
                index--;

            if (index < Length) Remove(++index..Length);

            _isModified = true;

            return this;
        }

        public MutableString TrimEnd(params char[]? chars)
        {
            if (chars is null) throw new ArgumentNullException(nameof(chars));
            if (chars.Length == 0) return this;

            foreach (var c in chars)
                TrimEnd(c);

            return this;
        }
        #endregion
        #endregion
        #region static methods
        /// <summary>
        /// Returns true if the <see cref="MutableString"/> is null or empty.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrEmpty(MutableString? input)
            => input is null || input.Length == 0;

        /// <summary>
        /// Returns true if the <see cref="MutableString"/> is null, empty or consists only of white-spaces characters.
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static bool IsNullOrWhiteSpace(MutableString? input)
            => IsNullOrEmpty(input) || input.All(char.IsWhiteSpace);

        private void EnsureRange(int startIndex, int count)
        {
            if (startIndex < 0 || startIndex > Length || startIndex + count > Length) throw new ArgumentOutOfRangeException(nameof(startIndex));
            if (count < 0) throw new ArgumentOutOfRangeException(nameof(count));
        }

        #endregion
        private struct MutableStringEnumerator : IEnumerator<char>, ICloneable
        {
            private readonly MutableString _string;
            private int _index;
            private char _current;

            public MutableStringEnumerator(MutableString mutableString)
            {
                _string = mutableString;
                _index = -1;
                _current = default;
            }

            public char Current
            {
                get
                {
                    if (_index == -1)
                        throw new InvalidOperationException("Enumeration hasn't started");
                    if (_index > _string.Length)
                        throw new InvalidOperationException("Enumeration ended already");

                    return _current;
                }
            }

            object IEnumerator.Current
                => Current;

            public void Dispose()
            {
            }

            public bool MoveNext()
            {
                if (_index < _string.Length - 1)
                {
                    _index++;
                    _current = _string[_index];
                    return true;
                }

                _index = _string.Length;
                return false;
            }

            public void Reset()
            {
                _index = -1;
                _current = default;
            }

            object ICloneable.Clone()
                => MemberwiseClone();
        }
    }
}
