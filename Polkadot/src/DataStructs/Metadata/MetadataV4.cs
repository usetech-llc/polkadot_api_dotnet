namespace Polkadot.DataStructs.Metadata
{
    public class MetadataV4 : MetadataBase
    {
        public ModuleV4[] Module { get; set; }

        public MetadataV4()
        {
            Version = 4;
        }
    }

    public class ModuleV4
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
    }

    public class FunctionCallArgV4
    {
        public string Name { get; set; }
        public string Type { get; set; }
    }

    public class EventArgV4
    {
        public string Name { get; set; }
        public string[] Args { get; set; }
        public string[] Documentation { get; set; }
    }

    public class CallV4
    {
        public string Name { get; set; }
        public FunctionCallArgV4[] Args { get; set; }
        public string[] Documentation { get; set; }
    }

    public class FuncTypeV4
    {
        // 0 - plain, 1 - map
        public uint Type { get; set; }
        public uint Hasher { get; set; }
        public string Key1 { get; set; }
        public string Key2 { get; set; }
    }

    public class StorageV4
    {
        public string Name { get; set; }
        // 0 - Optional, 1 - Default
        public uint Modifier { get; set; }
        public FuncTypeV4 Type { get; set; }
        public string Fallback { get; set; }
        public string[] Documentation { get; set; }
    }
}