using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.MessageHandling
{
    class MessageHandler
    {
        IMessageSenderReceiver _gmConnection;
        AgentInfo _agentInfo;
        public MessageHandler(IMessageSenderReceiver gmConnection, AgentInfo agentInfo)
        {
            _gmConnection = gmConnection;
            _agentInfo = agentInfo;
        }
        public void HandleMessages()
        {

        }
    }
}
