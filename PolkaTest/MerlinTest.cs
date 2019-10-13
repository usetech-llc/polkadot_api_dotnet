namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using System.Text;
    using Schnorrkel.Merlin;

    public class MerlinTest
    {
        private readonly ITestOutputHelper output;

        public MerlinTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            var ts = new Transcript("test protocol");

            ts.AppendMessage("some label", "some data");

            var challenge = new byte[32] { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };

            ts.ChallengeBytes(Encoding.UTF8.GetBytes("challenge"), ref challenge);

            output.WriteLine(ts.ToString());
        }
    }
}
