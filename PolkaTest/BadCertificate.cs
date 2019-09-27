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
        public void Ok()
        {
            var cert = File.ReadAllBytes(Consts.CertFileName);
            File.Delete(Consts.CertFileName);

            Assert.Throws<FileNotFoundException>(() =>
            {
                using (IApplication app = PolkaApi.GetAppication())
                {
                    app.Connect();

                    app.Disconnect();
                }
            });

            File.WriteAllBytes(Consts.CertFileName, cert);
        }
    }
}
