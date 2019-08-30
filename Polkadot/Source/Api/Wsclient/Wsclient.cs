namespace Polkadot.Api
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using WebSocketSharp;
    using System.Security.Cryptography.X509Certificates;
    using System.Net.Security;
    using System.Linq;

    public class Wsclient : IWebSocketClient
    {
        private WebSocketSharp.WebSocket _wss;
        private ILogger _logger;
        private List<IMessageObserver> _observers = new List<IMessageObserver>();

        public Wsclient(ILogger logger)
        {
            _logger = logger;
        }

        public int Connect(string node_url = "")
        {
            var connectionString = node_url.Equals(string.Empty) ? Consts.WssConnectionString : node_url;
            var certList = GetCertificatesFromPem(Consts.CertFileName);

            _wss = new WebSocketSharp.WebSocket(connectionString);

            _wss.SslConfiguration.EnabledSslProtocols = System.Security.Authentication.SslProtocols.Tls12;
            _wss.SslConfiguration.ClientCertificates = new X509CertificateCollection(certList);
            _wss.SslConfiguration.ServerCertificateValidationCallback =
                (sender, certificate, chain, sslPolicyErrors) => {
                    // If the server certificate is valid.
                    return (sslPolicyErrors == SslPolicyErrors.None);
            };

            _wss.OnMessage += (sender, e) => {

                var payload = e.Data;
                _logger.Info($"WS Received Message: {payload}");
                foreach(var item in _observers)
                {
                    item.HandleMessage(payload);
                }
            };

            _wss.Connect();

            return 0;

        }

        public void Disconnect()
        {
            _wss.Close();
        }

        public void Dispose()
        {
            if (_wss != null)
                ((IDisposable)_wss).Dispose();
        }

        public bool IsConnected()
        {
            return _wss == null ? false : _wss.IsAlive;
        }

        public void RegisterMessageObserver(IMessageObserver handler)
        {
            _observers.Add(handler);
        }

        public void Send(string msg)
        {
            _wss.Send(msg);
        }

        private X509CertificateCollection GetCertificatesFromPem(string filePath)
        {
            var certCollection = new X509CertificateCollection();

            using (StreamReader sr = new StreamReader(filePath))
            {
                var stringData = sr.ReadToEnd();
                var certs = stringData.Split("-----BEGIN CERTIFICATE-----").Where(i => i.Length > 0).ToArray();

                foreach (var cert in certs)
                {
                    var currCertBytes = new List<byte>();
                    var charArr = ("-----BEGIN CERTIFICATE-----" + cert).AsMemory().ToArray();
                    foreach (var item in charArr)
                    {
                        currCertBytes.Add((byte)item);
                    }
                    certCollection.Add(new X509Certificate(currCertBytes.ToArray()));
                }
            }

            return certCollection;
        }
    }
}
