using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    public interface IMessageSenderReceiver : IDisposable
    {
        void Send<T>(Message<T> m) where T : MessagePayload;
        void StartReceiving(Action<Message> receiveCallback);
    }
}
