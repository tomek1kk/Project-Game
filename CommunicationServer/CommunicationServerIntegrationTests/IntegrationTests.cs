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
        private Configuration config = new Configuration()
        {
            GMPort = 8081,
            AgentPort = 8080,
            CsIP = "127.0.0.1"
        };
        [TestMethod]
        public void ToAgentFromGM()
        {
            using (CommunicationServer communicationServer = new CommunicationServer(config))
            {
                communicationServer.StartConnectingGameMaster();
                TcpClient gameMasterSide = new TcpClient();
                gameMasterSide.Connect(config.CsIP, config.GMPort);
                communicationServer.AcceptGameMaster();

                var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
                AgentConnectTask.Start();

                TcpClient agentSide = new TcpClient(config.CsIP, config.AgentPort);

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

                senderReceiverAgent.StartReceiving(m =>
                {
                    semaphore.Release();
                    receivedMessage = m;
                },
                     (e) =>
                     {
                     });

                senderReceiverGameMaster.Send(message);

                AgentConnectTask.Wait();

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
        public void ToGMFromAgent()
        {
            using (CommunicationServer communicationServer = new CommunicationServer(config))
            {
                communicationServer.StartConnectingGameMaster();
                TcpClient gameMasterSide = new TcpClient();
                gameMasterSide.Connect(config.CsIP, config.GMPort);
                IMessageSenderReceiver senderReceiverGameMaster = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());
                communicationServer.AcceptGameMaster();

                var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
                AgentConnectTask.Start();

                TcpClient agentSide = new TcpClient(config.CsIP, config.AgentPort);

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
