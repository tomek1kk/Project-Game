﻿using CommunicationLibrary;
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

namespace CommunicationServer
{
    public class CommunicationServer
    {
        private List<AgentDescriptor> _agentsConnections = new List<AgentDescriptor>();
        private Descriptor _gameMasterConnection;
        private bool isWaitingForMoreAgents = true; //to me, there will be info from game master when we stop listening for new agent clients.


        public void ConnectGameMaster()
        {
            Console.WriteLine("GM connect");
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ipAddress, 8081);
            tcpListener.Start();
            TcpClient client = tcpListener.AcceptTcpClient();
            _gameMasterConnection = new Descriptor(client);
            _gameMasterConnection.StartReceiving(GetGMMessage);
            Console.WriteLine("GM end");
        }

        public void GetGMMessage(Message message)
        {
            if (isWaitingForMoreAgents) isWaitingForMoreAgents = !isWaitingForMoreAgents; //when appropiate messege is sent
            
            Console.WriteLine("I've got such message: " + message.GetPayload());
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
            }
            Console.WriteLine("Agent end");
        }

        public void GetAgentMessage(Message message)
        {
            Console.WriteLine("I've got such message: " + message.GetPayload());
            _gameMasterConnection.SendMessage(message);
        }

    }
}
