using CommunicationLibrary;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunicationLibrary.Response;

namespace GameMaster.MessageHandlers
{
    public class DestroyPieceRequestHandler : MessageHandler
    {
        protected override bool CheckRequest(Map map)
        {
            return map.GetPlayerById(_agentId).IsHolding && map.GetPlayerById(_agentId).IsUnlocked;
        }

        protected override void Execute(Map map)
        {
            map.GetPlayerById(_agentId).Holding = null;
        }

        protected override Message GetResponse(Map map)
        {
            return new Message<DestroyPieceResponse>()
            {
                AgentId = _agentId,
                MessagePayload = new DestroyPieceResponse() {}
            };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            return;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.DestroyPiecePenalty));
        }
    }
}
