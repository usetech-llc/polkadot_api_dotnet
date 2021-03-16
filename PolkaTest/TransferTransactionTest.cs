using System;
using System.Numerics;
using System.Threading.Tasks;
using Polkadot.Api;
using Polkadot.BinaryContracts.Calls.Balances;
using Polkadot.DataStructs;
using Polkadot.Source.Utils;
using Polkadot.Utils;
using Xunit;
using Xunit.Abstractions;

namespace PolkaTest
{
    [Collection("Sequential")]
    public class TransferTransactionTest
    {
        private readonly ITestOutputHelper _outputHelper;

        public TransferTransactionTest(ITestOutputHelper outputHelper)
        {
            _outputHelper = outputHelper;
        }

        [Fact]
        public async Task CanSendMoney()
        {
            var from = Constants.LocalAliceAddress;
            var privateKey = Constants.LocalAlicePrivateKey;
            var to = Constants.LocalBobAddress;
            
            using var application = PolkaApi.GetApplication();

            application.Connect(Constants.LocalNodeUri);

            var nonceBefore = application.GetAccountNonce(from);
            var taskCompletionSource = new TaskCompletionSource<string>();
            var result = application.SignAndSendTransfer(
                from.Symbols, 
                privateKey.ToHexString(), 
                to.Symbols, 
                BigInteger.One, 
                str =>
                {
                    if (taskCompletionSource.Task.Status != TaskStatus.RanToCompletion)
                    {
                        taskCompletionSource.SetResult(str);
                    }
                });

            await taskCompletionSource.Task.WithTimeout(TimeSpan.FromMinutes(2));
            await Task.Delay(TimeSpan.FromSeconds(10));
            var nonceAfter = application.GetAccountNonce(from);
            
            Assert.Equal(nonceBefore + 1, nonceAfter);
        }

        [Fact]
        public async Task CanDetectSuccessfulTransaction()
        {
            var from = Constants.LocalAliceAddress;
            var privateKey = Constants.LocalAlicePrivateKey;
            var to = Constants.LocalBobAddress;
            
            using var application = PolkaApi.GetApplication();

            application.Connect(Constants.LocalNodeUri);

            var result = await application.SignAndWaitForResult(from, privateKey, new TransferCall(AddressUtils.GetPublicKeyFromAddr(to), BigInteger.One));
            
            Assert.True(result.IsT0);
        }
        
        [Fact]
        public async Task CanTransferTwoTransactionsSimultaneously()
        {
            var from = Constants.LocalAliceAddress;
            var privateKey = Constants.LocalAlicePrivateKey;
            var to = Constants.LocalBobAddress;
            
            using var application = PolkaApi.GetApplication();
            application.Connect(Constants.LocalNodeUri);

            var result = await Task.WhenAll(
                Task.Run(async () => await application.SignWaitRetryOnLowPriority(@from, privateKey, new TransferCall(AddressUtils.GetPublicKeyFromAddr(to), BigInteger.One))),
                Task.Run(async () => await application.SignWaitRetryOnLowPriority(@from, privateKey, new TransferCall(AddressUtils.GetPublicKeyFromAddr(to), BigInteger.One)))
            );
            
            Assert.True(result[0].IsT0);
            Assert.True(result[1].IsT0);
        }
    }
}