namespace Polkadot.Api.Client.Model
{
    public class CallRequest
    {
        public static CallRequest<TAccountId, TBalance, TGasLimit, TInput>
            Create<TAccountId, TBalance, TGasLimit, TInput>(TAccountId origin, TAccountId dest, TBalance value,
                TGasLimit gasLimit, TInput inputData)
        {
            return new(origin, dest, value, gasLimit, inputData);
        }

    }
    public class CallRequest<TAccountId, TBalance, TGasLimit, TInput>
    {
        public TAccountId Origin { get; set; }
        public TAccountId Dest { get; set; }
        public TBalance Value { get; set; }
        public TGasLimit GasLimit { get; set; }
        public TInput InputData { get; set; }

        public CallRequest(TAccountId origin, TAccountId dest, TBalance value, TGasLimit gasLimit, TInput inputData)
        {
            Origin = origin;
            Dest = dest;
            Value = value;
            GasLimit = gasLimit;
            InputData = inputData;
        }
    }
}