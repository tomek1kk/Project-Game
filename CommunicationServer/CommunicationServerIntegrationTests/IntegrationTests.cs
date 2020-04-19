using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationServerNamespace;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using CommunicationLibrary;
using CommunicationLibrary.Exceptions;
using CommunicationLibrary.Request;
using CommunicationLibrary.Information;

namespace CommunicationServerIntegrationTests
{
    [TestClass]
    public class Integration
    {
        [TestMethod]
        public void FromGMToAgent()
        {
            using (CommunicationServer communicationServer = new CommunicationServer())
            { 
                //var GMConnectTask = new Task(() => communicationServer.StartConnectingGameMaster());
                //GMConnectTask.Start();
                //    GMConnectTask.Wait();
                //    TcpClient gameMasterSide = new TcpClient();
                communicationServer.StartConnectingGameMaster();
            TcpClient gameMasterSide = new TcpClient();
            gameMasterSide.Connect("127.0.0.1", 8081);
            communicationServer.AcceptGameMaster();




            var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
            AgentConnectTask.Start();

            TcpClient agentSide = new TcpClient("127.0.0.1", 8080);

            IMessageSenderReceiver senderReceiverGameMaster = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());
            var message = new Message<DiscoveryRequest>()
            {
                AgentId = 1,
                MessagePayload = new DiscoveryRequest()
            };

            var expectedMessageId = MessageType.DiscoveryRequest;
            Message receivedMessage = null;
            IMessageSenderReceiver senderReceiverAgent = new StreamMessageSenderReceiver(agentSide.GetStream(), new Parser());
            senderReceiverAgent.StartReceiving(m => { receivedMessage = m; });

            senderReceiverGameMaster.Send(message);

            AgentConnectTask.Wait();
            communicationServer.WaitForGameOver();

            for (int i = 0; receivedMessage == null && i < 10; i++)
                Thread.Sleep(100);
            Assert.IsFalse(receivedMessage == null);
            Assert.AreEqual(receivedMessage.MessageId, expectedMessageId);
            gameMasterSide.Close();
            agentSide.Close();
                senderReceiverGameMaster.Dispose();
                senderReceiverAgent.Dispose();
            }
        }
        [TestMethod]
        public void FromAgentToGM()
        {
            using (CommunicationServer communicationServer = new CommunicationServer())
            {
                communicationServer.StartConnectingGameMaster();
                TcpClient gameMasterSide = new TcpClient();
                gameMasterSide.Connect("127.0.0.1", 8081);
                communicationServer.AcceptGameMaster();
                //var GMConnectTask = new Task(() => communicationServer.ConnectGameMaster());
                //GMConnectTask.Start();
                ////Thread.Sleep(200);
                //TcpClient gameMasterSide = new TcpClient("127.0.0.1", 8081);
                //GMConnectTask.Wait();

                var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
                AgentConnectTask.Start();

                TcpClient agentSide = new TcpClient("127.0.0.1", 8080);

                IMessageSenderReceiver senderReceiverAgent = new StreamMessageSenderReceiver(agentSide.GetStream(), new Parser());
                var message = new Message<DiscoveryRequest>()
                {
                    AgentId = 1,
                    MessagePayload = new DiscoveryRequest()
                };
                senderReceiverAgent.Send(message);

                var expectedMessageId = MessageType.DiscoveryRequest;
                Message receivedMessage = null;
                IMessageSenderReceiver senderReceiverGameMaster = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());
                senderReceiverGameMaster.StartReceiving(m => { receivedMessage = m; });

                for (int i = 0; receivedMessage == null && i < 10; i++)
                    Thread.Sleep(100);
                Assert.IsFalse(receivedMessage == null);
                Assert.AreEqual(receivedMessage.MessageId, expectedMessageId);

                gameMasterSide.Close();
                senderReceiverGameMaster.Dispose();
                senderReceiverAgent.Dispose();
                agentSide.Close();
            }
        }
    }
}
