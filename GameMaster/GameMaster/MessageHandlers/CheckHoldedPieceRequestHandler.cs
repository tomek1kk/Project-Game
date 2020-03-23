using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.MessageHandlers
{
    public class CheckHoldedPieceRequestHandler : MessageHandler
    {
        private bool _sham;
        protected override bool CheckRequest(Map map)
        {
            return map.GetPlayerById(_agentId).IsHolding && map.GetPlayerById(_agentId).IsUnlocked;
        }

        protected override void Execute(Map map)
        {
            _sham = map.GetPlayerById(_agentId).Holding.IsSham();
        }

        protected override Message GetResponse(Map map)
        {
            return new Message<CheckHoldedPieceResponse>()
            {
                AgentId = _agentId,
                MessagePayload = new CheckHoldedPieceResponse()
                {
                    Sham = _sham
                }
            };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            return;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.CheckForShamPenalty));
        }
    }
}
