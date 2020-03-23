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
        private bool _noPiece;
        protected override bool CheckRequest(Map map)
        {
            _penaltyNotWaited = map.GetPlayerById(_agentId).IsUnlocked;
            _noPiece = map.GetPlayerById(_agentId).IsHolding;
            return _noPiece || _penaltyNotWaited;
        }

        protected override void Execute(Map map)
        {
            map.GetPlayerById(_agentId).Holding = null;
        }

        protected override Message GetResponse(Map map)
        {
            if (_penaltyNotWaited)
            {
                return new Message<PenaltyNotWaitedError>()
                {
                    AgentId = _agentId,
                    MessagePayload = new PenaltyNotWaitedError()
                    {
                        WaitUntill = map.GetPlayerById(_agentId).LockedTill
                    }
                };
            }
            if (_noPiece)
            {
                return new Message<NotDefinedError>()
                {
                    AgentId = _agentId,
                    MessagePayload = new NotDefinedError()
                    {
                        Position = new Position()
                        {
                            X = map.GetPlayerById(_agentId).Position.X,
                            Y = map.GetPlayerById(_agentId).Position.Y
                        },
                        HoldingPiece = map.GetPlayerById(_agentId).IsHolding
                    }
                };
            }
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
