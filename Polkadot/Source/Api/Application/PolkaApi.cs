namespace Polkadot.Api
{
    public class PolkaApi
    {
        private static IApplication _instance;

        private static void CreateInstance()
        {
            JsonRpcParams param = new JsonRpcParams();
            param.jsonrpcVersion = "2.0";

            var logger = new Logger();
            var jsonrpc = new JsonRpc(new Wsclient(logger), logger, param);
            _instance = new Application(logger, jsonrpc);
        }

        public static IApplication GetAppication()
        {
            if (_instance == null)
                CreateInstance();
               
            return _instance;
        }
    }
}
