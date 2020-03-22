using CommunicationLibrary;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster
{
    public interface IMessageHandler
    {
        Message ProcessRequest(Map map, Message message, GMConfiguration config);
    }
}
