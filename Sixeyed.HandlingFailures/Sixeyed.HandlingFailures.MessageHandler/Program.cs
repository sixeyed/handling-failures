using Newtonsoft.Json;
using Sixeyed.HandlingFailures.Core;
using Sixeyed.HandlingFailures.Core.Fulfilment;
using Sixeyed.HandlingFailures.Core.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using ZeroMQ;

namespace Sixeyed.HandlingFailures.MessageHandler
{
    class Program
    {
        static void Main(string[] args)
        {
            var queueAddress = Config.Get("Queues.Fulfilment.Address");

            using (var context = new ZContext())
            using (var receiver = new ZSocket(context, ZSocketType.PULL))
            {
                receiver.Bind(queueAddress);
                Console.WriteLine("Listening for messages on: " + queueAddress);

                while (true)
                {
                    using (var message = receiver.ReceiveMessage())
                    {
                        var headerFrame = message.First();
                        var header = JsonConvert.DeserializeObject<Header>(headerFrame.ReadString());
                        Console.WriteLine("* Received message, ID: {0}, body type: {1}, handled count: {2}", header.MessageId, header.BodyType, header.HandledCount);

                        //assume this is a permanent failure
                        if (header.HandledCount < 3)
                        {
                            Console.WriteLine("** Handling message. Previous attempts: {0}", header.HandledCount);
                            Handle(header, message.ElementAt(1));                            
                        }
                        else
                        {
                            Console.WriteLine("!! Message has failed {0} times. Not processing. Last exception: {1}", header.HandledCount, header.LastExceptionMessage);
                            //TODO - forward to error queue
                        }
                    }
                    Thread.Sleep(100);
                }
            }
        }

        private static void Handle(Header header, ZFrame bodyFrame)
        {
            //TODO - really we'd have a message handler factory:
            if (header.BodyType == typeof(SendFulfilmentCommand).Name)
            {                
                var command = JsonConvert.DeserializeObject<SendFulfilmentCommand>(bodyFrame.ReadString());                
                var client = FulfilmentClientFactory.GetApiClient(command.FulfilmentType);
                try
                {
                    client.Send(command.Address);
                    Console.WriteLine("*** Sent fulfilment, type: {0}, to address: {1}", command.FulfilmentType, command.Address);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("*** Fulfilment failed, resending message");

                    var queueAddress = Config.Get("Queues.Fulfilment.Address");
                    header.HandledCount++;
                    header.LastExceptionMessage = ex.Message;

                    var messageFrames = new List<ZFrame>();
                    messageFrames.Add(new ZFrame(JsonConvert.SerializeObject(header)));
                    messageFrames.Add(bodyFrame);

                    using (var context = new ZContext())
                    using (var sender = new ZSocket(context, ZSocketType.PUSH))
                    {
                        sender.Connect(queueAddress);
                        sender.Send(new ZMessage(messageFrames));
                    }
                }
            }
        }
    }
}