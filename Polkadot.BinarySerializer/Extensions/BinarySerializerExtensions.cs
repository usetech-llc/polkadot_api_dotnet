using System.IO;

namespace Polkadot.BinarySerializer.Extensions
{
    public static class BinarySerializerExtensions
    {
        /// <summary>
        /// Deserializes data from stream and throws <see cref="NotAllDataUsedException"/> if not all data was used.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="stream"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeserializeAssertReadAll<T>(this IBinarySerializer serializer, Stream stream)
        {
            var deserialized = serializer.Deserialize<T>(stream);
            if (stream.ReadByte() != -1)
            {
                throw new NotAllDataUsedException();
            }

            return deserialized;
        } 

        /// <summary>
        /// Deserializes data from stream and throws <see cref="NotAllDataUsedException"/> if not all data was used.
        /// </summary>
        /// <param name="serializer"></param>
        /// <param name="data"></param>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T DeserializeAssertReadAll<T>(this IBinarySerializer serializer, byte[] data)
        {
            using var stream = new MemoryStream(data);
            return serializer.DeserializeAssertReadAll<T>(stream);
        } 
    }
}