using System;

namespace Polkadot.Api.Client.Serialization
{
    public interface IHasConverter
    {
        Type ConverterType { get; set; }
    }
}