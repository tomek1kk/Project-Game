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

namespace CommunicationServerNamespace
{
    public class CommunicationServer
    {
        private List<AgentDescriptor> _agentsConnections = new List<AgentDescriptor>();
        private Descriptor _gameMasterConnection;
        private bool isWaitingForMoreAgents = true; //to me, there will be info from game master when we stop listening for new agent clients.
        public string IpAddress { get; private set; } = "127.0.0.1";
        public int PortCSforGM { get; private set; } = 8081;
        public int PortCSforAgents { get; private set; } = 8080;

        public void ConnectGameMaster()
        {
            Console.WriteLine("GM connect");
            IPAddress ipAddress = IPAddress.Parse(IpAddress);
            TcpListener tcpListener = new TcpListener(ipAddress, PortCSforGM);
            tcpListener.Start();
            TcpClient client = tcpListener.AcceptTcpClient();
            _gameMasterConnection = new Descriptor(client);
            _gameMasterConnection.StartReceiving(GetGMMessage, HandleConnectionError);
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
            SendMessageWithErrorHandling(agent, message);
        }

        public void ConnectAgents()
        {
            Console.WriteLine("Agent connect");
            IPAddress ipAddress = IPAddress.Parse(IpAddress);
            TcpListener tcpListener = new TcpListener(ipAddress, PortCSforAgents);
            tcpListener.Start();
            int i = 0;
            while (isWaitingForMoreAgents)
            {
                TcpClient agentClient = tcpListener.AcceptTcpClient();
                AgentDescriptor agent = new AgentDescriptor(agentClient);
                _agentsConnections.Add(agent);
                agent.StartReceiving(GetAgentMessage, HandleConnectionError);
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
                SendMessageWithErrorHandling(_gameMasterConnection, message);
            }
        }

        private void SendMessageWithErrorHandling(Descriptor descriptor, Message message)
        {
            try
            {
                descriptor.SendMessage(message);
            }
            catch(Exception e)
            {
                HandleConnectionError(e);
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

        private void HandleConnectionError(Exception connectionError)
        {
            if(connectionError is DisconnectedException)
            {
                if (connectionError.Data.Contains("agentId"))
                    Log.Error("Agent {id} disconnected, closing server", (int)connectionError.Data["agentId"]);
                else
                    Log.Error("Game Master disconnected, closing server");
                DisconnectAll();
            }
            else if(connectionError is ParsingException)
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

        private void DisconnectAll()
        {
            foreach (var connection in _agentsConnections)
                connection.Dispose();
            _gameMasterConnection.Dispose();
        }
    }
}
