using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using Polkadot.Api.Client;
using Polkadot.Api.Client.Serialization;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Xunit.Abstractions;

namespace PolkaTest
{
    public class Constants
    {
        public const string KusamaAccount1Address = "FpVmbKAqwoQaTBGwrTGsSu4nN1PCGY8XrQa4AaJVuzyVhh2";
        public const string KusamaAccount2Address = "5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY";

        public static readonly Address LocalAliceAddress = new Address("5GrwvaEF5zXb26Fz9rcQpDWS57CtERHpNehXCPcNoHGKutQY");
        public static readonly Address LocalAccountWithKey = new Address("5E4eP18dC346yx1PRXBFbyaAVzXfGqf2TMcbBkyWr28K4f3Q");
        
        public static readonly byte[] LocalAlicePrivateKey = "0x33A6F3093F158A7109F679410BEF1A0C54168145E0CECB4DF006C1C2FFFB1F09925A225D97AA00682D6A59B95B18780C10D7032336E88F3442B42361F4A66011".HexToByteArray();

        public static readonly byte[] LocalAccountPrivateKey = {
            25, 213, 29, 81, 62, 79, 15, 251, 133, 76, 195, 26, 105, 73, 195, 72, 250, 71, 29, 191, 218, 137, 226, 179,
            177, 231, 181, 184, 231, 131, 87, 8, 34, 136, 220, 254, 5, 36, 13, 150, 131, 44, 182, 66, 174, 140, 83, 204,
            30, 106, 8, 246, 45, 73, 25, 47, 15, 182, 26, 197, 26, 125, 25, 119
        };
        public static readonly Address LocalBobAddress = new Address("5FHneW46xGXgs5mUiveU4sbTyGBzmstUspZC92UhjJM694ty");

        public const string LocalNodeUri = "ws://localhost:9944/";

        public static ISubstrateClient<TextJsonElement> LocalClient(ITestOutputHelper output = null)
        {
            var client = SubstrateClient.FromSettings(SubstrateClientSettings.Default() with
            {
                RpcEndpoint = LocalNodeUri,
                RpcTimeout = TimeSpan.FromHours(4)
            });

            if (output != null)
            {
                client.Sending += b => output.WriteLine($"Sending: {PrettyJson(b)}");
                client.Received += b => output.WriteLine($"Received: {PrettyJson(b)}");
                client.Error += ex => output.WriteLine($"Error: {ex}");
            }

            return client;
        }

        private static string PrettyJson(byte[] utf8)
        {
            using var doc = JsonDocument.Parse(
                utf8,
                new JsonDocumentOptions
                {
                    AllowTrailingCommas = true
                }
            );
            MemoryStream memoryStream = new MemoryStream();
            using (
                var utf8JsonWriter = new Utf8JsonWriter(
                    memoryStream,
                    new JsonWriterOptions
                    {
                        Indented = true
                    }
                )
            )
            {
                doc.WriteTo(utf8JsonWriter);
            }
            return new System.Text.UTF8Encoding()
                .GetString(memoryStream.ToArray());
        }
    }
}