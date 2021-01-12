using System;
using System.Collections.Generic;
using System.Linq;
using OneOf;
using Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV7 : MetadataBase
    {
        public ModuleV7[] Module { get; set; }

        public MetadataV7()
        {
            Version = 7;
        }

        public override IReadOnlyList<IModuleMeta> GetModules()
        {
            return Module;
        }

        public override string[] GetExtrinsicExtension()
        {
            return null;
        }
    }

    public class ModuleV7 : ModuleBase, IModuleMeta
    {
        public string Name { get; set; }
        public StorageCollectionV7 Storage { get; set; }
        public CallV7[] Call { get; set; }
        public EventArgV7[] Ev { get; set; }
        public ConstV7[] Cons { get; set; }

        public ModuleV7()
        {
            Name = null;
            Storage = null;
            Call = null;
            Ev = null;
        }

        public override IReadOnlyList<IConstantMeta> GetConstants()
        {
            return Cons ?? Array.Empty<IConstantMeta>();
        }

        public override string GetName()
        {
            return Name;
        }

        public override IReadOnlyList<ICallMeta> GetCalls()
        {
            return Call;
        }

        public override IReadOnlyList<IEventMeta> GetEvents()
        {
            return Ev;
        }

        public override IReadOnlyList<IStorage> GetStorages()
        {
            return Storage?.Items;
        }
    }

    public class FunctionCallArgV7
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV7 : IEventMeta
    {
        public string Name { get; set; }
        public string[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }
    }

    public class CallV7 : ICallMeta
    {
        public string Name { get; set; }
        public FunctionCallArgV7[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }
    }

    public class FuncTypeV7
    {
        // 0 - plain, 1 - map
        public uint Type { get; set; }
        public uint Hasher { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
        public string Value { get; set; }
        public string Key2hasher { get; set; }
        public bool IsLinked { get; set; }
    }

    public class StorageCollectionV7
    {
        public string Prefix { get; set; }
        public StorageV7[] Items { get; set; }
    };

    public class StorageV7: IStorage
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV7 Type { get; set; }
        public string Fallback { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }

        public OneOf<Plain, Map, DoubleMap> GetStorageType()
        {
            return OneOf<Plain, Map, DoubleMap>.FromT0(new Plain());
        }
    }

    public class ConstV7 : IConstantMeta
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }

        public string GetValue()
        {
            return Value;
        }
    }
}