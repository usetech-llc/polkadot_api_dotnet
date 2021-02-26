using Polkadot.BinarySerializer;
using Polkadot.DataStructs;
using Polkadot.BinarySerializer.Converters;
using Polkadot.BinaryContracts.Nft;
using Polkadot.BinaryContracts.Common;
using System.Numerics;

namespace Polkadot.BinaryContracts.Common
{
    public partial class CollectionType
    {
        // Rust type "AccountId"
        [Serialize(0)]
        public PublicKey Owner { get; set; }


        // Rust type "CollectionMode"
        [Serialize(1)]
        public CollectionMode Mode { get; set; }


        // Rust type "AccessMode"
        [Serialize(2)]
        public AccessMode Access { get; set; }


        // Rust type "DecimalPoints"
        [Serialize(3)]
        public DecimalPoints DecimalPoints { get; set; }


        // Rust type "Vec<u16>"
        [Serialize(4)]
        [PrefixedArrayConverter]
        public ushort[] Name { get; set; }


        // Rust type "Vec<u16>"
        [Serialize(5)]
        [PrefixedArrayConverter]
        public ushort[] Description { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(6)]
        [PrefixedArrayConverter]
        public byte[] TokenPrefix { get; set; }


        // Rust type "bool"
        [Serialize(7)]
        public bool MintMode { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(8)]
        [PrefixedArrayConverter]
        public byte[] OffchainSchema { get; set; }


        // Rust type "SchemaVersion"
        [Serialize(9)]
        public SchemaVersion SchemaVersion { get; set; }


        // Rust type "AccountId"
        [Serialize(10)]
        public PublicKey Sponsor { get; set; }


        // Rust type "bool"
        [Serialize(11)]
        public bool SponsorConfirmed { get; set; }


        // Rust type "CollectionLimits"
        [Serialize(12)]
        public CollectionLimits Limits { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(13)]
        [PrefixedArrayConverter]
        public byte[] VariableOnChainSchema { get; set; }


        // Rust type "Vec<u8>"
        [Serialize(14)]
        [PrefixedArrayConverter]
        public byte[] ConstOnChainSchema { get; set; }



        public CollectionType() { }
        public CollectionType(PublicKey @owner, CollectionMode @mode, AccessMode @access, DecimalPoints @decimalPoints, ushort[] @name, ushort[] @description, byte[] @tokenPrefix, bool @mintMode, byte[] @offchainSchema, SchemaVersion @schemaVersion, PublicKey @sponsor, bool @sponsorConfirmed, CollectionLimits @limits, byte[] @variableOnChainSchema, byte[] @constOnChainSchema)
        {
            this.Owner = @owner;
            this.Mode = @mode;
            this.Access = @access;
            this.DecimalPoints = @decimalPoints;
            this.Name = @name;
            this.Description = @description;
            this.TokenPrefix = @tokenPrefix;
            this.MintMode = @mintMode;
            this.OffchainSchema = @offchainSchema;
            this.SchemaVersion = @schemaVersion;
            this.Sponsor = @sponsor;
            this.SponsorConfirmed = @sponsorConfirmed;
            this.Limits = @limits;
            this.VariableOnChainSchema = @variableOnChainSchema;
            this.ConstOnChainSchema = @constOnChainSchema;
        }

    }
}