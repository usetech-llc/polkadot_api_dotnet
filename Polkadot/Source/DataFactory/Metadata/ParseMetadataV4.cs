using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json.Linq;
using Polkadot.DataStructs.Metadata;
using Polkadot.Source.Utils;

namespace Polkadot.DataFactory.Metadata
{
    public class ParseMetadataV4 : IParseFactory<MetadataV4>
    {
        public MetadataV4 Parse(JObject json)
        {
            var str = json["result"].ToString().Substring(2);
            // magic bytes
            var magic1 = Scale.NextByte(ref str);
            var magic2 = Scale.NextByte(ref str);
            var magic3 = Scale.NextByte(ref str);
            var magic4 = Scale.NextByte(ref str);
            var magic5 = Scale.NextByte(ref str);

            var result = new MetadataV4();

            var moduleList = new List<ModuleV4>();
            var mLen = Scale.DecodeCompactInteger(ref str);

            for (var moduleIndex = 0; moduleIndex < mLen.Value; moduleIndex++)
            {
                var module = new ModuleV4();

                // get module name
                var moduleNameLen = Scale.DecodeCompactInteger(ref str);
                module.Name = Scale.ExtractString(ref str, moduleNameLen.Value);

                // get module prefix
                var modulePrefixLen = Scale.DecodeCompactInteger(ref str);
                module.Prefix = Scale.ExtractString(ref str, modulePrefixLen.Value);

                // ---------- Storage
                // storage is not null
                var storageIsset = Scale.NextByte(ref str);
                if (storageIsset != 0)
                {
                    var storageList = new List<StorageV4>();
                    var storageLen = Scale.DecodeCompactInteger(ref str);
                    if (storageLen.Value == 0)
                    {
                        storageList.Add(new StorageV4());
                    }

                    for (int i = 0; i < storageLen.Value; i++)
                    {
                        storageList.Add(GetStorageV4(ref str));
                    }

                    module.Storage = storageList.ToArray();
                }

                // ---------- Calls
                // calls is not null
                var callsIsset = Scale.NextByte(ref str);
                if (callsIsset != 0)
                {
                    var callList = new List<CallV4>();
                    var callsCount = Scale.DecodeCompactInteger(ref str);
                    if (callsCount.Value == 0)
                    {
                        callList.Add(new CallV4());
                    }

                    for (int i = 0; i < callsCount.Value; i++)
                    {
                        callList.Add(GetCallV4(ref str));
                    }
                    module.Call = callList.ToArray();
                }

                // ---------- Events
                // events is not null
                var eventsIsset = Scale.NextByte(ref str);
                if (eventsIsset != 0)
                {
                    var eventList = new List<EventArgV4>();
                    var eventsCount = Scale.DecodeCompactInteger(ref str);

                    if (eventsCount.Value == 0)
                    {
                        eventList.Add(new EventArgV4());
                    }

                    for (int i = 0; i < eventsCount.Value; i++)
                    {
                        eventList.Add(GetEventV4(ref str));
                    }

                    module.Ev = eventList.ToArray();
                }

                moduleList.Add(module);
            }
            result.Module = moduleList.ToArray();
            return result;
        }

        private EventArgV4 GetEventV4(ref string str)
        {
            var ea = new EventArgV4();

            var callNameLen = Scale.DecodeCompactInteger(ref str);
            ea.Name = Scale.ExtractString(ref str, callNameLen.Value);

            // args count
            var argList = new List<string>();
            var args = Scale.DecodeCompactInteger(ref str);
            for(var i = 0; i < args.Value; i++)
            {
                var argLen = Scale.DecodeCompactInteger(ref str);
                argList.Add(Scale.ExtractString(ref str, argLen.Value));
            }
            ea.Args = argList.ToArray();

            // documents count
            var docList = new List<string>();
            var docCount = Scale.DecodeCompactInteger(ref str);
            for (var i = 0; i < docCount.Value; i++)
            {
                var docStringLen = Scale.DecodeCompactInteger(ref str);
                docList.Add(Scale.ExtractString(ref str, docStringLen.Value));
            }
            ea.Documentation = docList.ToArray();

            return ea;
        }

        private CallV4 GetCallV4(ref string str)
        {
            var call = new CallV4();

            var callNameLen = Scale.DecodeCompactInteger(ref str);
            call.Name = Scale.ExtractString(ref str, callNameLen.Value);

            // args count
            var argList = new List<FunctionCallArgV4>();
            var args = Scale.DecodeCompactInteger(ref str);
            for (var i = 0; i < args.Value; i++)
            {
                var fca = new FunctionCallArgV4();
                var argNameLen = Scale.DecodeCompactInteger(ref str);
                fca.Name = Scale.ExtractString(ref str, argNameLen.Value);

                var argTypeLen = Scale.DecodeCompactInteger(ref str);
                fca.Type = Scale.ExtractString(ref str, argTypeLen.Value);

                argList.Add(fca);
            }
            call.Args = argList.ToArray();

            // documents count
            var docList = new List<string>();
            var docCount = Scale.DecodeCompactInteger(ref str);
            for (var i = 0; i < docCount.Value; i++)
            {
                var docStringLen = Scale.DecodeCompactInteger(ref str);
                var docItem = Scale.ExtractString(ref str, docStringLen.Value);
                docList.Add(docItem);
            }
            call.Documentation = docList.ToArray();

            return call;
        }

        private StorageV4 GetStorageV4(ref string str)
        {
            var storage = new StorageV4();

            var storageNameLen = Scale.DecodeCompactInteger(ref str);
            storage.Name = Scale.ExtractString(ref str, storageNameLen.Value);

            storage.Modifier = Scale.NextByte(ref str);
            storage.Type = new FuncTypeV4
            {
                Type = Scale.NextByte(ref str)
            };

            var type1Len = Scale.DecodeCompactInteger(ref str);
            var type1 = Scale.ExtractString(ref str, type1Len.Value);
            storage.Type.Key1 = type1;

            if (storage.Type.Type != 0)
            {
                var secondTypeLen = Scale.DecodeCompactInteger(ref str);
                storage.Type.Key2 = Scale.ExtractString(ref str, secondTypeLen.Value);
                storage.Type.Hasher = Scale.NextByte(ref str);
            }

            // extract fallback as raw hex
            var fallbackLen = Scale.DecodeCompactInteger(ref str);
            var fallback = str.Substring(0, (int)fallbackLen.Value * 2);
            str = str.Substring((int)fallbackLen.Value * 2);
            storage.Fallback = fallback;

            // documents count
            var docList = new List<string>();
            var docCount = Scale.DecodeCompactInteger(ref str);
            for (int di = 0; di < docCount.Value; di++)
            {
                var docStringLen = Scale.DecodeCompactInteger(ref str);
                var docItem = Scale.ExtractString(ref str, docStringLen.Value);
                docList.Add(docItem);
            }
            storage.Documentation = docList.ToArray();

            return storage;
        }
    }
}
