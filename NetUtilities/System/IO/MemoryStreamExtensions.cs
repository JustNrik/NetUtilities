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

            // false if they are not equal or if capacity is 0.
            int len = ms.GetBuffer().Length;
            bool changeCapacity = len != capacity || capacity == 0;
            Array.Clear(ms.GetBuffer(), 0, len);
            ms.Position = 0;
            ms.SetLength(0);
            if (changeCapacity)
            {
                ms.Capacity = capacity;
            }
        }
    }
}
