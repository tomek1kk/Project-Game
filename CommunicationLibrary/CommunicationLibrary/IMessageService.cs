using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public interface IMessageService
    {
        void InitService();
        void ListenForMessages();
        bool Send<T>(T message, int port) where T : Message;
    }
}
