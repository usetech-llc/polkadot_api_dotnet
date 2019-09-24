namespace Polkadot.DataFactory
{
    using Newtonsoft.Json.Linq;
    using Polkadot.Data;
    using System;
    using System.Collections.Generic;

    public class ParseBlockHeader : IParseFactory<BlockHeader>
    {
        public BlockHeader Parse(JObject json)
        {
            //strcpy(bh->parentHash, jsonObject["parentHash"].string_value().c_str());
            //bh->number = (unsigned long long)atoi128(jsonObject["parentHash"].string_value().substr(2).c_str());
            //strcpy(bh->stateRoot, jsonObject["stateRoot"].string_value().c_str());
            //strcpy(bh->extrinsicsRoot, jsonObject["extrinsicsRoot"].string_value().c_str());

            //int i = 0;
            //for (Json item : jsonObject["digest"]["logs"].array_items())
            //{
            //    strcpy(bh->digest[i].value, item.string_value().c_str());
            //    i++;
            //}

            var result = new BlockHeader
            {
                ParentHash = json["parentHash"].ToString(),
                Number = Convert.ToUInt64(json["number"].ToString().Substring(2)),
                StateRoot = json["stateRoot"].ToString(),
                ExtrinsicsRoot = json["extrinsicsRoot"].ToString()
            };

            var digests = new List<DigestItem>();
            //  int i = 0;
            foreach (var item in json["digest"]["logs"].Values())
            {
                digests.Add(new DigestItem {Key = DigestItemKey.AuthoritiesChange, Value = item.ToString() });
            }
            result.Digest = digests.ToArray();

            return result;
        }
    }
}
