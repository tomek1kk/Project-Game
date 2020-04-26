using Agent.Exceptions;
using CommunicationLibrary;
using CommunicationLibrary.Error;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Serilog;
using CommunicationLibrary.Exceptions;

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
        bool _underPenalty = false;
        public MessageHandler(SenderReceiverQueueAdapter gmConnection, AgentInfo agentInfo)
        {
            _gmConnection = gmConnection;
            _agentInfo = agentInfo;
            _gmConnection.SetErrorCallback(HandleConnectionError);
            ParsePenalties();
        }

        private void ParsePenalties()
        {
            var penalties = _agentInfo?.GameStartedMessage?.Penalties;
            if (penalties == null) throw new AgentInfoNotValidException();
            ParsePenalty(MessageType.CheckHoldedPieceResponse, penalties.CheckForSham);
            ParsePenalty(MessageType.DestroyPieceResponse, penalties.DestroyPiece);
            ParsePenalty(MessageType.DiscoveryResponse, penalties.Discovery);
            ParsePenalty(MessageType.ExchangeInformationGMResponse, penalties.InformationExchange);
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
                _underPenalty = false;
                Message actionRequest = _agentInfo.Strategy.MakeDecision(_agentInfo);
                Log.Debug("Made decision {@Decision}", actionRequest);
                SendToGM(actionRequest);


                if (_tokenSource != null) _tokenSource.Dispose();
                _tokenSource = new CancellationTokenSource();

                if (actionRequest.MessageId == MessageType.ExchangeInformationResponse)
                {
                    new Task(() =>
                    {
                        Log.Debug("Agent sleeps {@Time}", _responsePenalties[MessageType.ExchangeInformationGMResponse]);
                        _underPenalty = true;
                        Thread.Sleep(_responsePenalties[MessageType.ExchangeInformationGMResponse]);
                        _tokenSource.Cancel(false);
                    }
                ).Start();
                }
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

        private void SendToGM(Message actionRequest)
        {
            try
            {
                _gmConnection.Send(actionRequest);
            }
            catch(DisconnectedException)
            {
                Log.Error("Disconnected. Closing");
                _gameOver = true;
            }

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
                    Log.Debug("Agent sleeps {@Time}", _responsePenalties[received.MessageId]);
                    _underPenalty = true;
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
                    _underPenalty = true;
                    Thread.Sleep(penaltyNotWaitedError.WaitUntill - date);
                    _tokenSource.Cancel(false);
                }).Start();
        }

        private void HandleConnectionError(Exception ex)
        {
            lock(this)
            {
                if (_gameOver) return;
                if (ex is DisconnectedException)
                {
                    Log.Error("Disconnected. Closing");
                    _gameOver = true;
                    _tokenSource.Cancel();
                }
                else if (ex is ParsingException)
                {
                    Log.Warning("Parse error while exchanging messages.");
                    Log.Warning("{incorrectMessage}", (ex as ParsingException).IncorrectMessage);
                    if (!_underPenalty)
                    {
                        //if agent is not under penalty then
                        //incorrectly parsed message could determine next penalty,
                        //in that case we have to send next action and deal with PenaltyNotWaitedError
                        //or the agent will never send next action request
                        _tokenSource.Cancel();
                    }
                }
                else
                {
                    Log.Error("Error in queue callback");
                }
            }
        }
    }
}
