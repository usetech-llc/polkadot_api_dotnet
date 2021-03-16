using Polkadot.BinarySerializer;

namespace Polkadot.BinaryContracts
{
    public class Schedule
    {
        /// Version of the schedule.
        [Serialize(0)]
		public uint Version { get; set; }

		/// Cost of putting a byte of code into storage.
		[Serialize(1)]
		public Gas PutCodePerByteCost { get; set; }

		/// Gas cost of a growing memory by single page.
		[Serialize(2)]
		public Gas GrowMemCost { get; set; }

		/// Gas cost of a regular operation.
		[Serialize(3)]
		public Gas RegularOpCost { get; set; }

		/// Gas cost per one byte returned.
		[Serialize(4)]
		public Gas ReturnDataPerByteCost { get; set; }

		/// Gas cost to deposit an event; the per-byte portion.
		[Serialize(5)]
		public Gas EventDataPerByteCost { get; set; }

		/// Gas cost to deposit an event; the cost per topic.
		[Serialize(6)]
		public Gas EventPerTopicCost { get; set; }

		/// Gas cost to deposit an event; the base.
		[Serialize(7)]
		public Gas EventBaseCost { get; set; }

		/// Base gas cost to call into a contract.
		[Serialize(8)]
		public Gas CallBaseCost { get; set; }

		/// Base gas cost to instantiate a contract.
		[Serialize(9)]
		public Gas InstantiateBaseCost { get; set; }

		/// Base gas cost to dispatch a runtime call.
		[Serialize(10)]
		public Gas DispatchBaseCost { get; set; }

		/// Gas cost per one byte read from the sandbox memory.
		[Serialize(11)]
		public Gas SandboxDataReadCost { get; set; }

		/// Gas cost per one byte written to the sandbox memory.
		[Serialize(12)]
		public Gas SandboxDataWriteCost { get; set; }

		/// Cost for a simple balance transfer.
		[Serialize(13)]
		public Gas TransferCost { get; set; }

		/// Cost for instantiating a new contract.
		[Serialize(14)]
		public Gas InstantiateCost { get; set; }

		/// The maximum number of topics supported by an event.
		[Serialize(15)]
		public uint MaxEventTopics { get; set; }

		/// Maximum allowed stack height.
		///
		/// See https://wiki.parity.io/WebAssembly-StackHeight to find out
		/// how the stack frame cost is calculated.
		[Serialize(16)]
		public uint MaxStackHeight { get; set; }

		/// Maximum number of memory pages allowed for a contract.
		[Serialize(17)]
		public uint MaxMemoryPages { get; set; }

		/// Maximum allowed size of a declared table.
		[Serialize(18)]
		public uint MaxTableSize { get; set; }

		/// Whether the `seal_println` function is allowed to be used contracts.
		/// MUST only be enabled for `dev` chains, NOT for production chains
		[Serialize(19)]
		public bool EnablePrintln { get; set; }

		/// The maximum length of a subject used for PRNG generation.
		[Serialize(20)]
		public uint MaxSubjectLen { get; set; }

		/// The maximum length of a contract code in bytes. This limit applies to the uninstrumented
		// and pristine form of the code as supplied to `put_code`.
		[Serialize(21)]
		public uint MaxCodeSize { get; set; }
    }
}