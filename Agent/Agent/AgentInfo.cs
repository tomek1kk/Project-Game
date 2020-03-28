using System;
using System.Collections.Generic;
using System.Drawing;
using Agent.AgentBoard;
using Agent.Exceptions;
using Agent.Strategies;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using CommunicationLibrary.Response;

namespace Agent
{
    public class AgentInfo
    {
        private GameStarted _gameStartedMessage;
        public GameStarted GameStartedMessage
        {
            get => _gameStartedMessage;
            set
            {
                _gameStartedMessage = value;
                Position = new Point(value.Position.X.Value, value.Position.Y.Value);
                IsLeader = value.LeaderId == value.AgentId;
            }
        }

        public int LeaderId => _gameStartedMessage.LeaderId;
        public IEnumerable<int> AlliesIds => _gameStartedMessage.AlliesIds;
        public int GoalAreaSize => _gameStartedMessage.GoalAreaSize;
        public Point Position { get; private set; }
        public bool IsLeader { get; private set; }
        public bool HasPiece { get; private set; }
        public string GoalDirection => _gameStartedMessage.TeamId == "red" ? "N" : "S";
        public (int start, int end) goalArea { get; private set; }

        public IStrategy Strategy { get; private set; }
        public AgentInfo(IStrategy strategy, GameStarted gameStarted)
        {
            Strategy = strategy;
            HasPiece = false;
            if (gameStarted == null)
                throw new AgentInfoNotValidException("GameStarted bad config.");
            GameStartedMessage = gameStarted;
            goalArea = gameStarted.TeamId == "red"
                ? (gameStarted.BoardSize.Y.Value - 1 - gameStarted.GoalAreaSize, gameStarted.BoardSize.Y.Value)
                : (gameStarted.GoalAreaSize, 0);
        }
        public bool inGoalArea()
        {
            return Position.Y >= goalArea.start && Position.Y <= goalArea.end;
        }
        public void UpdateFromMessage(Message received)
        {
            switch (received.MessageId)
            {
                case MessageType.CheckHoldedPieceResponse:
                    CheckHoldedPieceHandler((CheckHoldedPieceResponse)received.GetPayload());
                    break;
                case MessageType.MoveResponse:
                    MoveResponseHandler((MoveResponse)received.GetPayload());
                    break;
                case MessageType.PickPieceResponse:
                    HasPiece = true;
                    break;
                case MessageType.PutPieceResponse | MessageType.DestroyPieceRequest:
                    HasPiece = false;
                    break;

                //discovery??
            }
            Strategy.UpdateMap(received, Position);
        }

        private void CheckHoldedPieceHandler(CheckHoldedPieceResponse checkHoldedPieceResponse)
        {
            if (!checkHoldedPieceResponse.Sham.HasValue)
                throw new Exception("Sham get null!");

            if (checkHoldedPieceResponse.Sham.Value)
                HasPiece = false;
        }

        private void MoveResponseHandler(MoveResponse moveResponse)
        {
            Position = new Point(moveResponse.CurrentPosition.X.Value, moveResponse.CurrentPosition.Y.Value);
        }
    }
}
