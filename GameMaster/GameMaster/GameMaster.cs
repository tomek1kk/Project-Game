﻿using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using GameMaster.Configuration;
using GameMaster.GUI;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using GameMaster.Game;

namespace GameMaster
{
    public class GameMaster : IDisposable
    {
        readonly IGuiMantainer _guiMantainer;
        readonly GMConfiguration _gmConfiguration;
        readonly MessageHandler _messageHandler;
        private StreamMessageSenderReceiver _communicator;
        private TcpClient _client;
        ManualGuiDataProvider _guiDataProvider;
        Map _map;

        public GameMaster(IGuiMantainer guiMantainer, GMConfiguration config, MessageHandler messageHandler)
        {
            _guiMantainer = guiMantainer;
            _gmConfiguration = config;
            _messageHandler = messageHandler;
        }
        public void Start()
        {
            _map = new Map(_gmConfiguration);
            InitGui();
            //TODO: rest of starting game master

            _client = new TcpClient("127.0.0.1", 8081);
            _communicator = new StreamMessageSenderReceiver(_client.GetStream(), new Parser());
            //streamMessageSenderReceiver.Send<JoinGameRequest>(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = "DUUPA" } });
            _communicator.StartReceiving(GetCSMessage);
            Console.WriteLine("Try connect");


            Thread.Sleep(10000);
            _guiMantainer.StopGui();
        }
        private void GetCSMessage(Message message)
        {

            Console.WriteLine(message.MessageId + "  " + message.GetPayload() + "agent id :: "+message.AgentId);
            this._messageHandler.HandleMessage(message, _communicator, _map);
            //var payload = new JoinGameResponse() { AgentID = message.AgentId };

            //_communicator.Send(new Message<JoinGameResponse> { MessagePayload = payload,AgentId=message.AgentId });
        }

        public void GenerateGui()
        {
            //TODO: use manual gui data provider to set apropriate fields
            //called every time game board is updated
        }
        private void InitGui()
        {
            //Manual Gui Data Provider can be replaced with another implementation of IGuiDataProvider
            //once code related to game board is complete
            //_guiDataProvider = new ManualGuiDataProvider(_gmConfiguration.BoardX, _gmConfiguration.BoardY, _gmConfiguration.GoalAreaHight);
            //_guiMantainer.StartGui(_guiDataProvider);

            //prototype of GameMaster Map
            _guiMantainer.StartGui(_map);
        }

        public void Dispose()
        {
            _client.Dispose();
            _communicator.Dispose();
        }
    }
}
