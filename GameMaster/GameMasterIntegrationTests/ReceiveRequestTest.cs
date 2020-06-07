using CommunicationLibrary;
using CommunicationLibrary.Request;
using GameMaster;
using GameMaster.Configuration;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace GameMasterIntegrationTests
{
    [TestClass]
    public class ReceiveRequestTest
    {
        [TestMethod]
        public void TestReceiveRequest()
        {
            StubGuiMaintainer guiMantainer = new StubGuiMaintainer();
            GMConfiguration config = new GMConfiguration()
            {
                BoardX = 40,
                BoardY = 40,
                CsIP = "127.0.0.1",
                CsPort = 8081,
                MovePenalty = 1500,
                DiscoveryPenalty = 700,
                PutPenalty = 500,
                CheckForShamPenalty = 700,
                InformationExchangePenalty = 1000,
                GoalAreaHeight = 5,
                NumberOfGoals = 5,
                NumberOfPieces = 10,
                ShamPieceProbability = 20
            };

            TcpListener serverSideListener = new TcpListener(IPAddress.Any, config.CsPort);
            serverSideListener.Start();
            TcpClient serverSide = null;
            var task = new Task(() => serverSide = serverSideListener.AcceptTcpClient());
            task.Start();


            GameMaster.GameMaster gm = new GameMaster.GameMaster(guiMantainer, config, new ProxyMessageHandler());
            gm.Start();
            guiMantainer.StartGame();
            var gmTask = new Task(() => gm.WaitForEnd());
            gmTask.Start();

            task.Wait();
            serverSideListener.Stop();
            IMessageSenderReceiver senderReceiver = new StreamMessageSenderReceiver(serverSide.GetStream(), new Parser());
            var message = new Message<DiscoveryRequest>()
            {
                AgentId = 1,
                MessagePayload = new DiscoveryRequest()
            };
            senderReceiver.Send(message);
            senderReceiver.StartReceiving(m =>
            {
                Assert.AreEqual(m.MessageId, MessageType.DiscoveryResponse);
            });


        }
    }
}
