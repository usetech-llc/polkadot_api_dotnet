namespace PolkaTest
{
    using Newtonsoft.Json.Linq;
    using Polkadot.Api;
    using Polkadot.DataFactory.Metadata;
    using Xunit;
    using Xunit.Abstractions;

    [Collection("Sequential")]
    public class GetMetadata
    {
        private readonly ITestOutputHelper output;

        public GetMetadata(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect();
                var result = app.GetMetadata(null);

                Assert.NotNull(result);
                output.WriteLine($"Result : {result}");
                app.Disconnect();
            }
        }
    }
}
