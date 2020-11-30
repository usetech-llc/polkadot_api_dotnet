using System;

namespace Polkadot.Api
{
    public class ConnectionParameters
    {
        public ConnectionParameters(string nodeUrl, string clientCertPath = "")
        {
            NodeUrl = nodeUrl ?? throw new ArgumentNullException(nameof(nodeUrl));
            ClientCertPath = clientCertPath;
        }

        public string NodeUrl { get; private set; }
        public string ClientCertPath { get; private set; }
    }
}
