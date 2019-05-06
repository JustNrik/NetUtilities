using System.IO;
using System.Xml.Serialization;

namespace System.Xml
{
    public static class XmlConvert
    {
        public static string SerializeObject(object @object)
        {
            using var stringWriter = new StringWriter();
            var serializer = new XmlSerializer(@object.GetType());
            serializer.Serialize(stringWriter, @object);
            return stringWriter.ToString();
        }

        public static T DeserializeObject<T>(string input)
        {
            using var stringReader = new StringReader(input);
            var serializer = new XmlSerializer(typeof(T));
            return (T)serializer.Deserialize(stringReader);
        }
    }
}
