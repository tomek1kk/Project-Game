using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Model;
using CommunicationLibrary.Request;
using Serilog;
using System;
using System.Collections.Generic;
using System.Text;

namespace Agent.MessageHandling
{
    public class Penalizer
    {
        Dictionary<MessageType, int> _responsePenalties = new Dictionary<MessageType, int>(); //in miliseconds
        int _exchangePenalty;
        private DateTime _blockedUntil;
        public bool UnderPenalty => UnblockTimeUnknown || DateTime.Now < _blockedUntil;

        public bool UnblockTimeUnknown { get; private set; } = true;

        public Penalizer(Penalties penalties)
        {
            ParsePenalties(penalties);
        }
        private void ParsePenalties(Penalties penalties)
        {
            ParsePenalty(MessageType.CheckHoldedPieceResponse, penalties.CheckForSham);
            ParsePenalty(MessageType.DestroyPieceResponse, penalties.DestroyPiece);
            ParsePenalty(MessageType.DiscoveryResponse, penalties.Discovery);
            ParsePenalty(MessageType.MoveResponse, penalties.Move);
            ParsePenalty(MessageType.PutPieceResponse, penalties.PutPiece);
            //TODO:
            //Temporary, because currently there is no PickPiece penalty
            ParsePenalty(MessageType.PickPieceResponse, penalties.DestroyPiece);
            _exchangePenalty = Int32.Parse(penalties.InformationExchange);
        }
        private void ParsePenalty(MessageType type, string penaltyString)
        {
            var penaltyValue = Int32.Parse(penaltyString);
            _responsePenalties.Add(type, penaltyValue);
        }
        public void PenalizeOnReceive(Message receivedMessage)
        {
            DateTime newBlockedUntil = DateTime.MinValue;

            if (_responsePenalties.ContainsKey(receivedMessage.MessageId))
            {
                newBlockedUntil = DateTime.Now.AddMilliseconds(_responsePenalties[receivedMessage.MessageId]);
                Log.Debug("Penalty on receive {penalty}", _responsePenalties[receivedMessage.MessageId]);
            }
            else if (receivedMessage.MessageId == MessageType.PenaltyNotWaitedError)
            {
                newBlockedUntil = ((PenaltyNotWaitedError)receivedMessage.GetPayload()).WaitUntill;
                Log.Debug("Penalty not waited, waiting until {unblock_time}", newBlockedUntil);
            }

            if(DateTime.Compare(_blockedUntil, newBlockedUntil) < 0)
            {
                _blockedUntil = newBlockedUntil;
                Log.Debug("Updating penalty {unblock_time}", _blockedUntil);
                UnblockTimeUnknown = false;
            }
        }

        public void PenalizeOnSend(Message sentMessage)
        {
            //assumes that if message is being sent then there is no penalty
            if (sentMessage.MessageId == MessageType.ExchangeInformationRequest
                || sentMessage.MessageId == MessageType.ExchangeInformationResponse)
            {
                _blockedUntil = DateTime.Now.AddMilliseconds(_exchangePenalty);
                UnblockTimeUnknown = false;
                Log.Debug("Penalty on send {penalty}, {unblock_time}", _exchangePenalty, _blockedUntil);
            }
            else
            {
                UnblockTimeUnknown = true;
            }
        }

        public void ClearPenalty()
        {
            UnblockTimeUnknown = false;
            _blockedUntil = DateTime.MinValue;
        }
    }
}
