using Newtonsoft.Json.Linq;
using Polkadot.Exceptions;

namespace Polkadot.Utils
{
    public static class RequestExtensions
    {
        public static JObject Unwrap(this (JObject Result, JObject Error) response)
        {
            if (response.Result != null)
            {
                return response.Result;
            }

            if (response.Error != null)
            {
                throw new JrpcErrorException(response.Error);
            }

            return null;
        }
    }
}