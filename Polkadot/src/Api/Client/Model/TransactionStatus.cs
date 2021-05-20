using OneOf;
using Polkadot.Api.Client.Model.TransactionStatusValues;

namespace Polkadot.Api.Client.Model
{
    public class TransactionStatus<THash, TBlockHash>
    {
        	public OneOf<
	            Future, 
	            Ready, 
	            Broadcast, 
	            InBlock<TBlockHash>, 
	            Retracted<TBlockHash>, 
	            FinalityTimeout<TBlockHash>, 
	            Finalized<TBlockHash>,
        		Usurped<THash>,
        		Dropped,
        		Invalid
            > Value { get; set; }
    }
}