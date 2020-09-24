using Polkadot.Utils;
using System;
using System.Text;

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

        public string GetTypeEncoded()
        {
            var aupk = AddressUtils.GetPublicKeyFromAddr(Symbols);
            var str = BitConverter.ToString(aupk.Bytes).Replace("-", "");
            return $"{Hash.GetStorageKey(Hasher.BLAKE2, aupk.Bytes)}{str}";
        }
    }
}