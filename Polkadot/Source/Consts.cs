using System;
using System.Collections.Generic;
using System.Text;

namespace Polkadot.Source
{
    public static class Consts
    {
        public const string WssConnectionString = "wss://alex.unfrastructure.io/public/ws";
        public const string WsConnectionString = "ws://192.168.100.135:9944";
        public const string CertFileName = "ca-chain.cert.pem";

        public const long RESPONSE_TIMEOUT_S = 10;
        public const int PAPI_OK = 0;
    }
}
