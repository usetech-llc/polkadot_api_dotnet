namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using System.IO;

    [Collection("Sequential")]
    public class BadCertificate
    {
        private readonly ITestOutputHelper output;

        public BadCertificate(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void ThrowsOnNoCertificateFile()
        {
            Assert.Throws<FileNotFoundException>(() =>
            {
                using (IApplication app = PolkaApi.GetApplication())
                {
                    app.Connect(new ConnectionParameters(Consts.WssConnectionString, "not-existing-file"));

                    app.Disconnect();
                }
            });
        }

        [Fact(Skip = "Figure out how to break this test.")]
        public void ThrowsOnWrongCertificate()
        {
            using (IApplication app = PolkaApi.GetApplication())
            {
                app.Connect(new ConnectionParameters(Consts.WssConnectionString, "invalid-certificate.pem"));

                app.GetMetadata(new GetMetadataParams());

                app.Disconnect();
            }
        }
    }
}
