using CommunicationLibrary;
using CommunicationLibrary.Error;
using GameMaster.Configuration;
using GameMaster.GUI;
using System;
using System.Net.Sockets;
using System.Threading;
using GameMaster.Game;
using Serilog;
using CommunicationLibrary.Exceptions;

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
        private Map _map;
        private GameEnder _gameEnder;


        public GameMaster(IGuiMantainer guiMantainer, GMConfiguration config, IMessageHandler messageHandler)
        {
            _guiMantainer = guiMantainer;
            _gmConfiguration = config;
            _messageHandler = messageHandler;
            _gameEnder = new GameEnder();
        }
        public void Start()
        {
            _client = new TcpClient(_gmConfiguration.CsIP, _gmConfiguration.CsPort);
            _communicator = new StreamMessageSenderReceiver(_client.GetStream(), new Parser());
            Log.Information("StreamMessageSenderReceiver started");

            _map = new Map(_gmConfiguration);
            Log.Information("Map created");

            InitGui();
            Log.Information("GUI started");

            _communicator.StartReceiving(GetCSMessage, EndGame);
            Log.Information("Started received messages");

        }
        public void WaitForEnd()
        {
            while (true)
            {
                lock (_gameEnder)
                {
                    if (_gameEnder.lockCondition)
                    {
                        Log.Information("lock_condition true, game ends");
                        break;
                    }
                }
                Thread.Sleep(100);
            }
            Thread.Sleep(20000);
            _guiMantainer.StopGui();
            Log.Information("GUI stopped");
        }
        private void GetCSMessage(Message message)
        {
            lock (_messageHandler)
            {
                if (_map.GameEnded)
                    return;
                Console.WriteLine(message.MessageId + "  " + message.GetPayload() + "agent id :: " + message.AgentId);
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
                if (_map.GameEnded)
                {
                    EndGame(new Exception("Dummy exception"));
                }
                else
                {
                    _communicator.Send(response);
                }
            }
        }

        public void StartGame()
        {
            GameStarter gameStarter = new GameStarter(_communicator, _gmConfiguration);
            gameStarter.StartGame(_map.Players);
            _gameStarted = true;
            _map.GameStarted = true;
        }

        public void EndGame(Exception e)
        {
            if(e is DisconnectedException)
            {
                _map.ErrorMessage = "One of modules disconnected";
            }
            if (_map.GameEnded && _gameEnder.endGameNotHandled)
            {
                _gameEnder.endGameNotHandled = false;
                Log.Information("GameEnd");
                _gameEnder.GameEndHandler(_map, _communicator);
            }
            else if (_gameEnder.endGameNotHandled)
            {
                _gameEnder.endGameNotHandled = false;
                Log.Information("ErrorGameEnd");
            }
            lock (_gameEnder)
            {
                _gameEnder.lockCondition = true;
            }
        }
        private void InitGui()
        {
            //Manual Gui Data Provider can be replaced with another implementation of IGuiDataProvider
            //once code related to game board is complete
            //_guiDataProvider = new ManualGuiDataProvider(_gmConfiguration.BoardX, _gmConfiguration.BoardY, _gmConfiguration.GoalAreaHight);
            //_guiMantainer.StartGui(_guiDataProvider);

            //prototype of GameMaster Map
            _guiMantainer.StartGui(_map, new CallbackGuiActionsExcecutor(() => StartGame()));
        }

        public void Dispose()
        {
            _client.Dispose();
            _communicator.Dispose();
        }
    }
}
