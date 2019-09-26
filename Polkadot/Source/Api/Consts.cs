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

        public const string STORAGE_TYPE_ADDRESS = "AccountId";
        public const string STORAGE_TYPE_BLOCK_NUMBER = "BlockNumber";
        public const string STORAGE_TYPE_U32 = "u32";
        public const string STORAGE_TYPE_ACCOUNT_INDEX = "AccountIndex";
        public const string STORAGE_TYPE_PROPOSAL_INDEX = "PropIndex";
        public const string STORAGE_TYPE_REFERENDUM_INDEX = "ReferendumIndex";
        public const string STORAGE_TYPE_HASH = "Hash";
        public const string STORAGE_TYPE_PARACHAIN_ID = "ParaId";
    }
}
