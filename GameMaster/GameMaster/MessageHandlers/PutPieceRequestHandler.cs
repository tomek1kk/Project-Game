using CommunicationLibrary;
using CommunicationLibrary.Model;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using GameMaster.Configuration;
using GameMaster.Game;
using System;

namespace GameMaster.MessageHandlers
{
    public class PutPieceRequestHandler : MessageHandler
    {
        private bool _agentHasNoPiece = false;
        private PutResultEnum _returnedEnum;

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
            {
                map.AddPiece();
                PutPieceInGoalArea(piece, position, map);
            }
            else
                PutPieceOutsideGoalArea(piece, position);
        }

        private void PutPieceInGoalArea(AbstractPiece piece, AbstractField position, Map map)
        {
            if (piece.IsSham())
                PutShamInGoalArea(piece, position);
            else
                PutNonShamInGoalArea(piece, position, map);
        }

        private void PutPieceOutsideGoalArea(AbstractPiece piece, AbstractField position)
        {
            _returnedEnum = PutResultEnum.TaskField;
            position.Put(piece);
        }

        private void PutShamInGoalArea(AbstractPiece piece, AbstractField position)
         => _returnedEnum = PutResultEnum.ShamOnGoalArea;

        private void PutNonShamInGoalArea(AbstractPiece piece, AbstractField position, Map map)
        {
            position.Discover();
            if (position.IsGoalField)
                PutNonShamOnGoal(piece, position, map);
            else
                PutNormalOnNonGoal(piece, position);
        }

        private void PutNonShamOnGoal(AbstractPiece piece, AbstractField position, Map map)
        {
            map.ScorePoint(position, _agentId);
            _returnedEnum = PutResultEnum.NormalOnGoalField;
            position.Put(piece);
        }

        private void PutNormalOnNonGoal(AbstractPiece piece, AbstractField position)
          => _returnedEnum = PutResultEnum.NormalOnNonGoalField;

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
                        PutResult = _returnedEnum
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
