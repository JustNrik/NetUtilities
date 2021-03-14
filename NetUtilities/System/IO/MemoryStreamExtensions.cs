using System;

namespace System.IO
{
    /// <summary>
    /// A extension class for extensions to <see cref="MemoryStream"/>.
    /// </summary>
    public static class MemoryStreamExtensions
    {
        /// <summary>
        /// Clears the data in the <see cref="MemoryStream"/>.
        /// </summary>
        /// <param name="ms">
        /// The input <see cref="MemoryStream"/>.
        /// </param>
        public static void Clear(this MemoryStream ms, int capacity)
        {
            if (ms == null)
            {
                throw new ArgumentNullException(nameof(ms));
            }

            if (capacity < 0)
            {
                throw new IndexOutOfRangeException("capacity must not be negative.");
            }

            var len = ms.GetBuffer().Length;
            var changeCapacity = len != capacity;
            Array.Clear(ms.GetBuffer(), 0, len);
            ms.Position = 0;
            ms.SetLength(0);
            ms.Capacity = 0;

            // avoid setting to the same value twice.
            if (changeCapacity && capacity != 0)
            {
                ms.Capacity = capacity;
            }
        }
    }
}
