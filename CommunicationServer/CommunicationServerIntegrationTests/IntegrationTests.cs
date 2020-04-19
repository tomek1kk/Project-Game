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
                communicationServer.StartConnectingGameMaster();
            TcpClient gameMasterSide = new TcpClient();
            gameMasterSide.Connect("127.0.0.1", 8081);
            communicationServer.AcceptGameMaster();

            var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
            AgentConnectTask.Start();

            TcpClient agentSide = new TcpClient("127.0.0.1", 8080);

            IMessageSenderReceiver senderReceiverGameMaster = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());
            var message = new Message<GameStarted>()
            {
                AgentId = 1,
                MessagePayload = new GameStarted()
            };

            var expectedMessageId = MessageType.GameStarted;
            Message receivedMessage = null;
            IMessageSenderReceiver senderReceiverAgent = new StreamMessageSenderReceiver(agentSide.GetStream(), new Parser());
                Semaphore semaphore = new Semaphore(0, 100);
                
                senderReceiverAgent.StartReceiving(m => {
                    semaphore.Release();
                     receivedMessage = m; }, 
                     (e) => {
                         int k = 5;
                     });

            senderReceiverGameMaster.Send(message);

            AgentConnectTask.Wait();

            //for (int i = 0; receivedMessage == null && i < 10; i++)
            //    Thread.Sleep(100);
                semaphore.WaitOne();
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
                IMessageSenderReceiver senderReceiverGameMaster = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());
                communicationServer.AcceptGameMaster();

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

                var gameStartMessage = new Message<GameStarted>()
                {
                    AgentId = 1,
                    MessagePayload = new GameStarted()
                };
                senderReceiverGameMaster.Send(gameStartMessage);
                AgentConnectTask.Wait();

                var expectedMessageId = MessageType.DiscoveryRequest;
                Message receivedMessage = null;
                
                senderReceiverGameMaster.StartReceiving(m => { receivedMessage = m; });

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
    }
}
