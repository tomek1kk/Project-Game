using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Model;
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
            _sham = map.GetPlayerById(_agentId).Holding.IsSham();
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
            return new Message<CheckHoldedPieceResponse>()
            {
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
