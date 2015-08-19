namespace Sixeyed.HandlingFailures.Core.Fulfilment
{
    public interface IFulfilmentClient
    {
        void Send(string address);
    }
}