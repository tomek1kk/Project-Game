using CommunicationLibrary;
using CommunicationLibrary.Model;
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
        private PutResult _returnedEnum;

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
            PutPieceAt(piece, position, map);
        }

        private void PutPieceAt(AbstractPiece piece, AbstractField position, Map map)
        {
            map.GetPlayerById(_agentId).Holding = null;
            if (map.IsInGoalArea(position))
                PutPieceInGoalArea(piece, position);
            else
                PutPieceOutsideGoalArea(piece, position);
        }

        private void PutPieceInGoalArea(AbstractPiece piece, AbstractField position)
        {
            if (piece.IsSham())
                PutShamInGoalArea(piece, position);
            else
                PutNonShamInGoalArea(piece, position);
        }

        private void PutPieceOutsideGoalArea(AbstractPiece piece, AbstractField position)
        {
            _returnedEnum = PutResult.TaskField;
            position.Put(piece);
        }

        private void PutShamInGoalArea(AbstractPiece piece, AbstractField position)
         => _returnedEnum = PutResult.ShamOnGoalArea;

        private void PutNonShamInGoalArea(AbstractPiece piece, AbstractField position)
        {
            if (position.IsGoalField)
                PutNonShamOnGoal(piece, position);
            else
                PutNormalOnNonGoal(piece, position);
        }

        private void PutNonShamOnGoal(AbstractPiece piece, AbstractField position)
        {
            _returnedEnum = PutResult.NormalOnGoalField;
            position.Put(piece);
        }

        private void PutNormalOnNonGoal(AbstractPiece piece, AbstractField position)
          => _returnedEnum = PutResult.NormalOnNonGoalField;

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
