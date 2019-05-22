#nullable enable
namespace System.Collections.Generic
{
    public static class DictionaryUtilities
    {
        /// <summary>
        /// Deconstructor for <see cref="KeyValuePair{TKey, TValue}"/> which will allow you to use Tuples in a foreach loop.
        /// </summary>
        /// <example>asd</example>
        /// <typeparam name="TKey"></typeparam>
        /// <typeparam name="TValue"></typeparam>
        /// <param name="kvp"></param>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public static void Deconstruct<TKey, TValue>(this KeyValuePair<TKey, TValue> kvp, out TKey key, out TValue value)
        {
            key = kvp.Key;
            value = kvp.Value;
        }
    }
}
