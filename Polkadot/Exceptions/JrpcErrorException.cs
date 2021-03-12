using System;
using Newtonsoft.Json.Linq;

namespace Polkadot.Exceptions
{
    public class JrpcErrorException : Exception
    {
        public long Code { get; set; }
        public JObject Error { get; set; }

        public JrpcErrorException(JObject error)
        {
            Error = error;
            Code = error["code"].Value<long>();
        }
    }
}