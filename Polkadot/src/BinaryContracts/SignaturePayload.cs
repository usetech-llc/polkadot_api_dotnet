using System;
using System.Collections.Generic;
using System.Numerics;
using System.Threading;
using Polkadot.Api;
using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class SignaturePayload
    {
        private SignaturePayload()
        {
        }
        
        public static SignatureData<TCall, TExtra> Create<TExtra, TCall>(TCall call, TExtra extra, IEnumerable<object> signedExtra)
            where TCall : IExtrinsicCall
            where TExtra : IExtrinsicExtra
        {
            return new SignatureData<TCall, TExtra>(call, extra, signedExtra);
        }
    }
    
    public class SignatureData<TCall, TExtra>
        where TCall : IExtrinsicCall
        where TExtra : IExtrinsicExtra
    {
        [Serialize(0)]
        public TCall Call { get; set; }
        
        [Serialize(1)]
        public TExtra Extra { get; set; }
        
        [Serialize(2)]
        public IEnumerable<object> SignedExtra { get; set; }

        public SignatureData()
        {
        }

        public SignatureData(TCall call, TExtra extra, IEnumerable<object> signedExtra)
        {
            Call = call;
            Extra = extra;
            SignedExtra = signedExtra;
        }
    }
}