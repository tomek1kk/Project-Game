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
        bool _gameOver;
        Penalizer _penalizer;
        public MessageHandler(SenderReceiverQueueAdapter gmConnection, AgentInfo agentInfo)
        {
            _gmConnection = gmConnection;
            _agentInfo = agentInfo;
            _gmConnection.SetErrorCallback(HandleConnectionError);
            _penalizer = new Penalizer(agentInfo.GameStartedMessage.Penalties);
        }

        public void HandleMessages()
        {
            while (!_gameOver)
            {
                Message actionRequest = _agentInfo.Strategy.MakeDecision(_agentInfo);
                Log.Debug("Made decision {@Decision}", actionRequest);
                SendToGM(actionRequest);
                _penalizer.PenalizeOnSend(actionRequest);
                while (!_gameOver && _penalizer.UnderPenalty)
                {
                    Message received = _gmConnection.TryTake(50);
                    if (received != null)
                    {
                        Log.Debug("Recieved message {@Message}", received);
                        HandleReceived(received);
                    }
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
            Log.Information("Received message with id {MessageId}", received.MessageId);
            if (received.MessageId == MessageType.GameEnded)
            {
                _gameOver = true;
                return;
            }
            _penalizer.PenalizeOnReceive(received);
            if (received.MessageId.IsError() && received.MessageId != MessageType.PenaltyNotWaitedError)
                _penalizer.ClearPenalty();
            _agentInfo.UpdateFromMessage(received);
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
                }
                else if (ex is ParsingException)
                {
                    Log.Warning("Parse error while exchanging messages.");
                    Log.Warning("{incorrectMessage}", (ex as ParsingException).IncorrectMessage);
                    if (_penalizer.UnblockTimeUnknown)
                    {
                        //if agent unblock time is unknown then
                        //incorrectly parsed message could determine next penalty,
                        //in that case we have to send next action and deal with PenaltyNotWaitedError
                        //or the agent will never send next action request
                        _penalizer.ClearPenalty();
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
