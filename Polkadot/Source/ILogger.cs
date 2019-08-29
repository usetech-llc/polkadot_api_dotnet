namespace Polkadot.Source
{
    public interface ILogger
    {
        void Info(string message);
        void Error(string message);
        void Warning(string message);
    }
}