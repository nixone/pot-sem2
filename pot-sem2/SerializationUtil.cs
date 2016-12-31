using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace pot_sem2
{
    /// <summary>
    /// Utility methods for serialization based on DataContracts
    /// </summary>
    public class SerializationUtil
    {
        /// <summary>
        /// Serializes the object
        /// </summary>
        /// <param name="obj">object to serialize</param>
        /// <returns>serialized as string</returns>
        public static string Serialize(object obj)
        {
            using (MemoryStream memoryStream = new MemoryStream())
            using (StreamReader reader = new StreamReader(memoryStream))
            {
                DataContractSerializer serializer = new DataContractSerializer(obj.GetType());
                serializer.WriteObject(memoryStream, obj);
                memoryStream.Position = 0;
                return reader.ReadToEnd();
            }
        }

        /// <summary>
        /// Deserializes the object
        /// </summary>
        /// <param name="str">previously serialized data</param>
        /// <param name="toType">type of data previously serialized</param>
        /// <returns>deserialized instance</returns>
        public static object Deserialize(string str, Type toType)
        {
            using (Stream stream = new MemoryStream())
            {
                byte[] data = System.Text.Encoding.UTF8.GetBytes(str);
                stream.Write(data, 0, data.Length);
                stream.Position = 0;
                DataContractSerializer deserializer = new DataContractSerializer(toType);
                return deserializer.ReadObject(stream);
            }
        }
    }
}
