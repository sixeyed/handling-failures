﻿using System;
using System.Net;

namespace Sixeyed.HandlingFailures.Core.Fulfilment
{
    public class EmailFulfilmentClient : IFulfilmentClient
    {
        public void Send(string customerAddress)
        {
            var baseAddress = new Uri(Config.Get("Api.Email.BaseUrl"));
            var requestUri = string.Format("{0}/fulfilment/email?address={1}", baseAddress, customerAddress);

            using (var client = new WebClient())
            {
                client.UploadString(requestUri, "Thanks");
            }
        }
    }
}
