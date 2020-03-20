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

namespace GameMaster
{
    public class GameMaster :IDisposable

    {
        readonly IGuiMantainer _guiMantainer;
        readonly GMConfiguration _gmConfiguration;
        private StreamMessageSenderReceiver _communicator;
        ManualGuiDataProvider _guiDataProvider;

        public GameMaster(IGuiMantainer guiMantainer, GMConfiguration config)
        {
            _guiMantainer = guiMantainer;
            _gmConfiguration = config;
        }
        public void Start()
        {
            InitGui();
            //TODO: rest of starting game master

            TcpClient client = new TcpClient("127.0.0.1", 8081);
            _communicator = new StreamMessageSenderReceiver(client.GetStream(), new Parser());
            //streamMessageSenderReceiver.Send<JoinGameRequest>(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = "DUUPA" } });
            _communicator.StartReceiving(GetCSMessage);
            Console.WriteLine("Try connect");


            Thread.Sleep(10000);
            _guiMantainer.StopGui();
        }
        private void GetCSMessage(Message message)
        {

            Console.WriteLine(message.MessageId + "  " + message.GetPayload() + "agent id :: "+message.AgentId);
            var payload = new JoinGameResponse() { AgentID = message.AgentId };
            _communicator.Send(new Message<JoinGameResponse> { MessagePayload = payload,AgentId=message.AgentId });
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
            _guiDataProvider = new ManualGuiDataProvider(_gmConfiguration.BoardX, _gmConfiguration.BoardY, _gmConfiguration.GoalAreaHight);
            _guiMantainer.StartGui(_guiDataProvider);
        }

        public void Dispose()
        {
            _communicator.Dispose();
        }
    }
}
