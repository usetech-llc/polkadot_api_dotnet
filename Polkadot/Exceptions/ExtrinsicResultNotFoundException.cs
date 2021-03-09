using System;

namespace Polkadot.Exceptions
{
    public class ExtrinsicResultNotFoundException : Exception
    {
        public ExtrinsicResultNotFoundException() : base("Failed to find ExtrinsicSuccess or ExtrinsicFailed result.")
        {
        }
    }
}