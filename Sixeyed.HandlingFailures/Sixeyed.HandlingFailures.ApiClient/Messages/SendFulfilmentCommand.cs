using Sixeyed.HandlingFailures.Core.Enums;

namespace Sixeyed.HandlingFailures.Core.Messages
{
    public class SendFulfilmentCommand
    {
        public string Address { get; set; }

        public FulfilmentType FulfilmentType { get; set; }
    }
}
