using NetUtilities;

namespace System.IO
{
    /// <summary>
    ///     Zero allocation equivalent of <see cref="StringReader"/>.
    /// </summary>
    public ref struct ValueStringReader
    {
        private readonly ReadOnlySpan<char> _span;
        private readonly int _length;
        private int _index;

        /// <summary>
        ///     Returns the current character. <see langword="null"/> if the end is reached.
        /// </summary>
        public char? Current
            => _index == _length
            ? null
            : _span[_index];

        /// <summary>
        ///     Initializes an instance of <see cref="ValueStringReader"/> <see langword="struct"/> with the provided <see cref="ReadOnlySpan{T}"/> of <see cref="char"/>.
        /// </summary>
        /// <param name="span">
        ///     The span to read.
        /// </param>
        public ValueStringReader(ReadOnlySpan<char> span) : this()
        {
            _length = span.Length;
            _span = span;
        }

        /// <summary>
        ///     Initializes an instance of <see cref="ValueStringReader"/> <see langword="struct"/> with the provided <see cref="string"/>.
        /// </summary>
        /// <param name="reference">
        ///     The string to read.
        /// </param>
        public ValueStringReader(string reference) : this()
        {
            Ensure.NotNull(reference);

            _length = reference.Length;
            _span = reference.AsSpan();
        }

        /// <summary>
        ///     Reads the next characters and advances one position of the buffer.
        /// </summary>
        /// <returns>
        ///     The current character after advancing one position. <see langword="null"/> if the buffer is completely read.
        /// </returns>
        public char? Read()
        {
            if (_index == _length)
                return null;

            return _span[_index++];
        }

        /// <summary>
        ///     Reads all the characters until a new line is found.
        /// </summary>
        /// <returns>
        ///     A <see cref="ReadOnlySpan{T}"/> of <see cref="char"/> with all characters found, excluding the new line.
        /// </returns>
        public ReadOnlySpan<char> ReadLine()
        {
            var index = _index;

            while (index < _length)
            {
                var current = _span[index];

                if (current == '\n' || current == '\r')
                {
                    var result = _span[_index..index];

                    _index = index + 1;

                    if (current == '\r' && _span[_index] == '\n' && _index < _length)
                        _index++;

                    return result;
                }

                index++;
            }

            if (index > _index)
            {
                var result = _span[_index..index];

                _index = index;
                return result;
            }

            return default;
        }

        /// <summary>
        ///     Fully reads the buffer from the current position.
        /// </summary>
        /// <returns>
        ///     A <see cref="ReadOnlySpan{T}"/> of <see cref="char"/> with all the characters from the current position until the end of the buffer.
        /// </returns>
        public ReadOnlySpan<char> ReadToEnd()
        {
            var span = _index == 0
                ? _span
                : _span[_index..];

            _index = _length;
            return span;
        }
    }
}
