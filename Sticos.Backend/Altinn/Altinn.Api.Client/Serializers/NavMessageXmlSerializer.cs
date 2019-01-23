using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Xml;
using System.Xml.Serialization;
using Altinn.Api.Domain.Interfaces;

namespace Altinn.Api.Client.Serializers
{
    public class NavMessageXmlSerializer : IXmlSerializer
    {
        private Dictionary<Type, XmlSerializer> _xmlSerializers = new Dictionary<Type, XmlSerializer>();

        public string Serialize<T>(T message)
        {
            var encoding = Encoding.UTF8;
            var settings = new XmlWriterSettings
            {
                Indent = true,
                OmitXmlDeclaration = false,
                Encoding = encoding
            };
            var serializer = GetSerializer<T>();
            using (var memoryStream = new MemoryStream())
            using (var xw = XmlWriter.Create(memoryStream, settings))
            {
                serializer.Serialize(xw, message);
                return encoding.GetString(memoryStream.ToArray());
            }
        }

        public T Deserialize<T>(string xml)
        {
            using (var reader = new StringReader(xml))
            {
                var serializer = GetSerializer<T>();
                return (T)serializer.Deserialize(reader);
            }
        }

        private XmlSerializer GetSerializer<T>()
        {
            var type = typeof(T);
            if (!_xmlSerializers.TryGetValue(type, out XmlSerializer serializer))
            {
                serializer = new XmlSerializer(type);
                _xmlSerializers.Add(type, serializer);
            }

            return serializer;
        }
    }
}