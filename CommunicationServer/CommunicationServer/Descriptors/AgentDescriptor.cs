using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServer
{
    public class AgentDescriptor : Descriptor
    {
        public AgentDescriptor(TcpClient tcpClient) : base(tcpClient) { }

        /// <summary>
        /// Receives messeges from agent
        /// </summary>
        /// <param name="action"></param>
        public void StartReceivingFromAgent(Action<Message> action)
        {
            //when we receive messege from agent we need to append AgentID before sending to GM.
            base.StartReceiving(
                 x =>
                 {
                     x.AgentId = this.Id;
                     action(x);
                 }
            );
        }

        /// <summary>
        /// Sends message to Agent
        /// </summary>
        /// <param name="message"></param>
        public void SendToAgent(Message m)
        {
            base.SendMessage(m);
        }
    }
}
