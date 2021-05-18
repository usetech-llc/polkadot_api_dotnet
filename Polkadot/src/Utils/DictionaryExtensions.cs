using System;
using System.Collections.Generic;

namespace Polkadot.Utils
{
    public static class DictionaryExtensions
    {
        public static TValue TryGetOrDefault<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, TValue defaultValue = default)
        {
            return dictionary.TryGetValue(key, out var value) ? value : defaultValue;
        }
        public static TValue TryGetOrCreate<TKey, TValue>(this IDictionary<TKey, TValue> dictionary, TKey key, Func<TValue> valueFactory)
        {
            if(dictionary.TryGetValue(key, out var value))
            {
                return value;
            }
            
            var createdValue = valueFactory();
            dictionary[key] = createdValue;
            return createdValue;
        }
    }
}