using System;
using System.Linq;
using System.Threading.Tasks;
using Polkadot.Api;
using Polkadot.BinaryContracts.Calls.Contracts;
using Polkadot.BinaryContracts.Events;
using Polkadot.BinarySerializer;
using Polkadot.BinarySerializer.Extensions;
using Polkadot.Utils;
using Xunit;

namespace PolkaTest
{
    public class SerializationTest
    {
        [Fact]
        public async Task EventSubscriptionDoesntFail()
        {
            using var app = PolkaApi.GetApplication();
            app.Connect(Constants.LocalNodeUri);

            var storageKey = app.StorageApi.GetKeys("System", "Events");
            var deserializedCount = 0;
            var tcs = new TaskCompletionSource<EventList>();
            app.StorageApi.SubscribeStorage(storageKey, s =>
            {
                try
                {
                    var list = app.Serializer.DeserializeAssertReadAll<EventList>(s.HexToByteArray());
                    tcs.SetResult(list);
                    deserializedCount++;
                }
                catch (Exception ex)
                {
                    tcs.SetException(ex);
                }

                deserializedCount++;
            });

            await tcs.Task;
            
            Assert.True(deserializedCount > 0);
        }

        class MockedParameter : IContractCallParameter
        {
            [Serialize(0)]
            public int Value { get; set; }
        }
        
        [Fact]
        public void ContractParamsSerialization()
        {
            var settings = new SerializerSettings().AddContractCallParameter<MockedParameter>("0xDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEAD".HexToByteArray(), "0xBEEF".HexToByteArray());
            var serializer = new BinarySerializer(new IndexResolver(), settings);

            var call = CallCall.Create(0, 0, new MockedParameter() {Value = 20}, serializer);
            var serialized = serializer.Serialize(call);
            
            Assert.Equal("0xDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEAD000018BEEF14000000".HexToByteArray(), serialized);
        } 
        
        [Fact]
        public void ContractParamsDeserialization()
        {
            var settings = new SerializerSettings().AddContractCallParameter<MockedParameter>("0xDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEAD".HexToByteArray(), "0xBEEF".HexToByteArray());
            var serializer = new BinarySerializer(new IndexResolver(), settings);

            var call = serializer.Deserialize<CallCall>("0xDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEAD000018BEEF14000000".HexToByteArray());
            
            Assert.Equal("0xDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEADDEAD".HexToByteArray(), call.Dest.Bytes);
            Assert.Equal(0, call.Value);
            Assert.Equal(0, call.GasLimit);
            var mockedParameter = Assert.IsType<MockedParameter>(call.Parameters);
            Assert.Equal(20, mockedParameter.Value);
        } 
    }
}