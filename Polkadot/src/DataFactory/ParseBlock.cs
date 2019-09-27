namespace Polkadot.DataFactory
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System;
    using System.Collections.Generic;

    public class ParseBlock : IParseFactory<SignedBlock>
    {
        public SignedBlock Parse(JObject json)
        {
            dynamic djson = JsonConvert.DeserializeObject(json["result"].ToString());

            var block = new Block
            {
                Extrinsic = null,
                Header = new BlockHeader
                {
                    ParentHash = djson["block"]["header"]["parentHash"].ToString(),
                    Number = Convert.ToUInt64(djson["block"]["header"]["number"].ToString().Substring(2), 16),
                    StateRoot = djson["block"]["header"]["stateRoot"].ToString(),
                    ExtrinsicsRoot = djson["block"]["header"]["extrinsicsRoot"].ToString()
                }
            };

            var digests = new List<DigestItem>();
            foreach (string item in djson["block"]["header"]["digest"]["logs"].ToObject<string[]>())
            {
                digests.Add(new DigestItem { Key = 0, Value = item.ToString() });
            }
            block.Header.Digest = digests.ToArray();


            var extrinsics = new List<string>();
            foreach (string item in djson["block"]["extrinsics"].ToObject<string[]>())
            {
                extrinsics.Add(item.ToString());
            }
            block.Extrinsic = extrinsics.ToArray();

            return new SignedBlock
            {
                Block = block,
                Justification = djson["justification"].ToString()
            };
        }
    }
}
