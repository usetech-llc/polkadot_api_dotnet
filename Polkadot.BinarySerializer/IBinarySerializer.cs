using System;
using System.Collections.Generic;
using System.IO;

namespace Polkadot.BinarySerializer
{
    public interface IBinarySerializer
    {
        byte[] Serialize<T>(T value);
        void Serialize<T>(T value, Stream stream);
        T Deserialize<T>(Stream stream);
        object Deserialize(Type type, Stream stream);
        T Deserialize<T>(byte[] data);
        object CreateObject(Type type);
        IBinaryConverter GetConverter(Type type);

        Type GetCallType(byte moduleIndex, byte methodIndex);
        (byte moduleIndex, byte methodIndex) GetCallIndex(Type typeOfCall);
        Type GetEventType(byte moduleIndex, byte eventIndex);
        (byte moduleIndex, byte eventIndex) GetEventIndex(Type typeOfEvent);
    }
}