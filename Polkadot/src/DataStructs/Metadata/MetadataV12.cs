using System;
using System.Collections.Generic;
using System.Linq;
using OneOf;
using Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV12 : MetadataBase
    {
        public ModuleV12[] Module { get; set; }
        public string[] ExtrinsicExtension { get; set; }

        public MetadataV12()
        {
            Version = 12;
        }

        public override IReadOnlyList<IModuleMeta> GetModules()
        {
            return Module;
        }

        public override string[] GetExtrinsicExtension()
        {
            return ExtrinsicExtension;
        }
    }

    public class ModuleV12 : ModuleBase, IModuleMeta
    {
        public string Name { get; set; }
        public StorageCollectionV12 Storage { get; set; }
        public CallV12[] Call { get; set; }
        public EventArgV12[] Ev { get; set; }
        public ConstV12[] Cons { get; set; }
        public ErrorV12[] Errors { get; set; }
        public byte Index { get; set; }

        public ModuleV12()
        {
            Name = null;
            Storage = null;
            Call = null;
            Ev = null;
            Index = 0;
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

    public class ErrorV12
    {
        public string Name { get; set; }
        public string[] Documentation { get; set; }
    }

    public class FunctionCallArgV12
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV12 : IEventMeta
    {
        public string Name { get; set; }
        public string[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }
    }

    public class CallV12 : ICallMeta
    {
        public string Name { get; set; }
        public FunctionCallArgV12[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }
    }

    public class FuncTypeV12
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

    public class StorageCollectionV12
    {
        public string Prefix { get; set; }
        public StorageV12[] Items { get; set; }
        public IReadOnlyCollection<IStorage> GetStorages()
        {
            return Items;
        }
    };

    public class StorageV12: IStorage
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV12 Type { get; set; }
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

    public class ConstV12 : IConstantMeta
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

        public byte[] GetValueBytes()
        {
            return null;
        }
    }
}