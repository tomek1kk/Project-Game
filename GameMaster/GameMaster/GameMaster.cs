using CommunicationLibrary;
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
using GameMaster.MessageHandlers;
using CommunicationLibrary.Information;

namespace GameMaster
{
    public class GameMaster : IDisposable
    {
        readonly IGuiMantainer _guiMantainer;
        readonly IMessageHandler _messageHandler;
        readonly GMConfiguration _gmConfiguration;
        private StreamMessageSenderReceiver _communicator;
        private TcpClient _client;
        private bool _gameStarted = false;
        Map _map;


        public GameMaster(IGuiMantainer guiMantainer, GMConfiguration config, IMessageHandler messageHandler)
        {
            _guiMantainer = guiMantainer;
            _gmConfiguration = config;
            _messageHandler = messageHandler;
        }
        public void Start()
        {
            
            //TODO: rest of starting game master

            _client = new TcpClient("127.0.0.1", 8081);
            _communicator = new StreamMessageSenderReceiver(_client.GetStream(), new Parser());

            _map = new Map(_gmConfiguration);
            InitGui();

            _communicator.StartReceiving(GetCSMessage);
            Console.WriteLine("Try connect");


            Thread.Sleep(100000);
            _guiMantainer.StopGui();
        }
        private void GetCSMessage(Message message)
        {
            Console.WriteLine(message.MessageId + "  " + message.GetPayload() + "agent id :: "+message.AgentId);
            if (message.GetPayload().ValidateMessage() == false || message.AgentId == null || (_gameStarted == false && message.MessageId != MessageType.JoinGameRequest))
            {
                _communicator.Send(new Message<NotDefinedError>()
                {
                    AgentId = message.AgentId,
                    MessagePayload = new NotDefinedError()
                });
                return;
            }
            
            var response = _messageHandler.ProcessRequest(_map, message, _gmConfiguration);
            _communicator.Send(response);
        }

        public void StartGame()
        {
            GameStarter gameStarter = new GameStarter(_communicator, _gmConfiguration);
            gameStarter.StartGame(_map.Players);
            _gameStarted = true;
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
            _guiMantainer.StartGui(_map, new CallbackGuiActionsExcecutor(()=>StartGame()));
        }

        public void Dispose()
        {
            _client.Dispose();
            _communicator.Dispose();
        }
    }
}
