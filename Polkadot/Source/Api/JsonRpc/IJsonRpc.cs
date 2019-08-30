namespace Polkadot.Api
{
    using System;
    using Newtonsoft.Json.Linq;

    public interface IJsonRpc : IDisposable
    {
        /// <summary>
        /// Connects to WebSocket
        /// </summary>
        /// <param name="node_url"> Node URL to connect to </param>
        /// <returns> operation result </returns>
        int Connect(string node_url);

        /// <summary>
        /// Disconnects from WebSocket
        /// </summary>
        void Disconnect();

        /// <summary>
        /// Synchronously send request and wait for result
        /// </summary>
        /// <param name="jsonMap"> jsonMap JSON object with command parameters </param>
        /// <param name="timeout"> timeout_s - timeout of response in seconds </param>
        /// <returns> JSON object that contains parsed node response</returns>
        JObject Request(JObject jsonMap, long timeout = Consts.RESPONSE_TIMEOUT_S);

        /// <summary>
        /// Send a command to subscribe to websocket updates, e.g.state_subscribeStorage
        /// </summary>
        /// <param name="jsonMap"> jsonMap JSON object with command parameters </param>
        /// <param name="observer"> observer The observer that will be notified of updates </param>
        /// <returns> subscription ID </returns>
        int SubscribeWs(JObject jsonMap, IWebSocketMessageObserver observer);

        /// <summary>
        /// Send a command to unsubscribe from websocket updates, e.g.state_unsubscribeStorage
        /// </summary>
        /// <param name="subscriptionId"> subscriptionId Id of subscription to unsubscribe from </param>
        /// <param name="method"></param>
        /// <returns> command execution result </returns>
        int UnsubscribeWs(int subscriptionId, string method);
    }
}