using Polkadot.BinarySerializer;

namespace Polkadot.Api
{
    public partial class Application : IApplication, IWebSocketMessageObserver
    {
        public static void RegisterGeneratedEvents(SerializerSettings settings) 
        {  
            settings
                .AddEvent<Polkadot.BinaryContracts.Events.System.ExtrinsicSuccess>("System", "ExtrinsicSuccess")
                .AddEvent<Polkadot.BinaryContracts.Events.System.ExtrinsicFailed>("System", "ExtrinsicFailed")
                .AddEvent<Polkadot.BinaryContracts.Events.System.CodeUpdated>("System", "CodeUpdated")
                .AddEvent<Polkadot.BinaryContracts.Events.System.NewAccount>("System", "NewAccount")
                .AddEvent<Polkadot.BinaryContracts.Events.System.KilledAccount>("System", "KilledAccount")
                .AddEvent<Polkadot.BinaryContracts.Events.Contracts.Instantiated>("Contracts", "Instantiated")
                .AddEvent<Polkadot.BinaryContracts.Events.Contracts.Evicted>("Contracts", "Evicted")
                .AddEvent<Polkadot.BinaryContracts.Events.Contracts.Restored>("Contracts", "Restored")
                .AddEvent<Polkadot.BinaryContracts.Events.Contracts.CodeStored>("Contracts", "CodeStored")
                .AddEvent<Polkadot.BinaryContracts.Events.Contracts.ScheduleUpdated>("Contracts", "ScheduleUpdated")
                .AddEvent<Polkadot.BinaryContracts.Events.Contracts.ContractExecution>("Contracts", "ContractExecution")
                .AddEvent<Polkadot.BinaryContracts.Events.Grandpa.NewAuthorities>("Grandpa", "NewAuthorities")
                .AddEvent<Polkadot.BinaryContracts.Events.Grandpa.Paused>("Grandpa", "Paused")
                .AddEvent<Polkadot.BinaryContracts.Events.Grandpa.Resumed>("Grandpa", "Resumed")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.Endowed>("Balances", "Endowed")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.DustLost>("Balances", "DustLost")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.Transfer>("Balances", "Transfer")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.BalanceSet>("Balances", "BalanceSet")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.Deposit>("Balances", "Deposit")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.Reserved>("Balances", "Reserved")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.Unreserved>("Balances", "Unreserved")
                .AddEvent<Polkadot.BinaryContracts.Events.Balances.ReserveRepatriated>("Balances", "ReserveRepatriated")
                .AddEvent<Polkadot.BinaryContracts.Events.Sudo.Sudid>("Sudo", "Sudid")
                .AddEvent<Polkadot.BinaryContracts.Events.Sudo.KeyChanged>("Sudo", "KeyChanged")
                .AddEvent<Polkadot.BinaryContracts.Events.Sudo.SudoAsDone>("Sudo", "SudoAsDone")
                .AddEvent<Polkadot.BinaryContracts.Events.Nft.Created>("Nft", "Created")
                .AddEvent<Polkadot.BinaryContracts.Events.Nft.ItemCreated>("Nft", "ItemCreated")
                .AddEvent<Polkadot.BinaryContracts.Events.Nft.ItemDestroyed>("Nft", "ItemDestroyed")
                .AddEvent<Polkadot.BinaryContracts.Events.Nft.Transfer>("Nft", "Transfer")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Proposed>("Treasury", "Proposed")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Spending>("Treasury", "Spending")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Awarded>("Treasury", "Awarded")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Rejected>("Treasury", "Rejected")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Burnt>("Treasury", "Burnt")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Rollover>("Treasury", "Rollover")
                .AddEvent<Polkadot.BinaryContracts.Events.Treasury.Deposit>("Treasury", "Deposit")
                .AddEvent<Polkadot.BinaryContracts.Events.Vesting.VestingUpdated>("Vesting", "VestingUpdated")
                .AddEvent<Polkadot.BinaryContracts.Events.Vesting.VestingCompleted>("Vesting", "VestingCompleted")
;
        }
    }
}