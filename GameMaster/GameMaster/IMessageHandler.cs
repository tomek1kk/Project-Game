using CommunicationLibrary;
using GameMaster.Configuration;
using GameMaster.Game;

namespace GameMaster
{
    public interface IMessageHandler
    {
        Message ProcessRequest(Map map, Message message, GMConfiguration config);
    }
}
