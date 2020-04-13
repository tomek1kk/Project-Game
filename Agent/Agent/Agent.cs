using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;
using FibonacciHeap;
using Agent.AgentBoard;
using Agent.MessageHandling;
using CommunicationLibrary.Response;
using CommunicationLibrary.Information;
using Agent.Strategies;
using Serilog;
using CommunicationLibrary.Exceptions;
using System.Threading.Tasks;

namespace Agent
{
    public class Agent : IDisposable
    {
        private SenderReceiverQueueAdapter _communicator;
        private TcpClient _client;
        public AgentConfiguration Configuration { get; set; }
        public AgentInfo AgentInfo;
        private CancellationTokenSource _joiningGame = new CancellationTokenSource();

        public Agent(AgentConfiguration configuration)
        {
            this.Configuration = configuration;
            _client = new TcpClient(Configuration.CsIp, Configuration.CsPort);
            NetworkStream stream = _client.GetStream();
            this._communicator = new SenderReceiverQueueAdapter(new StreamMessageSenderReceiver(stream, new Parser()),
                HandleJoinTimeError);
        }

        

        public void StartListening()
        {
            if (TryJoinGame())
            {
                MessageHandler m = new MessageHandler(_communicator, AgentInfo);
                m.HandleMessages();
            }
        }
        public bool TryJoinGame()
        {
            Task<bool> t = new Task<bool>(() => HandleGameJoining());
            t.Start();
            try
            {
                t.Wait(_joiningGame.Token);
            }
            catch (OperationCanceledException)
            {
                return false;
            }
            return t.Result;
            
        }

        private bool HandleGameJoining()
        {
            try
            {
                Message joinGameRequest = new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = Configuration.TeamId } };
                _communicator.Send(joinGameRequest);

                Log.Debug("Sending join game request: {@Request}", joinGameRequest);
                Message m = _communicator.Take();
                if (m.MessageId != MessageType.JoinGameResponse)
                {
                    Log.Error("No responce for join game request");
                    return false;
                }
                var joinGameResponse = (JoinGameResponse)m.GetPayload();
                if (!(joinGameResponse.Accepted ?? false))
                {
                    Log.Information("Join game request declined");
                    return false;
                }
                m = _communicator.Take();
                if (m.MessageId != MessageType.GameStarted)
                {
                    Log.Error("No information about starting game");
                    return false;
                }
                var gameStarted = (GameStarted)m.GetPayload();
                SetAgentInfo(gameStarted);
                Log.Information("GAME STARTED");
                return true;
            }
            catch(DisconnectedException)
            {
                Log.Error("Disconnected, closing");
                return false;
            }
        }

        public void SetAgentInfo(GameStarted gameInfo)
        {
            var strategy = new StrategyHandler(gameInfo.BoardSize.X.Value, gameInfo.BoardSize.Y.Value).GetStrategy(Configuration.Strategy);
            this.AgentInfo = new AgentInfo(strategy, gameInfo);
        }

        private void HandleJoinTimeError(Exception ex)
        {
            lock (this)
            {
                if (_joiningGame.IsCancellationRequested) return;
                if (ex is DisconnectedException)
                {
                    Log.Error("Disconnected, closing");
                    _joiningGame.Cancel();
                }
                else if (ex is ParsingException)
                {
                    Log.Error("Parse error while joining. Not recoverable. Closing");
                    Log.Error("{incorrectMessage}", (ex as ParsingException).IncorrectMessage);
                }
                else
                {
                    Log.Error("Error in queue callback");
                }
            }
        }

        public void Dispose()
        {
            _client.Dispose();
            _communicator.Dispose();
        }
    }
}

