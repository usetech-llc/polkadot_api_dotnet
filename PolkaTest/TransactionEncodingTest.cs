using System;
using System.Numerics;
using System.Text;
using Polkadot;
using Polkadot.Api;
using Polkadot.BinaryContracts;
using Polkadot.BinaryContracts.Calls;
using Polkadot.BinaryContracts.Calls.Balances;
using Polkadot.BinaryContracts.Extrinsic;
using Polkadot.BinarySerializer.Extensions;
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
            public MockedApplication(ILogger logger, IJsonRpc jsonRpc) : base(logger, jsonRpc, Application.DefaultSubstrateSettings())
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
                            Number = 1
                        }
                    }
                };
            }

            public override BlockHeader GetBlockHeader(GetBlockParams param)
            {
                return GetBlock(null).Block.Header;
            }
        }
        
        [Fact]
        public void TransactionEncodedCorrectly()
        {
            var expectedTransaction =
                "290284586CC32614D6A3F219667DB501ADE545753058D43B14E6E971E9C9CC908AD84301.{128}0A001D010006008EAF04151687736326C9FEA17E25FC5287613693C912909CB226AA4794F26A4804";
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

        [Fact]
        public void SerializingDeserializedTransactionMakesSameValue()
        {
            var transactionSample = "290284586CC32614D6A3F219667DB501ADE545753058D43B14E6E971E9C9CC908AD84301B6E14B633A7AE50BFE488E6E0AC4E35D870593264F1AF92FE4BC650879313B709132209719EB41CCFCE75D819E64B7DE6AB51A4C2FA1720C0C939541FAA1958D0A001D010006038EAF04151687736326C9FEA17E25FC5287613693C912909CB226AA4794F26A4804";
            
            using var application = MockedApplication.Create();
            application.Connect(Constants.LocalNodeUri);
            var serializer = application.CreateSerializer();
            var deserializedTransaction = serializer
                .DeserializeAssertReadAll<
                    AsByteVec<UncheckedExtrinsic<ExtrinsicAddress, ExtrinsicMultiSignature, SignedExtra,
                        ExtrinsicCallRaw<TransferCall>>>>(transactionSample.HexToByteArray());
            var serializedTransaction = serializer.Serialize(deserializedTransaction)
                .ToHexString();
            
            Assert.Equal(transactionSample, serializedTransaction);
        } 
    }
}