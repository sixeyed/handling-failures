using Sixeyed.HandlingFailures.Core.Enums;

namespace Sixeyed.HandlingFailures.Core.Fulfilment
{
    public static class FulfilmentClientFactory
    {
        public static IFulfilmentClient GetApiClient(FulfilmentType fulfilmentType)
        {
            switch(fulfilmentType)
            {
                case FulfilmentType.Sms:
                    return new SmsFulfilmentClient();

                case FulfilmentType.Twitter:
                    return new TwitterFulfilmentClient();
            }

            return new EmailFulfilmentClient();
        }
    }
}
