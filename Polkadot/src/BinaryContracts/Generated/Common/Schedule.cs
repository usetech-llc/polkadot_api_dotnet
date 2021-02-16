using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public class Schedule
    {
        // Rust type "u32"
        [Serialize(0)]
        public uint Version { get; set; }


        // Rust type "Gas"
        [Serialize(1)]
        public Gas PutCodePerByteCost { get; set; }


        // Rust type "Gas"
        [Serialize(2)]
        public Gas GrowMemCost { get; set; }


        // Rust type "Gas"
        [Serialize(3)]
        public Gas RegularOpCost { get; set; }


        // Rust type "Gas"
        [Serialize(4)]
        public Gas ReturnDataPerByteCost { get; set; }


        // Rust type "Gas"
        [Serialize(5)]
        public Gas EventDataPerByteCost { get; set; }


        // Rust type "Gas"
        [Serialize(6)]
        public Gas EventPerTopicCost { get; set; }


        // Rust type "Gas"
        [Serialize(7)]
        public Gas EventBaseCost { get; set; }


        // Rust type "Gas"
        [Serialize(8)]
        public Gas CallBaseCost { get; set; }


        // Rust type "Gas"
        [Serialize(9)]
        public Gas InstantiateBaseCost { get; set; }


        // Rust type "Gas"
        [Serialize(10)]
        public Gas DispatchBaseCost { get; set; }


        // Rust type "Gas"
        [Serialize(11)]
        public Gas SandboxDataReadCost { get; set; }


        // Rust type "Gas"
        [Serialize(12)]
        public Gas SandboxDataWriteCost { get; set; }


        // Rust type "Gas"
        [Serialize(13)]
        public Gas TransferCost { get; set; }


        // Rust type "Gas"
        [Serialize(14)]
        public Gas InstantiateCost { get; set; }


        // Rust type "u32"
        [Serialize(15)]
        public uint MaxEventTopics { get; set; }


        // Rust type "u32"
        [Serialize(16)]
        public uint MaxStackHeight { get; set; }


        // Rust type "u32"
        [Serialize(17)]
        public uint MaxMemoryPages { get; set; }


        // Rust type "u32"
        [Serialize(18)]
        public uint MaxTableSize { get; set; }


        // Rust type "bool"
        [Serialize(19)]
        public bool EnablePrintln { get; set; }


        // Rust type "u32"
        [Serialize(20)]
        public uint MaxSubjectLen { get; set; }



        public Schedule() { }
        public Schedule(uint @version, Gas @putCodePerByteCost, Gas @growMemCost, Gas @regularOpCost, Gas @returnDataPerByteCost, Gas @eventDataPerByteCost, Gas @eventPerTopicCost, Gas @eventBaseCost, Gas @callBaseCost, Gas @instantiateBaseCost, Gas @dispatchBaseCost, Gas @sandboxDataReadCost, Gas @sandboxDataWriteCost, Gas @transferCost, Gas @instantiateCost, uint @maxEventTopics, uint @maxStackHeight, uint @maxMemoryPages, uint @maxTableSize, bool @enablePrintln, uint @maxSubjectLen)
        {
            this.Version = @version;
            this.PutCodePerByteCost = @putCodePerByteCost;
            this.GrowMemCost = @growMemCost;
            this.RegularOpCost = @regularOpCost;
            this.ReturnDataPerByteCost = @returnDataPerByteCost;
            this.EventDataPerByteCost = @eventDataPerByteCost;
            this.EventPerTopicCost = @eventPerTopicCost;
            this.EventBaseCost = @eventBaseCost;
            this.CallBaseCost = @callBaseCost;
            this.InstantiateBaseCost = @instantiateBaseCost;
            this.DispatchBaseCost = @dispatchBaseCost;
            this.SandboxDataReadCost = @sandboxDataReadCost;
            this.SandboxDataWriteCost = @sandboxDataWriteCost;
            this.TransferCost = @transferCost;
            this.InstantiateCost = @instantiateCost;
            this.MaxEventTopics = @maxEventTopics;
            this.MaxStackHeight = @maxStackHeight;
            this.MaxMemoryPages = @maxMemoryPages;
            this.MaxTableSize = @maxTableSize;
            this.EnablePrintln = @enablePrintln;
            this.MaxSubjectLen = @maxSubjectLen;
        }

    }
}