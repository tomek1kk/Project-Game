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
        private bool sham;
        protected override bool CheckRequest(Map map)
        {
            return true;
        }

        protected override void Execute(Map map)
        {
            sham = false; // TODO
        }

        protected override Message GetResponse(Map map)
        {
            return new Message<CheckHoldedPieceResponse>()
            {
                AgentId = agentId,
                MessagePayload = new CheckHoldedPieceResponse()
                {
                    Sham = sham
                }
            };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            return;
        }

        protected override void SetTimeout(GMConfiguration config)
        {
            throw new NotImplementedException();
        }
    }
}
