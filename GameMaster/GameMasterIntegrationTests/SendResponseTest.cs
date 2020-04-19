using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using GameMaster;
using GameMaster.GUI;
using GameMaster.Configuration;
using CommunicationLibrary;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;

namespace GameMasterIntegrationTests
{
    [TestClass]
    public class SendResponseTest
    {
        [TestMethod]
        public void TestSendStartGame()
        {
            StubGuiMaintainer guiMantainer = new StubGuiMaintainer();
            GMConfiguration config = new GMConfiguration()
            {
                BoardX = 40,
                BoardY = 40,
                CsIP = "127.0.0.1",
                CsPort = 8081,
                TeamID = 3,
                MovePenalty = 1500,
                AskPenalty = 1000,
                DiscoveryPenalty = 700,
                PutPenalty = 500,
                CheckForShamPenalty = 700,
                ResponsePenalty = 1000,
                GoalAreaHight = 5,
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

            task.Wait();
            serverSideListener.Stop();
            IMessageSenderReceiver senderReceiver = new StreamMessageSenderReceiver(serverSide.GetStream(), new Parser());
            senderReceiver.StartReceiving(message =>
            {
                Assert.AreEqual(message.MessageId, MessageType.GameStarted);
            });


        }
    }
}
