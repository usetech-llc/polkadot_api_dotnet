using System;

namespace Polkadot.Exceptions
{
    public class UnsupportedTransactionVersionException: Exception
    {
        public UnsupportedTransactionVersionException(byte transactionVersion): base($"Unable to deserialize extrinsic, transaction version {transactionVersion} is not supported.")
        {
        }
    }
}