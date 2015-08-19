using System;
using System.Net;

namespace Sixeyed.HandlingFailures.Core.Fulfilment
{
    public class SmsFulfilmentClient : IFulfilmentClient
    {
        public void Send(string customerAddress)
        {
            var baseAddress = new Uri(Config.Get("Api.Sms.BaseUrl"));
            var requestUri = string.Format("{0}/fulfilment/sms?number={1}", baseAddress, customerAddress);

            using (var client = new WebClient())
            {
                client.UploadString(requestUri, "Thanks");
            }
        }
    }
}
