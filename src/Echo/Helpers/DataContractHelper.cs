using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

namespace Echo.Helpers
{
    public static class DataContractHelper
    {
        public static string SerializeToXmlString<T>(T graph)
        {
            var type = typeof(T);
            if (!type.IsDefined(typeof(DataContractAttribute), true))
                throw new SerializationException("Object's class does not have any defined DataContract attribute.");

            var sb = new StringBuilder();
            using (var xmlWriter = new XmlTextWriter(new StringWriter(sb)))
            {
                var serializer = new DataContractSerializer(type);
                serializer.WriteObject(xmlWriter, graph);
            }

            return sb.ToString();

        }

        public static T SerializeFromXmlString<T>(string graph)
        {
            if (string.IsNullOrEmpty(graph)) return default(T);

            var type = typeof(T);
            if (!type.IsDefined(typeof(DataContractAttribute), true))
                throw new SerializationException("Object's class does not have any defined DataContract attribute.");

            using (var xmlReader = new XmlTextReader(new StringReader(graph)))
            {
                var serializer = new DataContractSerializer(type);
                return (T)serializer.ReadObject(xmlReader);
            }
        }

        public static string SerializeListToXmlString<T>(IEnumerable<T> stringCollection)
        {
            var stringWriter = new StringWriter();
            using (var xmlWriter = new XmlTextWriter(stringWriter))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<T>));
                xmlSerializer.Serialize(xmlWriter, stringCollection);
                return stringWriter.ToString();
            }
        }

        public static IEnumerable<T> SerializeListFromXmlString<T>(string stringCollectionXml)
        {
            using (var xmlReader = new XmlTextReader(new StringReader(stringCollectionXml)))
            {
                var xmlSerializer = new XmlSerializer(typeof(List<T>));
                return (IEnumerable<T>)xmlSerializer.Deserialize(xmlReader);
            }
        }

    }
}
