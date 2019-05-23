using System.IO;
using System.Xml.Linq;
using System.Xml.Serialization;
#nullable enable
namespace System.Xml
{
    public static class XmlConvert
    {
        /// <summary>
        /// Serializes the object into XML Format
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeObject<T>(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        /// <summary>
        /// Deserializes a XML Formatted string into an object
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="input"></param>
        /// <returns></returns>
        public static T DeserializeObject<T>(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                throw new InvalidOperationException("The input is not deserializable, it's null, empty or consists only of white-spaces");

            using var stringReader = new StringReader(input);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }

        /// <summary>
        /// Extension method for <see cref="DeserializeObject{T}(string)"/>
        /// </summary>
        /// <exception cref="InvalidOperationException"></exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="element"></param>
        /// <returns></returns>
        public static T ToObject<T>(this XNode node)
            => DeserializeObject<T>(node.ToString());

        /// <summary>
        /// Extension method for <see cref="SerializeObject{T}(T)"/>
        /// </summary>
        /// <exception cref="ArgumentNullException"></exception>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static string SerializeAsXml<T>(this T obj)
            => SerializeObject(obj);
    }
}
