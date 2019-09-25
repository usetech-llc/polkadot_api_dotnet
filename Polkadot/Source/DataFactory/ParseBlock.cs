namespace Polkadot.DataFactory
{
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System;
    using System.Collections.Generic;

    public class ParseBlock : IParseFactory<SignedBlock>
    {
        public SignedBlock Parse(JObject json)
        {
            var block = new Block
            {
                Extrinsic = null,
                Header = new BlockHeader
                {
                    ParentHash = json["block"]["header"]["parentHash"].ToString(),
                    Number = Convert.ToUInt64(json["block"]["header"]["number"].ToString().Substring(2), 16),
                    StateRoot = json["block"]["header"]["stateRoot"].ToString(),
                    ExtrinsicsRoot = json["block"]["header"]["extrinsicsRoot"].ToString()
                }
            };

            var digests = new List<DigestItem>();
            foreach (var item in json["block"]["header"]["digest"]["logs"].Values())
            {
                digests.Add(new DigestItem { Key = 0, Value = item.ToString() });
            }
            block.Header.Digest = digests.ToArray();


            var extrinsics = new List<string>();
            foreach (var item in json["block"]["extrinsics"].Values())
            {
                extrinsics.Add(item.ToString());
            }
            block.Extrinsic = extrinsics.ToArray();

            return new SignedBlock
            {
                Block = block,
                Justification = json["justification"].ToString()
            };
        }
    }
}
