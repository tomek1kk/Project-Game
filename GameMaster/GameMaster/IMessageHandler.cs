using CommunicationLibrary;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface IMessageHandler
    {
        void BaseReadMessage(Message message);
        Message ProcessRequest(Map map);
        void SetTimeout();
    }
}
