using System;

namespace Sixeyed.HandlingFailures.Core.Messages
{
    public class Header
    {
        public string MessageId { get; set; }

        public string BodyType { get; set; }

        public int HandledCount { get; set; }

        public string LastExceptionMessage { get; set; }

        public Header()
        {
            MessageId = Guid.NewGuid().ToString().Substring(0, 6);
        }
    }
}
