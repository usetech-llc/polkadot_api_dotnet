using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Polkadot.JsonConverters
{
    public class TupleConverter : JsonConverter
    {
        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            var type = value.GetType();
            var array = new List<object>();
            FieldInfo fieldInfo;
            var i = 1;

            while ((fieldInfo = type.GetField($"Item{i++}")) != null)
                array.Add(fieldInfo.GetValue(value));

            serializer.Serialize(writer, array);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            var argTypes = objectType.GetGenericArguments();
            var array = serializer.Deserialize<JArray>(reader);
            var items = array.Select((a, index) => a.ToObject(argTypes[index])).ToArray();

            var constructor = objectType.GetConstructor(argTypes);
            return constructor.Invoke(items);
        }

        public override bool CanConvert(Type type)
        {
            return type.Name.StartsWith("ValueTuple`");
        }
    }
}