using Agent.Exceptions;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;

namespace Agent.MessageHandling
{
    //assumes agent already joined the game
    public class MessageHandler
    {
        SenderReceiverQueueAdapter _gmConnection;
        AgentInfo _agentInfo;
        CancellationTokenSource _tokenSource = null;
        Dictionary<MessageType, int> _responsePenalties = new Dictionary<MessageType, int>(); //in miliseconds
        bool _gameOver;
        public MessageHandler(SenderReceiverQueueAdapter gmConnection, AgentInfo agentInfo)
        {
            _gmConnection = gmConnection;
            _agentInfo = agentInfo;
            ParsePenalties();
        }

        private void ParsePenalties()
        {
            var penalties = _agentInfo?.GameStartedMessage?.Penalties;
            if (penalties == null) throw new AgentInfoNotValidException();
            ParsePenalty(MessageType.CheckHoldedPieceResponse, penalties.CheckForSham);
            ParsePenalty(MessageType.DestroyPieceResponse, penalties.DestroyPiece);
            ParsePenalty(MessageType.DiscoveryResponse, penalties.Discovery);
            ParsePenalty(MessageType.ExchangeInformationResponse, penalties.InformationExchange);
            ParsePenalty(MessageType.MoveResponse, penalties.Move);
            ParsePenalty(MessageType.PutPieceResponse, penalties.PutPiece);
            //TODO:
            //Temporary, because currently there is no PickPiece penalty
            ParsePenalty(MessageType.PickPieceResponse, penalties.DestroyPiece);
        }
        private void ParsePenalty(MessageType type, string penaltyString)
        {
            var penaltyValue = Int32.Parse(penaltyString);
            _responsePenalties.Add(type, penaltyValue);
        }

        public void HandleMessages()
        {
            while (!_gameOver)
            {
                Message actionRequest = _agentInfo.Strategy.MakeDecision(_agentInfo);
                _gmConnection.Send(actionRequest);
                Log.Debug("Made decision {@Decision}", actionRequest);

                if (_tokenSource != null) _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource();
                while (!_gameOver && !_tokenSource.IsCancellationRequested)
                {
                    Message received = _gmConnection.TryTake(_tokenSource.Token, 50);
                    if (received != null)
                    {
                        Log.Debug("Recieved message {@Message}", received);
                        HandleReceived(received);
                    }
                    else
                        Log.Debug("Recieved null message {@Message}", received);
                }
            }
            Log.Information("GAME OVER");
        }

        private void HandleReceived(Message received)
        {
            if (received.MessageId == MessageType.GameEnded)
            {
                _gameOver = true;
                return;
            }
            if (_responsePenalties.ContainsKey(received.MessageId))
            {
                new Task(() =>
                {
                    Log.Debug("Agnet sleeps {@Time}", _responsePenalties[received.MessageId]);
                    Thread.Sleep(_responsePenalties[received.MessageId]);
                    _tokenSource.Cancel(false);
                }
                ).Start();
            }
            else if (received.MessageId.IsError())
            {
                HandleErrorMessage(received);
                _tokenSource.Cancel();
            }
            _agentInfo.UpdateFromMessage(received);
        }

        private void HandleErrorMessage(Message message)
        {
            if (message.MessageId != MessageType.PenaltyNotWaitedError)
                return;
            PenaltyNotWaitedError penaltyNotWaitedError = (PenaltyNotWaitedError)message.GetPayload();
            var date = DateTime.Now;
            if (date < penaltyNotWaitedError.WaitUntill)
                new Task(() =>
                {
                    Thread.Sleep(penaltyNotWaitedError.WaitUntill - DateTime.Now);
                    _tokenSource.Cancel(false);
                }).Start();
        }
    }
}
