using CommunicationLibrary;
using CommunicationLibrary.Model;
using CommunicationLibrary.Error;
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
        private bool _hasPiece;

        protected override void ClearHandler() { }
        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            CheckIfAgentHasPenalty(map);
        }

        protected override bool CheckRequest(Map map)
        {
            _hasPiece = map.GetPlayerById(_agentId).IsHolding;
            return _hasPiece;
        }

        protected override void Execute(Map map)
        {
            map.GetPlayerById(_agentId).Holding = null;
            map.AddPiece();
        }

        protected override Message GetResponse(Map map)
        {
            if (!_hasPiece)
            {
                return new Message<NotDefinedError>()
                {
                    MessagePayload = new NotDefinedError()
                    {
                        Position = (Position)map.GetPlayerById(_agentId).Position,
                        HoldingPiece = map.GetPlayerById(_agentId).IsHolding
                    }
                };
            }
            return new Message<DestroyPieceResponse>()
            {
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
