using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.MessageHandling
{
    //assumes agent already joined the game
    class MessageHandler
    {
        SenderReceiverQueueAdapter _gmConnection;
        AgentInfo _agentInfo;
        public MessageHandler(SenderReceiverQueueAdapter gmConnection, AgentInfo agentInfo)
        {
            _gmConnection = gmConnection;
            _agentInfo = agentInfo;
        }
        public void HandleMessages()
        {

        }
    }
}
