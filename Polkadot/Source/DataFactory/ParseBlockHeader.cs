namespace Polkadot.DataFactory
{
    using Newtonsoft.Json;
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System;
    using System.Collections.Generic;

    public class ParseBlockHeader : IParseFactory<BlockHeader>
    {
        public BlockHeader Parse(JObject json)
        {
            dynamic djson = JsonConvert.DeserializeObject(json["result"].ToString());

            var result = new BlockHeader
            {
                ParentHash = djson["parentHash"].ToString(),
                Number = Convert.ToUInt64(djson["number"].ToString().Substring(2)),
                StateRoot = djson["stateRoot"].ToString(),
                ExtrinsicsRoot = djson["extrinsicsRoot"].ToString()
            };

            var digests = new List<DigestItem>();
            foreach (var item in djson["digest"]["logs"].ToObject<string[]>())
            {
                digests.Add(new DigestItem {Key = DigestItemKey.AuthoritiesChange, Value = item.ToString() });
            }
            result.Digest = digests.ToArray();

            return result;
        }
    }
}
