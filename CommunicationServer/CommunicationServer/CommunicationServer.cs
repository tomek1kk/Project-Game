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
using CommunicationServer.Helpers;

namespace CommunicationServer
{
    public class CommunicationServer
    {
        private List<AgentDescriptor> _agentsConnections = new List<AgentDescriptor>();
        private Descriptor _gameMasterConnection;
        private bool isWaitingForMoreAgents = true; //to me, there will be info from game master when we stop listening for new agent clients.
        private string _ipAddress = "127.0.0.1";
        private int _portCS = 8081;

        public void ConnectGameMaster()
        {
            Console.WriteLine("GM connect");
            IPAddress ipAddress = IPAddress.Parse(_ipAddress);
            TcpListener tcpListener = new TcpListener(ipAddress, _portCS);
            tcpListener.Start();
            TcpClient client = tcpListener.AcceptTcpClient();
            _gameMasterConnection = new Descriptor(client);
            _gameMasterConnection.StartReceiving(GetGMMessage);
            Console.WriteLine("GM end");
        }

        private void GetGMMessage(Message message)
        {
            if (message.IsGameStarted()) isWaitingForMoreAgents = false;
            if (message.IsEndGame())
            {
                HandleEndGame(message);
                _gameMasterConnection.Dispose();
                return;
            }

            Console.WriteLine("I've got such message: " + message.GetPayload());
            Log.Information("GetGMMessege: {@m}", message);
            AgentDescriptor agent = _agentsConnections.Find(x => x.Id == message.AgentId);
            agent.SendMessage(message);
        }

        public void ConnectAgents()
        {
            Console.WriteLine("Agent connect");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ipAddress, 8080);
            tcpListener.Start();
            int i = 0;
            while (isWaitingForMoreAgents)
            {
                TcpClient agentClient = tcpListener.AcceptTcpClient();
                AgentDescriptor agent = new AgentDescriptor(agentClient);
                _agentsConnections.Add(agent);
                agent.StartReceiving(GetAgentMessage);
                Console.WriteLine("Agent connected: " + ++i);
                Log.Information("New agent connected.");
            }
            Console.WriteLine("Agent end");
        }

        private void GetAgentMessage(Message message)
        {
            lock (this)
            {
                Console.WriteLine("I've got such message: " + message.GetPayload());
                Log.Information("GetAgentMessage: {@m}", message);
                _gameMasterConnection.SendMessage(message);
            }
        }
        private void HandleEndGame(Message message)
        {
            foreach (var agent in _agentsConnections)
            {
                agent.SendMessage(message);
                _agentsConnections.ForEach(x => x.Dispose());
            }
        }
    }
}
