using System;
using System.Collections.Generic;
using System.Text;
using Polkadot.BinarySerializer;

namespace Polkadot.DataStructs
{
    public interface ITypeCreate
    {
        byte[] GetTypeEncoded(IBinarySerializer serializer);
    }
}
