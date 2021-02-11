using System;
using System.Collections.Generic;
using System.Linq;
using OneOf;
using Polkadot.DataStructs.Metadata.BinaryContracts.StorageEntryType;
using Polkadot.DataStructs.Metadata.Interfaces;

namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV4 : MetadataBase
    {
        public ModuleV4[] Module { get; set; }

        public MetadataV4()
        {
            Version = 4;
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

    public class ModuleV4 : ModuleBase, IModuleMeta
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        public StorageV4[] Storage { get; set; }
        public CallV4[] Call { get; set; }
        public EventArgV4[] Ev { get; set; }

        public ModuleV4()
        {
            Name = null;
            Prefix = null;
            Storage = null;
            Call = null;
            Ev = null;
        }

        public override IReadOnlyList<IConstantMeta> GetConstants()
        {
            return Array.Empty<IConstantMeta>();
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
            return Storage;
        }
    }

    public class FunctionCallArgV4 : ICallArgument
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV4 : IEventMeta
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

    public class CallV4 : ICallMeta
    {
        public string Name { get; set; }
        public FunctionCallArgV4[] Args { get; set; }
        public string[] Documentation { get; set; }
        public string GetName() => Name;

        public IReadOnlyList<ICallArgument> GetArguments() => Args;
    }

    public class FuncTypeV4
    {
        // 0 - plain, 1 - map
        public uint Type { get; set; }
        public uint Hasher { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
    }

    public class StorageV4: IStorage
    {
        public string Name { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV4 Type { get; set; }
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
}