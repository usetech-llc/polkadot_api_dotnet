namespace Polkadot.Api
{
    public class PolkaApi
    {
        private static Application CreateInstance()
        {
            var param = new JsonRpcParams {JsonrpcVersion = "2.0"};

            var logger = new Logger();
            var jsonrpc = new JsonRpc(new Wsclient(logger), logger, param, _ =>
            {
                
            });
            return new Application(logger, jsonrpc, Application.DefaultSubstrateSettings());
        }

        public static IApplication GetApplication()
        {
            return CreateInstance();
        }
    }
}
