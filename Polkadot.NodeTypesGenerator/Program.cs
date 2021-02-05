using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Extensions.Configuration;
using Polkadot.Api;
using Polkadot.DataStructs.Metadata;
using Polkadot.DataStructs.Metadata.Interfaces;
using Polkadot.NodeTypesGenerator.TypesParser;
using Polkadot.NodeTypesGenerator.TypesParser.Types;
using Sprache;

namespace Polkadot.NodeTypesGenerator
{
    class Program
    {
        private static readonly HashSet<string> UnknownTypes = new();
        private static readonly HashSet<string> KnownTypes = new()
        {
            "Key",
            "KeyValue",
            "Perbill",
            "ulong",
            "ushort",
            "uint",
            "byte",
            "PublicKey",
            "ChangesTrieConfiguration",
            "System.Numerics.BigInteger",
            "CollectionMode",
            "bool",
            "ChainLimits",
            "CollectionLimits",
            "AccessMode",
            "CreateItemData",
            "SchemaVersion",
            "Weight",
            "Gas",
            "Schedule",
            "VestingInfo",
            "Balance",
            "BlockNumber",
            "InheritanceCall<IExtrinsicCall>",
            "Hash",
            "KeyOwnerProof",
            "EquivocationProof",
        };
        private const string Tab = "    ";
        private static readonly string PropertyTab = $"{Tab}{Tab}";
        
        public static Configuration GetConfiguration()
        {
            var builder = new ConfigurationBuilder()
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
            
            var configurationRoot = builder.Build();
            var configuration = new Configuration();
            configurationRoot.Bind(configuration);
            return configuration;
        }
        
        static void Main(string[] args)
        {
            var configuration = GetConfiguration();
            using var app = PolkaApi.GetApplication();
            app.Connect(configuration.NodeWsEndpoint);
            var metadata = app.GetMetadata(null);
            Directory.CreateDirectory($"{configuration.Output}/Calls");
            var serializerRegistrations = new StringBuilder();
            var usingCalls = new StringBuilder();
            foreach (var module in metadata.GetModules())
            {
                usingCalls.AppendLine($"using Polkadot.BinaryContracts.Calls.{CamelCase(module.GetName())};");
                Directory.CreateDirectory($"{configuration.Output}/Calls/{CamelCase(module.GetName())}");
                foreach (var callRegistration in CreateCalls(module, configuration))
                {
                    serializerRegistrations.AppendLine(callRegistration);
                }
            }

            var registerCalls = $@"{usingCalls}

namespace Polkadot.Api
{{
    public partial class Application : IApplication, IWebSocketMessageObserver
    {{
        public void RegisterGeneratedCalls(SerializerSettings settings) 
        {{  
            settings
            {serializerRegistrations};
        }}
    }}
}}";
            
            File.WriteAllText($"{configuration.Output}/Calls/Application.calls.cs", registerCalls);
            
            Console.WriteLine("Unknown types:");
            Console.WriteLine(string.Join(Environment.NewLine, UnknownTypes));
        }

        private static IEnumerable<string> CreateCalls(IModuleMeta module, Configuration configuration)
        {
            foreach (var call in module.GetCalls())
            {
                var args = call.GetArguments()
                    .Select(a => (Property: ParseProperty(a.Name, a.Type), OriginalType: a.Type))
                    .Select((p, i) => 
                        @$"{PropertyTab}// Rust type {p.OriginalType}
{PropertyTab}[Serialize({i})]
{p.Property}");

                var callClass = $@"using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;


namespace Polkadot.BinaryContracts.Calls.{CamelCase(module.GetName())}
{{
{Tab}public class {CamelCase(call.GetName())} : IExtrinsicCall
{Tab}{{
{string.Join(Environment.NewLine + Environment.NewLine, args)}
{Tab}}}
}}";
                
                File.WriteAllText($"{configuration.Output}/Calls/{CamelCase(module.GetName())}/{CamelCase(call.GetName())}.cs", callClass);
                yield return @$"{Tab}{Tab}{Tab}{Tab}.AddCall<{CamelCase(call.GetName())}>(""{module.GetName()}"", ""{call.GetName()}"")";
            }
        }

        private static string ParseProperty(string name, string type)
        {
            var parser = RustTypeParser.CreateParser();
            var parseResult = parser.TryParse(type);
            RustSimpleType flatType;
            if (!parseResult.WasSuccessful)
            {
                AddUnknownType(type);
                flatType = new(){Name = type};
            }
            else
            {
                flatType = FlattenPropertyType(parseResult.Value);
            }

            var property = new StringBuilder();
            if (!string.IsNullOrEmpty(flatType.ConvertAttributeName))
            {
                property.AppendLine($"[{flatType.ConvertAttributeName}]");
            }

            property.AppendLine($"{PropertyTab}public {flatType.Name} {CamelCase(name)} {{ get; set; }}");

            return property.ToString();
        }

        private static RustSimpleType FlattenPropertyType(RustType parseResult)
        {
            return parseResult.Type.Match(
                FlattenGeneric,
                FlattenTuple,
                simple =>
                {
                    if (!KnownTypes.Contains(simple.Name))
                    {
                        AddUnknownType(simple.Name);
                    }
                    
                    return simple;
                });
        }

        private static RustSimpleType FlattenGeneric(RustGeneric generic)
        {
            return generic switch
            {
                {GenericName: "Vec", GenericParams: {Count: 1}} => FlattenVec(generic.GenericParams[0]),
                {GenericName: "Box", GenericParams: {Count: 1}} => FlattenPropertyType(generic.GenericParams[0]),
                {GenericName: "Option", GenericParams: {Count: 1}} => FlattenOption(generic.GenericParams[0]),
                {GenericName: "Compact"} => FlattenCompact(),
                {GenericName: "CodeHash"} => new () { Name = "Hash"},
                _ => FlattenUnknownGeneric(generic)
            };
        }

        private static RustSimpleType FlattenCompact()
        {
            return new RustSimpleType()
            {
                Name = "System.Numerics.BigInteger",
                ConvertAttributeName = "CompactBigIntegerConverter"
            };
        }

        private static RustSimpleType FlattenUnknownGeneric(RustGeneric generic)
        {
            AddUnknownType(generic.GenericName);
            return new RustSimpleType()
            {
                Name = $"{generic.GenericName}<{string.Join(", ", generic.GenericParams.Select(FlattenPropertyType))}>"
            };
        }

        private static RustSimpleType FlattenOption(RustType innerType)
        {
            var flattenedInnerType = FlattenPropertyType(innerType);

            return new ()
            {
                Name = $"OneOf.OneOf<OneOf.Types.None, {flattenedInnerType.Name}>",
                ConvertAttributeName = string.IsNullOrEmpty(flattenedInnerType.ConvertAttributeName)
                    ? "OneOfConverter"
                    : $"OneOfConverter(ItemConverters = new []{{ null, typeof({flattenedInnerType.ConvertAttributeName})}})"
            };
        }

        private static RustSimpleType FlattenTuple(RustTuple tuple)
        {
            var types = tuple.RustTypes.Select(FlattenPropertyType).ToList();

            var typeNames = types.Select(t => t.Name);
            var itemConverters = types.Select(t => string.IsNullOrEmpty(t.ConvertAttributeName) ? "null" : t.ConvertAttributeName);
            return new ()
            {
                Name = $"Tuple<{string.Join(", ", typeNames)}>",
                ConvertAttributeName = $"TupleConverter(ItemConverters = new [] {{ {string.Join(", ", itemConverters)} }})"
            };
        }

        private static RustSimpleType FlattenVec(RustType elementType)
        {
            var flattenedElementType = FlattenPropertyType(elementType);
            return new RustSimpleType
            {
                Name = $"{flattenedElementType.Name}[]",
                ConvertAttributeName = string.IsNullOrEmpty(flattenedElementType.ConvertAttributeName)
                    ? "PrefixedArrayConverter"
                    : $"PrefixedArrayConverter(ItemConverter = typeof({flattenedElementType.ConvertAttributeName}))"
            };
        }

        private static string CamelCase(string str)
        {
            var sb = new StringBuilder(str);
            for (int i = 0; i < sb.Length; i++)
            {
                if (sb[i] == '_')
                {
                    sb.Remove(i, 1);
                    if (char.IsLetter(sb[i]))
                    {
                        sb[i] = char.ToUpper(sb[i]);
                    }
                }
            }

            if (char.IsLetter(sb[0]))
            {
                sb[0] = char.ToUpper(sb[0]);
            }
            return sb.ToString();
        }

        private static void StorageHashes(MetadataBase metadata, IApplication app)
        {
            var sb = new StringBuilder();

            foreach (var module in metadata.GetModules())
            {
                foreach (var storage in module.GetStorages())
                {
                    var key = app.StorageApi.GetKeys(module.GetName(), storage.GetName());
                    sb.AppendLine($"{module.GetName()}.{storage.GetName()} = {key}");
                }
            }
        }

        private static void AddUnknownType(string type)
        {
            if (type == "CodeHash")
            {
                
            }
            UnknownTypes.Add(type);
        }
    }
}