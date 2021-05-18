using System;

namespace Polkadot.BinarySerializer
{
    [AttributeUsage(AttributeTargets.All)]
    public class ConverterAttribute : Attribute
    {
        /// <summary>
        /// Converter used to serialize value.
        /// </summary>
        public Type SerializeConverterType;
        /// <summary>
        /// Converter used to deserialize value.
        /// </summary>
        public Type DeserializeConverterType;

        /// <summary>
        /// Shortcut for setting both <see cref="SerializeConverterType"/> and <see cref="DeserializeConverterType"/> at the same time.
        /// </summary>
        public Type ConverterType
        {
            get => SerializeConverterType ?? DeserializeConverterType;
            set
            {
                SerializeConverterType = value;
                DeserializeConverterType = value;
            }
        }

        /// <summary>
        /// Parameter required to pass to <see cref="IBinaryConverter.Serialize"/> method.
        /// </summary>
        public object[] SerializeParameters;
        
        /// <summary>
        /// Parameter required to pass to <see cref="IBinaryConverter.Deserialize"/> method. 
        /// </summary>
        public object[] DeserializeParameters;

        /// <summary>
        /// Shortcut for setting both <see cref="SerializeParameters"/> and <see cref="DeserializeParameters"/> at the same time.
        /// </summary>
        public object[] Parameters
        {
            get => SerializeParameters ?? DeserializeParameters;
            set
            {
                SerializeParameters = value;
                DeserializeParameters = value;
            }
        }

        public ConverterAttribute()
        {
        }
    }
}