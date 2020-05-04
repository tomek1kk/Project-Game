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

        public override void SendMessage(Message message)
        {
            try
            {
                base.SendMessage(message);
            }
            catch(Exception e)
            {
                e.Data.Add("agentId", this.Id);
                throw;
            }
        }

        public override void StartReceiving(Action<Message> action, Action<Exception> errorCallback)
        {
            //when we receive messege from agent we need to append AgentID before sending to GM.
            base.StartReceiving(
                 x =>
                 {
                     x.AgentId = this.Id;
                     action(x);
                 },
                 x =>
                 {
                     x.Data.Add("agentId", this.Id);
                     errorCallback(x);
                 }
            );
        }
    }
}
