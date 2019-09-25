namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;

    public class GetChild
    {
        private readonly ITestOutputHelper output;

        public GetChild(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();

                //var childKeysResponse = app.GetChildKeys(string.Empty, string.Empty);
                //output.WriteLine($"Child Keys Response: {childKeysResponse}");

                //var childStorageResponse = app.GetChildStorage(string.Empty, string.Empty);
                //output.WriteLine($"Child Storage Response: {childStorageResponse}");

                //var childStorageHashResponse = app.GetChildStorageHash(string.Empty, string.Empty);
                //output.WriteLine($"Child Storage Hash Response: {childStorageHashResponse}");

                //var childStorageSizeResponse = app.GetChildStorageSize(string.Empty, string.Empty);
                //output.WriteLine($"Child Storage Size Response: {childKeysResponse}");

                app.Disconnect();
            }
        }
    }
}
