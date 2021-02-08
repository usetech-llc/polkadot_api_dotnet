using System.Linq;
using Polkadot.NodeTypesGenerator.TypesParser.Types;
using Sprache;

namespace Polkadot.NodeTypesGenerator.TypesParser
{
    public class RustTypeParser
    {
        public static Parser<RustType> CreateParser()
        {
            Parser<RustType> type = null;

            var delimiter = from leadingSpaces in Parse.WhiteSpace.Many().Optional()
                from comma in Parse.Char(',')
                from trailingSpaces in Parse.WhiteSpace.Many().Optional()
                select comma;

            var spaces = from f in Parse.Char(' ')
                from s in Parse.Char(' ').Many().Optional()
                select f;
            
            Parser<RustType> tuple = 
                from t in Parse
                    .Ref(() => type)
                    .DelimitedBy(delimiter)
                    .Contained(Parse.Char('('), Parse.Char(')'))
                select new RustType() {Type = new RustTuple() { RustTypes = t.ToList() }};

            var simpleType = (from prefix in Parse.Letter.Or(Parse.Char('_')).Once()
                from other in Parse
                    .LetterOrDigit
                    .Or(Parse.Char('_'))
                    .Many()
                    .Text()
                select new RustSimpleType() {Name = new string(prefix.Concat(other).ToArray())});

            var alias = from open in Parse.Char('<')
                from t in Parse.Ref(() => type)
                from s in spaces
                from @as in Parse.String("as")
                from s2 in spaces
                from i in simpleType
                from close in Parse.Char('>')
                select t;

            var generic = from genericName in simpleType
                from t in Parse
                    .Ref(() => type)
                    .DelimitedBy(delimiter)
                    .Contained(Parse.Char('<'), Parse.Char('>'))
                select new RustType() {Type = new RustGeneric() {GenericName = genericName.Name, GenericParams = t.ToList()}};


            var nestedType =
                from pref in Parse.String("::")
                from s in ParseHardcodedTypes().Or(simpleType.Select(r => new RustType(){Type = r}))
                select s;
            
            type = ParseHardcodedTypes()
                .Or(tuple)
                .Or(alias)
                .Or(generic)
                .Or(simpleType
                    .Select(r => new RustType(){Type = r}))
                .Then(t => nestedType.Optional().Select(o => o.GetOrElse(t)));

            return type.End();
        }

        private static Parser<RustType> ParseHardcodedTypes()
        {
            var @byte = Parse.String("u8").Text().Select(s => new RustSimpleType(){ Name = "byte"});
            var @ushort = Parse.String("u16").Text().Select(s => new RustSimpleType(){ Name = "ushort"});
            var @ulong = Parse.String("u64").Text().Select(s => new RustSimpleType(){ Name = "ulong"});
            var @uint = Parse.String("u32").Text().Select(s => new RustSimpleType(){ Name = "uint"});
            var collectionId = Parse.String("CollectionId").Text().Select(s => new RustSimpleType(){ Name = "uint"});
            var tokenId = Parse.String("TokenId").Text().Select(s => new RustSimpleType(){ Name = "uint"});
            var u128 = Parse.String("u128").Text().Select(s => new RustSimpleType(){ Name = "BigInteger", ConvertAttributeName = "U128Converter"});
            var address = Parse.String("<T::Lookup as StaticLookup>::Source").Text().Select(s => new RustSimpleType(){ Name = "PublicKey"});
            var balanceOf = Parse.String("BalanceOf<T>").Text().Select(s => new RustSimpleType(){ Name = "Balance"});
            var blockNumber = Parse.String("T::BlockNumber").Text().Select(s => new RustSimpleType(){ Name = "BlockNumber"});
            var vestingInfo = Parse.String("VestingInfo<BalanceOf<T>, T::BlockNumber>").Text().Select(s => new RustSimpleType(){ Name = "VestingInfo"});
            var accountId = Parse.String("T::AccountId").Text().Select(s => new RustSimpleType(){ Name = "PublicKey"});
            var call = Parse.String("Call").Text().Select(s => new RustSimpleType(){ Name = "InheritanceCall<IExtrinsicCall>"});
            var createItemData = Parse.String("CreateItemData").Text().Select(s => new RustSimpleType(){ Name = "Polkadot.BinaryContracts.Nft.CreateItem.CreateItemData"});

            return @byte
                .Or(@ushort)
                .Or(@uint)
                .Or(@ulong)
                .Or(@u128)
                .Or(collectionId)
                .Or(tokenId)
                .Or(balanceOf)
                .Or(blockNumber)
                .Or(vestingInfo)
                .Or(accountId)
                .Or(address)
                .Or(call)
                .Or(createItemData)
                .Select(r => new RustType(){Type = r});
        }
    }
}