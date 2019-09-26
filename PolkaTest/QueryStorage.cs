namespace PolkaTest
{
    using Polkadot.Api;
    using Xunit;
    using Xunit.Abstractions;
    using Polkadot.Data;
    using System.Linq;
    using System.Collections.Generic;
    using System;

    class StorageItemTimestampComparer : IEqualityComparer<StorageItem>
    {
        public bool Equals(StorageItem x, StorageItem y)
        {
            return x.Value.Equals(y.Value, StringComparison.OrdinalIgnoreCase);
        }

        public int GetHashCode(StorageItem product)
        {
            //Check whether the object is null
            if (product is null) return 0;
            return product.Value.GetHashCode();
        }
    }

    [Collection("Sequential")]
    public class QueryStorage
    {
        private readonly ITestOutputHelper output;

        public QueryStorage(ITestOutputHelper output)
        {
            this.output = output;
        }

        [Fact]
        public void Ok()
        {
            using (IApplication app = PolkaApi.GetAppication())
            {
                app.Connect();
                // Get the most recent block number and hash
                var lastblock = app.GetBlock(null);
                var lastNumber = (int)lastblock.Block.Header.Number;
                var lastHash = app.GetBlockHash(new GetBlockHashParams { BlockNumber = lastNumber });

                // Get the 10 blocks back number and hash
                var prm2 = new GetBlockHashParams
                {
                    BlockNumber = lastNumber - 10
                };
                var tenBackHash = app.GetBlockHash(prm2);
                output.WriteLine($"Most recent block  : {lastNumber}, hash: {lastHash.Hash} ");

                // Get timestamp history for last 50 blocks
                string module = "Timestamp";
                string variable = "Now";
                string key = app.GetKeys("", module, variable);
                StorageItem[] items = app.QueryStorage(key, tenBackHash.Hash, lastHash.Hash, 20);

                // Assert that item count is at least 10
                output.WriteLine($"Item count returned : {items.Length} ");
                Assert.True(items.Length >= 10);

                // Assert that all items are different (time changes every block)
               // set<string> values;
                foreach (var item in items)
                {
                    output.WriteLine($"Block: {item.BlockHash}, value: {item.Value} ");
                }

                Assert.True(items.ToList().Distinct(new StorageItemTimestampComparer()).Count() == items.Length);
                output.WriteLine("All timestamps are different");

                app.Disconnect();
            }
        }
    }
}
