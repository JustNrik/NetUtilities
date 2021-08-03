using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Xml.Serialization;
using NetUtilities;

namespace System.Xml
{
    public static class XmlConvert<T> 
    {
        private static readonly XmlSerializer _serializer = new(typeof(T));

        /// <summary>
        ///     Serializes the object into XML Format
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the object that will be serialized
        /// </typeparam>
        /// <param name="obj">
        ///     The object to be serialized
        /// </param>
        /// <returns>
        ///     A string with the XML representation of the object provided.
        /// </returns>
        /// <exception cref="ArgumentNullException">
        ///     Thrown if the object is <see langword="null"/>.
        /// </exception>
        public static string SerializeObject(T obj)
        {
            using var stringWriter = new StringWriter();
            _serializer.Serialize(stringWriter, Ensure.NotNull(obj));
            var ret = stringWriter.ToString();
            return ret;
        }

        /// <summary>
        ///     Deserializes a XML Formatted string into an object
        /// </summary>
        /// <typeparam name="T">
        ///     Type of the object that will be serialized
        /// </typeparam>
        /// <param name="input">
        ///     The Formatted XML string
        /// </param>
        /// <returns>
        ///     The object of type <typeparamref name="T"/> being deserialized.
        /// </returns>
        /// <exception cref="InvalidOperationException">
        ///     Thrown if the deserialization fails.
        /// </exception>
        public static T? DeserializeObject(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidOperationException("The input is not deserializable, it's null, empty or consists only of white-spaces");

            using var stringReader = new StringReader(input);
            var ret = (T?)_serializer.Deserialize(stringReader);
            return ret;
        }
    }
}
