using System;

namespace Polkadot.BinarySerializer
{
    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field)]
    public class SerializeAttribute : Attribute
    {
        public int Order { get; set; }

        public SerializeAttribute(int order)
        {
            Order = order;
        }
    }
}