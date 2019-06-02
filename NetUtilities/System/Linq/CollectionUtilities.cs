using NetUtilities;
using System.Collections.Generic;
using System.Collections.ObjectModel;
#nullable enable
namespace System.Linq
{
    public static partial class LinqUtilities
    {
        public static void AddOrUpdate<T>(this IList<T> source, T element)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.CanWrite(source);

            var index = source.IndexOf(element);

            if (index == -1)
            {
                source.Add(element);
            }
            else
            {
                source[index] = element;
            }
        }

        public static void AddOrUpdate<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(key, nameof(key));
            Ensure.CanWrite(source);

            if (source.ContainsKey(key))
            {
                source[key] = value;
            }
            else
            {
                source.Add(key, value);
            }
        }

        public static TValue GetOrAdd<TKey, TValue>(this IDictionary<TKey, TValue> source, TKey key, TValue value)
        {
            Ensure.NotNull(source, nameof(source));
            Ensure.NotNull(key, nameof(key));
            Ensure.CanWrite(source);

            if (source.TryGetValue(key, out var val))
                return val;

            source.Add(key, value);
            return value;
        }
    }
}
