using System.IO;
using System.Xml.Serialization;
#nullable enable
namespace System.Xml
{
    public static class XmlConvert
    {
        public static string SerializeObject<T>(T obj)
        {
            if (obj is null)
                throw new ArgumentNullException(nameof(obj));

            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(typeof(T));
            serializer.Serialize(stringWriter, obj);
            return stringWriter.ToString();
        }

        public static T DeserializeObject<T>(string input)
        {
            if (input is null)
                throw new ArgumentNullException(nameof(input));

            using var stringReader = new StringReader(input);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
    }
}
