using System;
using System.Net;

namespace Sixeyed.HandlingFailures.Core.Fulfilment
{
    public class TwitterFulfilmentClient : IFulfilmentClient
    {
        public void Send(string customerAddress)
        {
            var baseAddress = new Uri(Config.Get("Api.Twitter.BaseUrl"));
            var requestUri = string.Format("{0}/fulfilment/twitter?id={1}", baseAddress, customerAddress);

            using (var client = new WebClient())
            {
                client.UploadString(requestUri, "Thanks");
            }
        }
    }
}
