using CommunicationLibrary;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.MessageHandlers
{
    public class PutPieceRequestHandler : MessageHandler
    {
        private bool _agentHasNoPiece = false;
        private string _returnedEnum;

        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            CheckIfAgentHasPenalty(map);
        }
        protected override bool CheckRequest(Map map)
        {
            _agentHasNoPiece = map.GetPlayerById(_agentId).Holding == null;
            return !_agentHasNoPiece;
        }

        protected override void Execute(Map map)
        {
            AbstractField position = map.GetPlayerById(_agentId).Position;
            AbstractPiece piece = map.GetPlayerById(_agentId).Holding;
            bool isSham = piece.IsSham();
            bool isGoalField = map.GetPlayerById(_agentId).Position.IsGoalField;
            bool isInGoalArea = map.IsInsideBlueGoalArea(position.X, position.Y) || map.IsInsideBlueGoalArea(position.X, position.Y);
            map.GetPlayerById(_agentId).Holding = null;
            if (isInGoalArea)
            {
                if (isSham)
                    _returnedEnum = "SchamOnGoalArea";
                else
                {
                    if (isGoalField)
                    {
                        _returnedEnum = "NormalOnGoalField";
                        position.Put(piece);
                    }
                    else
                    {
                        _returnedEnum = "NormapOnNonGoalField";
                    }
                }
            }
            else
            {
                _returnedEnum = "TaskField";
                position.Put(piece);
            }
            //TODO: generating new pieces?
        }

        protected override Message GetResponse(Map map)
        {
            if (_agentHasNoPiece)
                return new Message<PutPieceError>()
                {
                    AgentId = _agentId,
                    MessagePayload = new PutPieceError()
                    {
                        ErrorSubtype = "AgentNotHolding"
                    }
                };
            else
                return new Message<PutPieceResponse>()
                {
                    AgentId = _agentId,
                    MessagePayload = new PutPieceResponse()
                    {
                        //ReturnedEnum = _returnedEnum //this need to be add in documentation - https://github.com/MINI-IO/IO-project-game/issues/119
                    }
                };

        }

        protected override void ReadMessage(MessagePayload payload)
        {
            return;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            map.GetPlayerById(_agentId).TryLock(DateTime.Now.AddMilliseconds(config.PutPenalty));
        }
    }
}
