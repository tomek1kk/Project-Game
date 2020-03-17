using CommunicationLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace CommunicationServer
{
    public class CommunicationServer
    {
        private Dictionary<int, Descriptor> correlation;
        private Descriptor GMDescriptor;

        
        static void Main(string[] args)
        {
            // TODO: Get config
            Console.WriteLine("cos");

            Stream stream = new MemoryStream();
            Parser parser = new Parser();
            StreamMessageSenderReceiver streamMessageSenderReceiver = new StreamMessageSenderReceiver(stream, new Parser());
            BlockingCollection<Message> messages = new BlockingCollection<Message>();
            streamMessageSenderReceiver.StartReceiving(message => messages.Add(message));
            while (true)
            {
                if (messages.Count > 0)
                {
                    Message m = messages.Take();
                    Console.WriteLine("Received message: ");
                    Console.WriteLine(m.MessageId);
                }
            }
        }

        public void GetConfig()
        {

        }

    }
}
