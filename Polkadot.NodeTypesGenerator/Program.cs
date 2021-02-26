using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using Humanizer;
using Microsoft.Extensions.Configuration;
using Polkadot.Api;
using Polkadot.BinarySerializer.Converters;
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
            "BigInteger",
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
            "DispatchInfo",
            "DispatchError",
            "Polkadot.BinaryContracts.Events.BalanceStatusEnum.BalanceStatus",
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
            GenerateCustomTypes(configuration);
            GenerateCalls(configuration, metadata);
            GenerateEvents(configuration, metadata);

            UnknownTypes.ExceptWith(KnownTypes);
            if (UnknownTypes.Any())
            {
                Console.WriteLine("Unknown types:");
                Console.WriteLine(string.Join(Environment.NewLine, UnknownTypes));
            }
        }

        private static void GenerateCustomTypes(Configuration configuration)
        {
            if (string.IsNullOrEmpty(configuration.CustomTypesPath))
            {
                return;
            }
            Directory.CreateDirectory($"{configuration.Output}/Generated");
            Directory.CreateDirectory($"{configuration.Output}/Generated/Common");

            var customTypesString = File.ReadAllText(configuration.CustomTypesPath);
            var json = JsonDocument.Parse(customTypesString,
                new JsonDocumentOptions() {CommentHandling = JsonCommentHandling.Skip,});

            GenerateCustomTypes(configuration, json.RootElement);
        }

        private static void GenerateCustomTypes(Configuration configuration, JsonElement json, string @namespace = null, string path = null)
        {
            foreach (var property in json.EnumerateObject())
            {
                var properties = GetCustomTypeProperties(configuration, property);
                var className = PascalCase(property.Name);
                @namespace ??= Namespace("Common");
                var @class = GenerateClass(properties.ToList(), className, @namespace, null);

                path ??= $"{configuration.Output}/Generated/Common";
                File.WriteAllText($"{path}/{className}.cs", @class);
            }
        }

        private static IEnumerable<Property> GetCustomTypeProperties(Configuration configuration, JsonProperty property)
        {
            return property.Value.ValueKind switch
            {
                JsonValueKind.String => CustomTypeStringProperties(property.Value.GetString()),
                JsonValueKind.Object => CustomTypeObjectProperties(configuration, property),
                JsonValueKind.Null => Array.Empty<Property>()
            };
        }

        private static IEnumerable<Property> CustomTypeObjectProperties(Configuration configuration, JsonProperty property)
        {
            var properties = property.Value.EnumerateObject().ToList();
            if (properties.Count == 1 && string.Equals(properties[0].Name, "_enum"))
            {
                return CustomTypeEnum(configuration, properties[0].Value, property.Name);
            }

            RustSimpleType ObjectType(JsonElement element, string property)
            {
                GenerateCustomTypes(configuration, element);
                return new RustSimpleType()
                {
                    Name = $"{PascalCase(property)}Property",
                };
            }
            
            IEnumerable<Property> CustomTypeObject()
            {
                foreach (var jsonProperty in properties)
                {
                    var type = jsonProperty.Value switch
                    {
                        {ValueKind: JsonValueKind.String} v => ParseType(null, v.GetString()),
                        {ValueKind: JsonValueKind.Object} v => ObjectType(v, jsonProperty.Name) 
                    };

                    yield return new Property()
                    {
                        PropertyName = PascalCase(jsonProperty.Name),
                        OriginalType = ReplaceNewLines(jsonProperty.Value.GetRawText()),
                        Type = type.Name,
                        ConverterAttribute = type.ConvertAttributeName
                    };
                }
            }

            return CustomTypeObject();
        }

        private static string ReplaceNewLines(string s)
        {
            return Regex.Replace(s, "\r|\n", " ");
        }

        private static IEnumerable<Property> CustomTypeEnum(Configuration configuration, JsonElement value, string mainClassName)
        {
            var lastNamespacePart = PluralizeLastPascalCaseWord(PascalCase(mainClassName));
            var rootDirectory = $"{configuration.Output}/Generated/Common/{lastNamespacePart}";
            Directory.CreateDirectory(rootDirectory);
            var enumNamespace = Namespace("Common", lastNamespacePart);
            string enumCases;
            switch (value.ValueKind)
            {
                case JsonValueKind.Object:
                {
                    enumCases = string.Join(", ", value.EnumerateObject().Select(v => $"{enumNamespace}.{PascalCase(v.Name)}"));
                    GenerateCustomTypes(configuration, value, enumNamespace, rootDirectory); break;
                }
                case JsonValueKind.Array:
                {
                    foreach (var element in value.EnumerateArray())
                    {
                        if (element.ValueKind != JsonValueKind.String)
                        {
                            throw new Exception("Unknown non string array enum syntax");
                        }

                        var className = PascalCase(element.GetString());
                        var @class = GenerateClass(Array.Empty<Property>(), className, enumNamespace, null);
                        File.WriteAllText($"{rootDirectory}/{className}.cs", @class);
                    }
                    enumCases = string.Join(", ", value.EnumerateArray().Select(v => $"{enumNamespace}.{PascalCase(v.GetString())}"));
                    break;
                }
                default: throw new Exception("Unknown enum format.");
            }

            yield return new Property()
            {
                Type = $"OneOf.OneOf<{enumCases}>",
                ConverterAttribute = nameof(OneOfConverter),
                OriginalType = ReplaceNewLines(value.GetRawText()),
                PropertyName = "Value"
            };
        }

        private static string PluralizeLastPascalCaseWord(string pascalCase)
        {
            var words = Regex.Split(pascalCase, "(?=[A-Z])");
            var lastWord = words[^1];
            var plural = lastWord.Pluralize();
            if (string.Equals(lastWord, plural, StringComparison.Ordinal))
            {
                plural = $"{plural}Entries";
            }

            words[^1] = plural;
            return string.Join("", words);
        }

        private static IEnumerable<Property> CustomTypeStringProperties(string value)
        {
            var parsed = ParseType(null, value);
            yield return new Property()
            {
                Type = parsed.Name,
                ConverterAttribute = parsed.ConvertAttributeName,
                OriginalType = value,
                PropertyName = "Value"
            };
        }

        private static void GenerateEvents(Configuration configuration, MetadataBase metadata)
        {
            Directory.CreateDirectory($"{configuration.Output}/Generated");
            Directory.CreateDirectory($"{configuration.Output}/Generated/Events");

            var eventRegistrations = new StringBuilder();
            foreach (var module in metadata.GetModules())
            {
                Directory.CreateDirectory($"{configuration.Output}/Generated/Events/{PascalCase(module.GetName())}");
                foreach (var eventMeta in module.GetEvents())
                {
                    var properties = eventMeta.GetArguments()
                        .Select((type, index) =>
                        {
                            var parsed = ParseType(module.GetName(), type);
                            return new Property()
                            {
                                Type = parsed.Name,
                                ConverterAttribute = parsed.ConvertAttributeName,
                                OriginalType = type,
                                PropertyName = $"EventArgument{index}"
                            };
                        }).ToList();
                    var className = PascalCase(eventMeta.GetName());
                    var @namespace = Namespace("Events", PascalCase(module.GetName()));
                    var eventClass = GenerateClass(properties, className, @namespace, "IEvent");
                    File.WriteAllText($"{configuration.Output}/Generated/Events/{PascalCase(module.GetName())}/{className}.cs", eventClass);

                    eventRegistrations.AppendLine(
                        $"{PropertyTab}{Tab}{Tab}.AddEvent<{@namespace}.{className}>(\"{module.GetName()}\", \"{eventMeta.GetName()}\")");
                }
            }

            var application = ApplicationPartialMethod("RegisterGeneratedEvents", eventRegistrations.ToString());
            File.WriteAllText($"{configuration.Output}/Generated/Events/Application.events.cs", application);
        }

        private static void GenerateCalls(Configuration configuration, MetadataBase metadata)
        {
            Directory.CreateDirectory($"{configuration.Output}/Generated");
            Directory.CreateDirectory($"{configuration.Output}/Generated/Calls");
            var serializerRegistrations = new StringBuilder();
            foreach (var module in metadata.GetModules())
            {
                Directory.CreateDirectory($"{configuration.Output}/Generated/Calls/{PascalCase(module.GetName())}");
                foreach (var callRegistration in CreateModuleCalls(module, configuration))
                {
                    serializerRegistrations.AppendLine(callRegistration);
                }
            }


            var registerCalls = ApplicationPartialMethod("RegisterGeneratedCalls", serializerRegistrations.ToString());
            File.WriteAllText($"{configuration.Output}/Generated/Calls/Application.calls.cs", registerCalls);
        }

        private static string ApplicationPartialMethod(string methodName, string registrations)
        {
            var applicationClass = $@"using Polkadot.BinarySerializer;

namespace Polkadot.Api
{{
    public partial class Application : IApplication, IWebSocketMessageObserver
    {{
        public static void {methodName}(SerializerSettings settings) 
        {{  
            settings
{registrations};
        }}
    }}
}}";
            return applicationClass;
        }

        private static IEnumerable<string> CreateModuleCalls(IModuleMeta module, Configuration configuration)
        {
            foreach (var call in module.GetCalls())
            {
                var properties = call.GetArguments()
                    .Select(a =>
                    {
                        var parsed = ParseType(module.GetName(), a.Type);
                        return new Property()
                        {
                            Type = parsed.Name,
                            ConverterAttribute = parsed.ConvertAttributeName,
                            PropertyName = a.Name,
                            OriginalType = a.Type
                        };
                    }).ToList();
                var className = $"{PascalCase(call.GetName())}Call";
                var @namespace = Namespace("Calls", PascalCase(module.GetName()));

                var callClass = GenerateClass(properties, className, @namespace, "IExtrinsicCall");
                
                File.WriteAllText($"{configuration.Output}/Generated/Calls/{PascalCase(module.GetName())}/{className}.cs", callClass);
                yield return @$"{PropertyTab}{Tab}{Tab}.AddCall<{@namespace}.{className}>(""{module.GetName()}"", ""{call.GetName()}"")";
            }
        }

        private static string Namespace(params string[] segments)
        {
            return string.Join(".", new[] {"Polkadot.BinaryContracts"}.Concat(segments));
        }

        public static string GenerateClass(ICollection<Property> properties, string className, string @namespace, string @interface)
        {
            KnownTypes.Add(className);
            string FormatAttribute(string attribute) => string.IsNullOrEmpty(attribute)
                ? ""
                : $"{Environment.NewLine}{PropertyTab}[{attribute}]";

            var propertiesStr = string.Join($"{Environment.NewLine}{Environment.NewLine}",
                properties.Select((p, i) => $@"{PropertyTab}// Rust type {p.OriginalType}
{PropertyTab}[Serialize({i})]{FormatAttribute(p.ConverterAttribute)}
{PropertyTab}public {p.Type} {PascalCase(p.PropertyName)} {{ get; set; }}
"));

            var constructorParams = string.Join(", ", properties.Select(p => $"{p.Type} @{CamelCase(p.PropertyName)}"));
            var constructorSetters = string.Join($"{Environment.NewLine}",
                properties.Select(p => $"{PropertyTab}{Tab}this.{PascalCase(p.PropertyName)} = @{CamelCase(p.PropertyName)};"));
            var parameterfullConstructor = properties.Any() ? $@"{PropertyTab}public {className}({constructorParams})
{PropertyTab}{{
{constructorSetters}
{PropertyTab}}}" : "";
                
                
            var constructors = $@"
{PropertyTab}public {className}() {{ }}
{parameterfullConstructor}
";

            var implements = string.IsNullOrEmpty(@interface) ? "" : $" : {@interface}";

            var @class = $@"using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace {@namespace}
{{
{Tab}public partial class {className}{implements}
{Tab}{{
{propertiesStr}

{constructors}
{Tab}}}
}}";
            return @class;
        }

        private static RustSimpleType ParseType(string module, string type)
        {
            var parser = RustTypeParser.CreateParser(module);
            var parseResult = parser.TryParse(type);
            if (!parseResult.WasSuccessful)
            {
                AddUnknownType(type);
                return FlattenPropertyType(new(){ Type = new RustSimpleType() {Name = type}});
            }

            return FlattenPropertyType(parseResult.Value);
        }

        private static RustSimpleType FlattenPropertyType(RustType parseResult)
        {
            return parseResult.Type.Match(
                FlattenGeneric,
                FlattenTuple,
                simple =>
                {
                    AddUnknownType(simple.Name);
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
            return new()
            {
                Name = "BigInteger",
                ConvertAttributeName = "CompactBigIntegerConverter"
            };
        }

        private static RustSimpleType FlattenUnknownGeneric(RustGeneric generic)
        {
            AddUnknownType(generic.GenericName);
            return new RustSimpleType()
            {
                Name = $"{generic.GenericName}<{string.Join(", ", generic.GenericParams.Select(FlattenPropertyType).Select(t => t.Name))}>"
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

        private static string PascalCase(string str)
        {
            return str.Pascalize();
        }

        private static string CamelCase(string str)
        {
            return str.Camelize();
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

            if (!KnownTypes.Contains(type))
            {
                UnknownTypes.Add(type);
            }
        }
    }
}