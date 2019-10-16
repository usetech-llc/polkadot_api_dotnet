namespace Polkadot.DataStructs
{
    public class Address
    {
        public string Symbols { get; set; }

        public Address() { }

        public Address(string symbols)
        {
            Symbols = symbols;
        }
    }
}