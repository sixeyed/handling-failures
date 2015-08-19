using Newtonsoft.Json;
using Sixeyed.HandlingFailures.Core;
using Sixeyed.HandlingFailures.Core.Enums;
using Sixeyed.HandlingFailures.Core.Fulfilment;
using Sixeyed.HandlingFailures.Core.Messages;
using System;
using System.Collections.Generic;
using System.Net;
using System.Web.Mvc;
using ZeroMQ;

namespace Sixeyed.HandlingFailures.Web.Controllers
{
    public class FulfilmentController : Controller
    {
        [HttpPost]
        public ActionResult SendV1()
        {
            var fulfilmentType = FulfilmentType.Email;
            Enum.TryParse<FulfilmentType>(Request.Form["fulfilmentType"], out fulfilmentType);
            
            var client = FulfilmentClientFactory.GetApiClient(fulfilmentType);
            client.Send("customer-address");

            ViewData.Add("fulfilmentType", fulfilmentType);
            return View("Index");
        }

        [HttpPost]
        public ActionResult SendV2()
        {
            var fulfilmentType = FulfilmentType.Email;
            Enum.TryParse<FulfilmentType>(Request.Form["fulfilmentType"], out fulfilmentType);
                        
            var client = FulfilmentClientFactory.GetApiClient(fulfilmentType);
            try
            {
                client.Send("customer-address");
            }
            catch(WebException ex)
            {
                var response = ex.Response as HttpWebResponse;
                switch (response.StatusCode)
                {
                    case HttpStatusCode.ServiceUnavailable:
                        //transient, try again:
                        client.Send("customer-address");
                        break;

                    case HttpStatusCode.BadRequest:
                        //permanent:
                        break;
                }
            }            

            ViewData.Add("fulfilmentType", fulfilmentType);
            return View("Index");
        }

        [HttpPost]
        public ActionResult SendV3()
        {
            var fulfilmentType = FulfilmentType.Email;
            Enum.TryParse<FulfilmentType>(Request.Form["fulfilmentType"], out fulfilmentType);

            var queueAddress = Config.Get("Queues.Fulfilment.Address");

            var header = new Header
            {
                BodyType = typeof(SendFulfilmentCommand).Name
            };
            var body = new SendFulfilmentCommand
            {
                FulfilmentType = fulfilmentType,
                Address = "customer-address"
            };

            var messageFrames = new List<ZFrame>();
            messageFrames.Add(new ZFrame(JsonConvert.SerializeObject(header)));
            messageFrames.Add(new ZFrame(JsonConvert.SerializeObject(body)));

            using (var context = new ZContext())
            using (var sender = new ZSocket(context, ZSocketType.PUSH))
            {
                sender.Connect(queueAddress);
                sender.Send(new ZMessage(messageFrames));
            }

            ViewData.Add("fulfilmentType", fulfilmentType);
            return View("Index");
        }
    }
}