using System.Threading;
using System.Threading.Tasks;

namespace Polkadot.Api.Client.Modules.Payment.Rpc
{
    public interface IPaymentRpc<TBlockHash>
    {
        Task<TResponse> QueryInfo<TResponse, TEncodedXt>(TEncodedXt encodedXt, TBlockHash at = default, CancellationToken token = default);
    }
}