using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    interface IMessageSenderReceiver : IDisposable
    {
        void Send(Message m);
        void StartReceiving(Action<Message> receiveCallback);
    }
}
