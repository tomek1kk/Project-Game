using CommunicationLibrary;
using System;
using System.Collections.Generic;

namespace CommunicationServer
{
    public class CommunicationServer
    {
        private Dictionary<int, Descriptor> correlation;
        private Descriptor GMDescriptor;
        private static IMessageSenderReceiver messageService = new CommunicationServerMessageService();

        
        static void Main(string[] args)
        {
            // TODO: Get config
            messageService.StartReceiving(null);
        }

        public void GetConfig()
        {

        }

    }
}
