using System;
using System.Collections.Generic;
using System.Linq;
using OneOf;
using Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV8 : MetadataBase
    {
        public ModuleV8[] Module { get; set; }

        public MetadataV8()
        {
            Version = 8;
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

    public class ModuleV8 : ModuleBase, IModuleMeta
    {
        public string Name { get; set; }
        public StorageCollectionV8 Storage { get; set; }
        public CallV8[] Call { get; set; }
        public EventArgV8[] Ev { get; set; }
        public ConstV8[] Cons { get; set; }
        public ErrorV8[] Errors { get; set; }

        public ModuleV8()
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

        public override IReadOnlyList<IErrorMeta> GetErrors()
        {
            return Errors;
        }
    }

    public class ErrorV8 : IErrorMeta
    {
        public string Name { get; set; }
        public string[] Documentation { get; set; }
        public string GetName()
        {
            return Name;
        }
    }

    public class FunctionCallArgV8 : ICallArgument
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV8 : IEventMeta
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

    public class CallV8 : ICallMeta
    {
        public string Name { get; set; }
        public FunctionCallArgV8[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName() => Name;
        public IReadOnlyList<ICallArgument> GetArguments() => Args;
    }

    public class FuncTypeV8
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

    public class StorageCollectionV8
    {
        public string Prefix { get; set; }
        public StorageV8[] Items { get; set; }
    };

    public class StorageV8: IStorage
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV8 Type { get; set; }
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

    public class ConstV8 : IConstantMeta
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