using System;
using System.Collections.Generic;
using System.Linq;
using OneOf;
using Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV11 : MetadataBase
    {
        public ModuleV11[] Module { get; set; }
        public string[] ExtrinsicExtension { get; set; }

        public MetadataV11()
        {
            Version = 11;
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

    public class ModuleV11 : ModuleBase, IModuleMeta
    {
        public string Name { get; set; }
        public StorageCollectionV11 Storage { get; set; }
        public CallV11[] Call { get; set; }
        public EventArgV11[] Ev { get; set; }
        public ConstV11[] Cons { get; set; }
        public ErrorV11[] Errors { get; set; }

        public ModuleV11()
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

    public class ErrorV11
    {
        public string Name { get; set; }
        public string[] Documentation { get; set; }
    }

    public class FunctionCallArgV11 : ICallArgument
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV11 : IEventMeta
    {
        public string Name { get; set; }
        public string[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }

        public IReadOnlyList<string> GetArguments() => Args;
    }

    public class CallV11 : ICallMeta
    {
        public string Name { get; set; }
        public FunctionCallArgV11[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName() => Name;
        public IReadOnlyList<ICallArgument> GetArguments() => Args;
    }

    public class FuncTypeV11
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

    public class StorageCollectionV11
    {
        public string Prefix { get; set; }
        public StorageV11[] Items { get; set; }
        public IReadOnlyCollection<IStorage> GetStorages()
        {
            return Items;
        }
    };

    public class StorageV11: IStorage
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV11 Type { get; set; }
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

    public class ConstV11 : IConstantMeta
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