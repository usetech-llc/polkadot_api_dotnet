namespace Polkadot.Api
{
    public static class Consts
    {
        public const string WssConnectionString = "wss://alex.unfrastructure.io/public/ws";
        public const string WsConnectionString = "ws://192.168.100.135:9944";
        public const string CertFileName = "ca-chain.cert.pem";

        public const int RESPONSE_TIMEOUT_S = 10;
        public const int PAPI_OK = 0;
        public const int STORAGE_KEY_BYTELENGTH = 32;
        public const int PUBLIC_KEY_LENGTH = 32;

        public const int SR25519_PUBLIC_SIZE = 32;
        public const int SR25519_SIGNATURE_SIZE = 64;
        public const byte ADDRESS_SEPARATOR = 0xFF;
        public const int ADDRESS_LENGTH = 48;

        public const string STORAGE_TYPE_ADDRESS = "AccountId";
        public const string STORAGE_TYPE_BLOCK_NUMBER = "BlockNumber";
        public const string STORAGE_TYPE_U32 = "u32";
        public const string STORAGE_TYPE_ACCOUNT_INDEX = "AccountIndex";
        public const string STORAGE_TYPE_PROPOSAL_INDEX = "PropIndex";
        public const string STORAGE_TYPE_REFERENDUM_INDEX = "ReferendumIndex";
        public const string STORAGE_TYPE_HASH = "Hash";
        public const string STORAGE_TYPE_PARACHAIN_ID = "ParaId";

        public const byte SIGNATURE_VERSION = 0x81;
        public const long BLOCK_HASH_SIZE = 32;
        public const int MAX_METHOD_BYTES_SZ = 2048;

        public static int DEFAULT_FIXED_EXSTRINSIC_SIZE = 103;


        // lastLengthChange storage subscription hash
        public const string LAST_LENGTH_CHANGE_SUBSCRIPTION = "0xe781aa1e06ea53e01a4e129e0496764e";
        // sessionLenth storage subscription hash
        public const string SESSION_LENGTH_SUBSCRIPTION = "0xd9c94b41dc87728ebf0a966d2e9ad9c0";
    }
}
