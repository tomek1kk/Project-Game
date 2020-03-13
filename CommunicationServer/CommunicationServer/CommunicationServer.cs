using CommunicationLibrary;
using System;
using System.Collections.Generic;

namespace CommunicationServer
{
    public class CommunicationServer
    {
        private Dictionary<int, Descriptor> correlation;
        private Descriptor GMDescriptor;
        private static IMessageService messageService = new CommunicationServerMessageService();

        
        static void Main(string[] args)
        {
            // Get config
            messageService.ListenForMessages();
        }

        public void GetConfig()
        {

        }

    }
}
