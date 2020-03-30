using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServerNamespace
{
    public class AgentDescriptor : Descriptor
    {
        public AgentDescriptor(TcpClient tcpClient) : base(tcpClient) { }

        public override void StartReceiving(Action<Message> action)
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
    }
}
