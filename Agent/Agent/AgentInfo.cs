using System;
using System.Collections.Generic;
using System.Drawing;
using Agent.Board;
using Agent.Exceptions;
using Agent.Strategies;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using System.Linq;

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
        public List<int> AlliesIds => _gameStartedMessage.AlliesIds.ToList();
        public Point Position { get; private set; }
        public bool IsLeader { get; private set; }
        public bool HasPiece { get; private set; }
        public IStrategy Strategy { get; private set; }
        public List<RedirectedExchangeInformationRequest> ExchangeInfoRequests { get; set; } = new List<RedirectedExchangeInformationRequest>();
        public AgentInfo(IStrategy strategy, GameStarted gameStarted)
        {
            Strategy = strategy;
            HasPiece = false;
            if (gameStarted == null)
                throw new AgentInfoNotValidException("GameStarted bad config.");

            GameStartedMessage = gameStarted;
        }
        public void UpdateFromMessage(Message received)
        {
            switch (received.MessageId)
            {
                case MessageType.RedirectedExchangeInformationRequest:
                    RequestResponse((RedirectedExchangeInformationRequest)received.GetPayload());
                    break;
                case MessageType.ExchangeInformationGMResponse:
                    Strategy.GetInfo((ExchangeInformationGMResponse)received.GetPayload());
                    break;
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
                case MessageType.MoveError:
                    var position = ((MoveError)received.GetPayload()).Position;
                    Position = new Point(position.X.Value, position.Y.Value);
                    break;
                case MessageType.PutPieceError:
                    if (((PutPieceError)received.GetPayload()).ErrorSubtype == "AgentNotHolding")
                        HasPiece = false;
                    break;
                case MessageType.NotDefinedError:
                    var notDefinedError = ((NotDefinedError)received.GetPayload());
                    Position = new Point(notDefinedError.Position.X.Value, notDefinedError.Position.Y.Value);
                    HasPiece = notDefinedError.HoldingPiece.Value;
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

        public Message BegForInfo()
        {
            var req = new ExchangeInformationRequest();
            Random r = new Random();
            req.AskedAgentId = r.Next(AlliesIds.Count);
            return new Message<ExchangeInformationRequest>(req);
        }

        public void RequestResponse(RedirectedExchangeInformationRequest requesest)
        {
            if (requesest.Leader.Value && requesest.TeamId.ToLower() == _gameStartedMessage.TeamId.ToLower())
                ExchangeInfoRequests.Insert(0, requesest);
            else
                ExchangeInfoRequests.Add(requesest);
        }
    }
}
