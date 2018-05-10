using System.IO;
using System.Text;

namespace MyTestWeb2017.Service
{
    public class DotNetXmlSerializer : ISerializer
    {
        public string Serialize<T>(T t)
        {
            var serializedString = string.Empty;

            if (t != null)
            {
                //using (MemoryStream stream = new MemoryStream())
                //{
                //    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                //    serializer.Serialize(stream, t);
                //    serializedString = Encoding.UTF8.GetString(stream.ToArray());
                //}

                using (StringWriter stream = new StringWriter())
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    serializer.Serialize(stream, t);
                    serializedString = stream.ToString();
                }
            }

            return serializedString;
        }

        public T Deserialize<T>(string serializedString)
        {
            T t = default(T);

            if (!string.IsNullOrWhiteSpace(serializedString))
            {
                var serializedBytes = Encoding.UTF8.GetBytes(serializedString);

                using (MemoryStream stream = new MemoryStream(serializedBytes))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    t = (T)serializer.Deserialize(stream);
                }
            }

            return t;
        }

        public byte[] SerializeToBytes<T>(T t)
        {
            var serializedBytes = default(byte[]);

            if (t != null)
            {
                using (MemoryStream stream = new MemoryStream())
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    serializer.Serialize(stream, t);
                    serializedBytes = stream.ToArray();
                }
            }

            return serializedBytes;
        }

        public T Deserialize<T>(byte[] bytes)
        {
            T t = default(T);

            if (bytes != null && bytes.Length > 0)
            {
                using (MemoryStream stream = new MemoryStream(bytes))
                {
                    System.Xml.Serialization.XmlSerializer serializer = new System.Xml.Serialization.XmlSerializer(typeof(T));
                    t = (T)serializer.Deserialize(stream);
                    return t;
                }
            }

            return default(T);
        }
    }
}
