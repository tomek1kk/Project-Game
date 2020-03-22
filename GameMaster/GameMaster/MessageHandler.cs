using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.MessageHandlers;

namespace GameMaster.Game
{
    public abstract class MessageHandler
    {
        protected int agentId;

        public void BaseReadMessage(Message message)
        {
            agentId = (int)message.AgentId;
            ReadMessage(message.GetPayload());
        }
        public Message ProcessRequest(Map map)
        {
            if (CheckRequest(map))
                Execute(map);
            return GetResponse(map);
        }
        public void SetTimeout()
        {
            // TODO
            return;
        }
        protected abstract bool CheckRequest(Map map);
        protected abstract Message GetResponse(Map map);
        protected abstract void Execute(Map map);
        protected abstract void ReadMessage(MessagePayload payload);
    }
}
