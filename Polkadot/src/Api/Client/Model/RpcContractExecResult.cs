using OneOf;
using Polkadot.Api.Client.Model.RpcContractExecResultValues;
using Polkadot.BinarySerializer.Converters;

namespace Polkadot.Api.Client.Model
{
    public class RpcContractExecResult<TData>
    {
        [OneOfConverter]
        public OneOf<Success<TData>, Error> Value { get; set; }
    }
}