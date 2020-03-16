using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    public interface IMessageSenderReceiver : IDisposable
    {
        void Send(Message m);
        void StartReceiving(Action<Message> receiveCallback);
    }
}
