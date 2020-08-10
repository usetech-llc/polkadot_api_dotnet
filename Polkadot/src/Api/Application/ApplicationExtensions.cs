using Polkadot.Api;

namespace Polkadot.Api
{
    public static class ApplicationExtensions
    {
        // Note: this is added solely for compatibility purpose with previous versions of the library
        public static int Connect(this IApplication app, string nodeUrl = Consts.WssConnectionString)
        {
            return app.Connect(new ConnectionParameters(nodeUrl));
        }
    }
}
