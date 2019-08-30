namespace Polkadot.Api
{
    using Newtonsoft.Json.Linq;

    public interface IWebSocketMessageObserver
    {
        /// <summary>
        /// Do not call API in message handler thread
        /// </summary>
        void handleWsMessage(int subscriptionId, JObject message);
    }
}