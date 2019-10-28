namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV8 : MetadataBase
    {
        public ModuleV8[] Module { get; set; }

        public MetadataV8()
        {
            Version = 8;
        }
    }

    public class ModuleV8
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
    }

    public class ErrorV8
    {
        public string Name { get; set; }
        public string[] Documentation { get; set; }
    }

    public class FunctionCallArgV8
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV8
    {
        public string Name { get; set; }
        public string[] Args { get; set; }
        public string[] Documentation { get; set; }
    }

    public class CallV8
    {
        public string Name { get; set; }
        public FunctionCallArgV8[] Args { get; set; }
        public string[] Documentation { get; set; }
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

    public class StorageV8
    {
        public string Name { get; set; }
        public string Prefix { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV8 Type { get; set; }
        public string Fallback { get; set; }
        public string[] Documentation { get; set; }
    }

    public class ConstV8
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
        public string[] Documentation { get; set; }
    }
}