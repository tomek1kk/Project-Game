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
    public class PickPieceRequestHandler : MessageHandler
    {
        private bool _noPieceOnField;
        private bool _fieldIsOnGoalArea;
        private bool _playerAlreadyHasPiece;

        protected override void ClearHandler() { }
        protected override void CheckAgentPenaltyIfNeeded(Map map)
        {
            CheckIfAgentHasPenalty(map);
        }
        protected override bool CheckRequest(Map map)
        {
            AbstractField position = map.GetPlayerById(_agentId).Position;
            _playerAlreadyHasPiece = map.GetPlayerById(_agentId).Holding != null;
            if (_playerAlreadyHasPiece)
                return false;
            _fieldIsOnGoalArea = map.IsInsideBlueGoalArea(position) || map.IsInsideRedGoalArea(position);
            if (_fieldIsOnGoalArea)
                return false;
            _noPieceOnField = !position.ContainsPieces();
            if (_noPieceOnField)
                return false;
            return true;
        }

        protected override void Execute(Map map)
        {
            map.GetPlayerById(_agentId).Holding = map.GetPlayerById(_agentId).Position.PickUp();
        }

        protected override Message GetResponse(Map map)
        {
            if (_playerAlreadyHasPiece || _fieldIsOnGoalArea)
                return new Message<PickPieceError>()
                {
                    MessagePayload = new PickPieceError()
                    {
                        ErrorSubtype = "Other"
                    }
                };
            else if (_noPieceOnField)
                return new Message<PickPieceError>()
                {
                    MessagePayload = new PickPieceError()
                    {
                        ErrorSubtype = "NothingThere"
                    }
                };
            else
                return new Message<PickPieceResponse>()
                {
                    MessagePayload = new PickPieceResponse() { }
                };
        }

        protected override void ReadMessage(MessagePayload payload)
        {
            return;
        }

        protected override void SetTimeout(GMConfiguration config, Map map)
        {
            return;
        }
    }
}
