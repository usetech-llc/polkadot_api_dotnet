namespace Polkadot.Source.Utils
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;

    public static class JsonParse
    {
        public static KeyValuePair<string, string> ParseJsonKeyValuePair(string paramString)
        {
            // Parse parameters
            try
            {
                dynamic json = JsonConvert.DeserializeObject(paramString);
                var type = json["type"].ToString();
                var value = json["value"].ToString();
                return new KeyValuePair<string, string>(type, value);
            }
            catch (Exception e) {
                throw new ApplicationException($"Invalid parameter string was provided. " +
                    $"Should be a JSON string and have two fields: type and value. Provided: {paramString} "+
                    $"Error message: {e.Message}");
            }
        }
    }
}
