namespace Polkadot.Api.Client.Model.RpcContractExecResultValues
{
    /// Successful execution
    public class Success<TData>
    {
        /// The return flags
        public uint Flags { get; set; }
        /// Output data
        public TData Data { get; set; }
        /// How much gas was consumed by the call.
        public ulong GasConsumed { get; set; }     
    }
}