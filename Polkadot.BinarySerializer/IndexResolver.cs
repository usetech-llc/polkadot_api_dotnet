using System;

namespace Polkadot.BinarySerializer
{
    public class IndexResolver
    {
        public Func<(string module, string method), (byte moduleIndex, byte methodIndex)?> CallIndex;
        public Func<(string module, string method), (byte moduleIndex, byte eventIndex)?> EventIndex;
    }
}