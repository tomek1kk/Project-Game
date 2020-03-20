using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServer
{
    public class GMDescriptor : Descriptor
    {
        public GMDescriptor(TcpClient tcpClient) : base(tcpClient) { }

        /// <summary>
        /// Receives messeges from agent
        /// </summary>
        /// <param name="action"></param>
        public void StartReceivingFromGM(Action<Message> m)
        {
            base.StartReceiving(m);
        }

        /// <summary>
        /// Sends message to Agent
        /// </summary>
        /// <param name="message"></param>
        public void SendToGM(Message m)
        {
            base.SendMessage(m);
        }
    }
}
