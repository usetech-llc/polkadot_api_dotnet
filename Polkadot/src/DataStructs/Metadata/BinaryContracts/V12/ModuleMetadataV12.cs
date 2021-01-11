using System;
using System.Collections.Generic;
using OneOf;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinarySerializer.Types;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata.BinaryContracts.V12
{
    public class ModuleMetadataV12 : ModuleBase, IModuleMeta
    {
        [Serialize(0)]
        [Utf8StringConverter]
        public string Name { get; set; }

        [Serialize(1)]
        public Option<StorageMetadataV12> Storage { get; set; } 
        
        [Serialize(2)]
        [OneOfConverter(ItemConverters = new []{ null, typeof(PrefixedArrayConverter)})]
        public OneOf<OneOf.Types.None, FunctionMetadataV12[]> Calls { get; set; }
        
        [Serialize(3)]
        [OneOfConverter(ItemConverters = new []{ null, typeof(PrefixedArrayConverter)})]
        public OneOf<OneOf.Types.None, EventMetadataV12[]> Events { get; set; }
        
        [Serialize(4)]
        [PrefixedArrayConverter]
        public ModuleConstantMetadataV12[] Constants { get; set; }
        
        [Serialize(5)]
        [PrefixedArrayConverter]
        public ErrorMetadataV12[] Errors { get; set; }
        /// Define the index of the module, this index will be used for the encoding of module event,
        /// call and origin variants.
        [Serialize(6)]
        public byte Index { get; set; }

        public override IReadOnlyList<IConstantMeta> GetConstants()
        {
            return Constants;
        }

        public override string GetName()
        {
            return Name;
        }

        public override IReadOnlyList<ICallMeta> GetCalls()
        {
            return Calls.Match(_ => Array.Empty<ICallMeta>(), c => c);
        }

        public override IReadOnlyList<IEventMeta> GetEvents()
        {
            return Events.Match(_ => Array.Empty<IEventMeta>(), e => e);
        }
    }
}