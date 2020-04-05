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

namespace Agent
{
    public class Agent : IDisposable
    {
        private SenderReceiverQueueAdapter _communicator;
        private TcpClient _client;
        public AgentConfiguration Configuration { get; set; }
        public AgentInfo AgentInfo;

        public Agent(AgentConfiguration configuration)
        {
            this.Configuration = configuration;
            _client = new TcpClient(Configuration.CsIp, Configuration.CsPort);
            NetworkStream stream = _client.GetStream();
            this._communicator = new SenderReceiverQueueAdapter(new StreamMessageSenderReceiver(stream, new Parser()));
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
            //not tested

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

        public void SetAgentInfo(GameStarted gameInfo)
        {
            var strategy = new StrategyHandler(gameInfo.BoardSize.X.Value, gameInfo.BoardSize.Y.Value).GetStrategy(Configuration.Strategy);
            this.AgentInfo = new AgentInfo(strategy, gameInfo);
        }

        public void Dispose()
        {
            _client.Dispose();
            _communicator.Dispose();
        }
    }
}

