using Newtonsoft.Json.Linq;

namespace Polkadot.Source
{
    public interface IWebSocketMessageObserver
    {
        /// <summary>
        /// Do not call API in message handler thread
        /// </summary>
        void handleWsMessage(int subscriptionId, JObject message);
    }
}