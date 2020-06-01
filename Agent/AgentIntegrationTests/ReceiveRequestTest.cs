using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using Agent;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using CommunicationLibrary.Information;
using CommunicationLibrary.Response;
using System.Collections.Generic;
using CommunicationLibrary.Model;
using System.Threading;

namespace AgentIntegrationTests
{
    [TestClass]
    public class ReceiveRequestTest
    {
        [TestMethod]
        public void TestJoinGameResponse()
        {
            GameStarted defaultGameStartedMessage = new GameStarted
            {
                AgentId = 0,
                AlliesIds = new List<int> { 1 },
                BoardSize = new BoardSize { X = 10, Y = 10 },
                EnemiesIds = new List<int> { 2, 3 },
                GoalAreaSize = 3,
                LeaderId = 1,
                NumberOfGoals = 3,
                NumberOfPieces = 5,
                NumberOfPlayers = new NumberOfPlayers { Allies = 2, Enemies = 2 },
                Penalties = new Penalties
                {
                    CheckForSham = "100",
                    DestroyPiece = "100",
                    Discovery = "100",
                    InformationExchange = "100",
                    Move = "100",
                    PutPiece = "100"
                },
                Position = new Position { X = 2, Y = 2 },
                ShamPieceProbability = 0.5,
                TeamId = "red"
            };

            //gets random free port
            TcpListener serverSideListener = new TcpListener(IPAddress.Any, 0);
            serverSideListener.Start();
            int port = ((IPEndPoint)serverSideListener.LocalEndpoint).Port;

            File.WriteAllText("TMPpath.txt",
               "{\"CsIP\": \"127.0.0.1\"," +
               $"\"CsPort\": {port}," +
               "\"teamID\": \"red\"," +
               "\"strategy\": 1}");
            string[] args = new string[1] { "./TMPpath.txt" };
            AgentConfiguration configuration = AgentConfiguration.ReadConfiguration(args);

            TcpClient serverSide = null;
            var task = new Task(() => serverSide = serverSideListener.AcceptTcpClient());
            task.Start();

            Agent.Agent agent = new Agent.Agent(configuration);
            var listenTask = new Task(() =>  agent.StartListening());
            listenTask.Start();

            task.Wait();
            serverSideListener.Stop();
            IMessageSenderReceiver senderReceiver = new StreamMessageSenderReceiver(serverSide.GetStream(), new Parser());
            Message joinGameMessage = null;
            Message exchangeInfoRequest = null;

            Semaphore semaphore = new Semaphore(0, 100);
            senderReceiver.StartReceiving(message =>
            {   
                switch (message.MessageId)
                {
                    case MessageType.JoinGameRequest:

                        senderReceiver.Send(new Message<JoinGameResponse>(new JoinGameResponse() { AgentID = 1 , Accepted = true}));
                        senderReceiver.Send(new Message<GameStarted>(defaultGameStartedMessage));
                        joinGameMessage = message;
                        break;
                    default:
                        exchangeInfoRequest = message;
                        semaphore.Release();
                        break;
                }
            });

            semaphore.WaitOne();
            Assert.AreEqual(joinGameMessage.MessageId, MessageType.JoinGameRequest);
            Assert.AreEqual(exchangeInfoRequest.MessageId, MessageType.ExchangeInformationRequest);
        }
    }
}
