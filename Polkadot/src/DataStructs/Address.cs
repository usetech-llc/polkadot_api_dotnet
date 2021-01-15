using Polkadot.Utils;
using System;
using System.IO;
using System.Linq;
using System.Text;
using Polkadot.BinarySerializer;

namespace Polkadot.DataStructs
{
    public class Address : IBinarySerializable
    {
        public string Symbols { get; set; }

        public Address() { }

        public Address(string symbols)
        {
            Symbols = symbols;
        }

        public void Serialize(Stream stream, IBinarySerializer serializer)
        {
            var bytes = AddressUtils.GetPublicKeyFromAddr(Symbols).Bytes;
            stream.Write(bytes, 0, bytes.Length);
        }
    }
}