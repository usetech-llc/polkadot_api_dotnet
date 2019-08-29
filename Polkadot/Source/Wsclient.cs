using System;
using System.Collections.Generic;
using System.IO;
using WebSocketSharp;
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;
using System.Linq;
using System.Configuration;

namespace Polkadot.Source
{
    public class Wsclient : IWebSocketClient
    {
        //public:
        //    static IWebSocketClient* getInstance(ILogger* logger);
        //    ~CWebSocketClient() override;
        //    virtual int connect(string node_url = "");
        //    virtual bool isConnected();
        //    virtual void disconnect();
        //    virtual int send(const string &msg);
        //    virtual void registerMessageObserver(IMessageObserver* handler);

        //private:
        //    ILogger* _logger;
        //    static CWebSocketClient* _instance;
        //    vector<IMessageObserver*> _observers;
        //    thread* _connectedThread;
        //    thread* _healthThread;
        //    client _c;
        //    client_no_tls _c_no_tls;
        //    bool _tls;
        //    client::connection_ptr _connection;
        //    client_no_tls::connection_ptr _connection_no_tls;
        //    bool _connected;
        //    condition_variable _connectionCV; // Condition variable used to notify about connection
        //    mutex _connectionMtx;             // Mutex for condition varaiable
        //    static chrono::seconds ConnectionTimeout;
        //    mutex _sendMtx; // Mutex for sending data to be block send method

        //    friend context_ptr on_tls_init(const char* hostname, websocketpp::connection_hdl);
        //    friend void on_message(websocketpp::connection_hdl, client::message_ptr msg);
        //    friend void on_open(client* c, websocketpp::connection_hdl hdl);
        //    void health();
        //    void runWsMessages();
        //    int connect_tls(string node_url);
        //    int connect_no_tls(string node_url);

        //    CWebSocketClient(ILogger* logger);

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
