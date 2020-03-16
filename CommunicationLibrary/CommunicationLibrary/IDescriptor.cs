using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary
{
    interface IDescriptor : IDisposable
    {
        void Send(Message m);
        void SetReceiveCallback(Action<Message> receiveCallback);
    }
}
