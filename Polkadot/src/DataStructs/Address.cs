using Polkadot.Utils;
using System;
using System.Linq;
using System.Text;
using Polkadot.BinarySerializer;

namespace Polkadot.DataStructs
{
    public class Address : ITypeCreate
    {
        public string Symbols { get; set; }

        public Address() { }

        public Address(string symbols)
        {
            Symbols = symbols;
        }

        public byte[] GetTypeEncoded(IBinarySerializer serializer)
        {
            var aupk = AddressUtils.GetPublicKeyFromAddr(Symbols);
            return Hash.GetStorageKey(Hasher.BLAKE2, aupk.Bytes, aupk.Bytes.Length, serializer).Concat(aupk.Bytes).ToArray();
        }
    }
}