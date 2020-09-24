﻿namespace Polkadot.Api
{
    using Newtonsoft.Json.Linq;

    public interface IWebSocketMessageObserver
    {
        /// <summary>
        /// Do not call API in message handler thread
        /// </summary>
        void HandleWsMessage(string subscriptionId, JObject message);
    }
}