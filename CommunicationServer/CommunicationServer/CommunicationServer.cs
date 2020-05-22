using CommunicationLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.Json;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using Serilog;
using CommunicationServerNamespace.Helpers;
using CommunicationLibrary.Exceptions;
using System.Threading.Tasks;
using CommunicationLibrary.Response;

namespace CommunicationServerNamespace
{
    public class CommunicationServer : IDisposable
    {
        private List<AgentDescriptor> _agentsConnections = new List<AgentDescriptor>();
        private Descriptor _gameMasterConnection;
        public string IpAddress { get; private set; }
        public int PortCSforGM { get; private set; }
        public int PortCSforAgents { get; private set; }
        private TcpListener _gmListener;

        private bool _acceptingAgents = true;
        private TaskCompletionSource<bool> _gameOver = new TaskCompletionSource<bool>();

        public CommunicationServer(Configuration config)
        {
            IpAddress = config.CsIP;
            PortCSforAgents = config.AgentPort;
            PortCSforGM = config.GMPort;
        }

        public void StartConnectingGameMaster()
        {
            Console.WriteLine("GM connect");
            IPAddress ipAddress = IPAddress.Parse(IpAddress);
            _gmListener = new TcpListener(ipAddress, PortCSforGM);
            _gmListener.Start();
            PortCSforGM = ((IPEndPoint)_gmListener.LocalEndpoint).Port;
        }
        public void AcceptGameMaster()
        {
            TcpClient client = _gmListener.AcceptTcpClient();
            _gameMasterConnection = new Descriptor(client);
            _gameMasterConnection.StartReceiving(GetGMMessage, HandleConnectionError);
            _gmListener.Stop();
            Console.WriteLine("GM end");
        }

        private void GetGMMessage(Message message)
        {
            if(message.MessageId == MessageType.JoinGameResponse)
            {
                JoinGameResponse resp = (JoinGameResponse)message.GetPayload();
                if(resp.Accepted == false)
                {
                    _agentsConnections.Remove(_agentsConnections.Find(a => a.Id == message.AgentId));
                }
            }
            if (message.IsGameStarted()) _acceptingAgents = false;
            if (message.IsEndGame())
            {
                HandleEndGame(message);
                //_gameMasterConnection.Dispose();
                return;
            }

            Console.WriteLine("I've got such message: " + message.GetPayload());
            Log.Information("GetGMMessege: {@m}", message);
            AgentDescriptor agent = _agentsConnections.Find(x => x.Id == message.AgentId);
            SendMessageWithErrorHandling(agent, message);
        }

        public void ConnectAgents()
        {
            Console.WriteLine("Agent connect");
            IPAddress ipAddress = IPAddress.Parse(IpAddress);
            TcpListener tcpListener = new TcpListener(ipAddress, PortCSforAgents);
            tcpListener.Start();
            PortCSforAgents = ((IPEndPoint)tcpListener.LocalEndpoint).Port;
            int i = 0;
            while (_acceptingAgents)
            {
                if(tcpListener.Pending() && _acceptingAgents)
                {
                TcpClient agentClient = tcpListener.AcceptTcpClient();
                    AgentDescriptor agent = new AgentDescriptor(agentClient);
                    _agentsConnections.Add(agent);
                    agent.StartReceiving(GetAgentMessage, HandleConnectionError);
                    Console.WriteLine("Agent connected: " + ++i);
                    Log.Information("New agent connected.");
                }
                else if(!_acceptingAgents)
                {
                    continue;
                }
                else
                {
                    Thread.Sleep(1000);
                }
            }
            tcpListener.Stop();
            Console.WriteLine("Agent end");
        }

        public void WaitForGameOver()
        {
            _gameOver.Task.Wait();
        }

        private void GetAgentMessage(Message message)
        {
            lock (this)
            {
                Console.WriteLine("I've got such message: " + message.GetPayload());
                Log.Information("GetAgentMessage: {@m}", message);
                SendMessageWithErrorHandling(_gameMasterConnection, message);
            }
        }

        private void SendMessageWithErrorHandling(Descriptor descriptor, Message message)
        {
            try
            {
                descriptor.SendMessage(message);
            }
            catch (Exception e)
            {
                HandleConnectionError(e);
            }
        }

        private void HandleEndGame(Message message)
        {
            foreach (var agent in _agentsConnections)
            {
                agent.SendMessage(message);
            }
            StopWorking();
        }

        private void HandleConnectionError(Exception connectionError)
        {
            lock (this.IpAddress)
            {
                if (_gameOver.Task.IsCompleted) return;
                if (connectionError is DisconnectedException)
                {
                    if (connectionError.Data.Contains("agentId"))
                        Log.Error("Agent {id} disconnected, closing server", (int)connectionError.Data["agentId"]);
                    else
                        Log.Error("Game Master disconnected, closing server");
                    StopWorking();
                }
                else if (connectionError is ParsingException)
                {
                    if (connectionError.Data.Contains("agentId"))
                        Log.Warning("Failed to parse message from agent {id} ", (int)connectionError.Data["agentId"]);
                    else
                        Log.Warning("Failed to parse message from GM");
                    Log.Warning("Incorrect message: {message}", ((ParsingException)connectionError).IncorrectMessage);
                }
                else
                {
                    Log.Warning("Message handler threw an exception {exception} ", connectionError.ToString());
                }
            }
        }

        private void StopWorking()
        {
            _gameOver.TrySetResult(true);
            _acceptingAgents = false;
        }

        public void Dispose()
        {
            foreach (var connection in _agentsConnections)
                connection.Dispose();
            _gameMasterConnection.Dispose();
            _gameOver.TrySetResult(true);
        }
    }
}
