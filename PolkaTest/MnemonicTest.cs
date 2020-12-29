namespace PolkaTest
{
    using Xunit;
    using Xunit.Abstractions;
    using System;
    using Polkadot.Utils;
    using Schnorrkel.Keys;
    using StrobeNet.Extensions;
    using Mnemonic;

    public class MnemonicTest
    {
        private readonly ITestOutputHelper output;

        public MnemonicTest(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void BasicMnemonicTest()
        {
            // subkey inspect "girl acid actress catalog behave cactus broom social earth connect strike dynamic"
            // Secret phrase `girl acid actress catalog behave cactus broom social earth connect strike dynamic` is account:
            // Network ID/ version: substrate
            // Secret seed:         0x28f317caf58a0980c68bad3bb99db769c41f16ff0a70bf807c1276a4a17ebcc5
            // Public key(hex):     0xa8895a6c2ac0e7c4ad27237fbb056d227b125bb8a497f2d595e5c47007e54904
            // Account ID:          0xa8895a6c2ac0e7c4ad27237fbb056d227b125bb8a497f2d595e5c47007e54904
            // SS58 Address:        5FsghoEzDPrJUPefHvjJsQ7WSuYJE9MkjYSuBt6XWne75gxm

            var seed = "girl acid actress catalog behave cactus broom social earth connect strike dynamic";

            var secretBytes = MnemonicSubstrate.GenerateSecretKeyFromMnemonic(seed);
            var msk = new MiniSecret(secretBytes, ExpandMode.Ed25519);
            var mini = secretBytes.ToHexString();
            var pk = msk.ExpandToPublic();

            // Check public key
            Assert.Equal("a8895a6c2ac0e7c4ad27237fbb056d227b125bb8a497f2d595e5c47007e54904".ToUpper(),
                pk.Key.ToHexString());

            // Check secret seed
            Assert.Equal("28f317caf58a0980c68bad3bb99db769c41f16ff0a70bf807c1276a4a17ebcc5".ToUpper(),
                mini);
        }

        [Fact]
        public void PairMnemonicTest()
        {
            // subkey inspect "girl acid actress catalog behave cactus broom social earth connect strike dynamic"
            // Secret phrase `girl acid actress catalog behave cactus broom social earth connect strike dynamic` is account:
            // Network ID/ version: substrate
            // Secret seed:         0x28f317caf58a0980c68bad3bb99db769c41f16ff0a70bf807c1276a4a17ebcc5
            // Public key(hex):     0xa8895a6c2ac0e7c4ad27237fbb056d227b125bb8a497f2d595e5c47007e54904
            // Account ID:          0xa8895a6c2ac0e7c4ad27237fbb056d227b125bb8a497f2d595e5c47007e54904
            // SS58 Address:        5FsghoEzDPrJUPefHvjJsQ7WSuYJE9MkjYSuBt6XWne75gxm

            var seed = "girl acid actress catalog behave cactus broom social earth connect strike dynamic";

            var pair = MnemonicSubstrate.GeneratePairFromMnemonic(seed);
  
            // Check public key
            Assert.Equal("a8895a6c2ac0e7c4ad27237fbb056d227b125bb8a497f2d595e5c47007e54904".ToUpper(),
                pair.Public.Key.ToHexString());

            // Check secret seed
            var k = new MiniSecret("28f317caf58a0980c68bad3bb99db769c41f16ff0a70bf807c1276a4a17ebcc5".ToByteArray(),
                ExpandMode.Ed25519);
            
            Assert.Equal(k.ExpandToSecret().ToBytes().ToHexString(),
                pair.Secret.ToBytes().ToHexString());
        }
    }
}
