using System;
using System.Numerics;
using System.Text;
using Polkadot;
using Polkadot.Api;
using Polkadot.Data;
using Polkadot.DataStructs;
using Polkadot.Utils;
using Schnorrkel;
using Xunit;

namespace PolkaTest
{
    [Collection("Sequential")]
    public class TransactionEncodingTest
    {
        class MockedApplication : Application
        {
            public MockedApplication(ILogger logger, IJsonRpc jsonRpc) : base(logger, jsonRpc)
            {
            }

            public static MockedApplication Create()
            {
                JsonRpcParams param = new JsonRpcParams();
                param.JsonrpcVersion = "2.0";

                var logger = new Logger();
                var jsonrpc = new JsonRpc(new Wsclient(logger), logger, param);
                return new MockedApplication(logger, jsonrpc);
            }

            public override BigInteger GetAccountNonce(Address address)
            {
                return 71;
            }

            public override SignedBlock GetBlock(GetBlockParams param)
            {
                return new SignedBlock()
                {
                    Block = new Block()
                    {
                        Header = new BlockHeader()
                        {
                            Number = 123456
                        }
                    }
                };
            }
        }
        
        [Fact]
        public void TransactionEncodedCorrectly()
        {
            var expectedTransaction =
                "250284586CC32614D6A3F219667DB501ADE545753058D43B14E6E971E9C9CC908AD84301.{128}001D010005008EAF04151687736326C9FEA17E25FC5287613693C912909CB226AA4794F26A4804";
            using var application = MockedApplication.Create();
            application.Connect(Constants.LocalNodeUri);

            var from = Constants.LocalAccountWithKey;
            var to = Constants.LocalBobAddress;
            var privateKey = Constants.LocalAccountPrivateKey;
            var extrinsic = application.CreateSignedTransferExtrinsic(from, privateKey, to, BigInteger.One);
            var transaction = application.CreateSerializer()
                .Serialize(extrinsic)
                .ToHexString();
            
            Assert.Matches(expectedTransaction, transaction);
        }
    }
}