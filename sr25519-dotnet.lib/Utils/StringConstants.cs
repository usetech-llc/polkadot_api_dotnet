using System;
using System.Collections.Generic;
using System.Text;

namespace sr25519_dotnet.lib
{
    public static class StringConstants
    {
        public const string BadKeySizeMessage = "Cannot instantiate keypair. Public or secret key size is invalid.";

        public const string BadChaincodeSizeMessage = "Chaincode size is invalid.";
    }
}
