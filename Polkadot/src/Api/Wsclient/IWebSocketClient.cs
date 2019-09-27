namespace Polkadot.Api
{
    using System;

    public interface IWebSocketClient : IDisposable
    {
        int Connect(string node_url);
        bool IsConnected();
        void Disconnect();
        void Send(string msg);
        void RegisterMessageObserver(IMessageObserver handler);
    }
}