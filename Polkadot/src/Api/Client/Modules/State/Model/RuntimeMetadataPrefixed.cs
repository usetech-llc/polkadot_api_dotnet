using System.Text.Json.Serialization;
using OneOf;
using Polkadot.Api.Client.Modules.State.Model.V12;
using Polkadot.Api.Client.RpcCalls;
using Polkadot.Api.Client.Serialization;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Modules.State.Model
{
    /// <summary>
    /// From frame\metadata\src\lib.rs
    /// </summary>
    [JsonConverter(typeof(BinaryJsonConverter<RuntimeMetadataPrefixed>))]
    public class RuntimeMetadataPrefixed
    {
        [Serialize(0)]
        public int Reserved { get; set; }
        
        [Serialize(1)]
        [OneOfConverter]
        public OneOf<
            RuntimeMetadataDeprecated,// V0
            RuntimeMetadataDeprecated,// V1
            RuntimeMetadataDeprecated,// V2
            RuntimeMetadataDeprecated,// V3
            RuntimeMetadataDeprecated,// V4
            RuntimeMetadataDeprecated,// V5
            RuntimeMetadataDeprecated,// V6
            RuntimeMetadataDeprecated,// V7
            RuntimeMetadataDeprecated,// V8
            RuntimeMetadataDeprecated,// V9
            RuntimeMetadataDeprecated,// V10
            RuntimeMetadataDeprecated,// V11
            RuntimeMetadataV12        // V12
        > RuntimeMetadata { get; set; }
    }
}