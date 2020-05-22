using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationServerNamespace;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Threading;
using CommunicationLibrary;
using CommunicationLibrary.Exceptions;
using CommunicationLibrary.Request;
using CommunicationLibrary.Information;
using System.Net;

namespace CommunicationServerIntegrationTests
{
    [TestClass]
    public class Integration
    {
        private Configuration config = new Configuration()
        {
            GMPort = 0,
            AgentPort = 0,
            CsIP = "0.0.0.0"
        };
        [TestMethod]
        public void ToAgentFromGM()
        {
            using (CommunicationServer communicationServer = new CommunicationServer(config))
            {
                //connecting game master
                communicationServer.StartConnectingGameMaster();
                TcpClient gameMasterSide = new TcpClient();
                gameMasterSide.Connect(IPAddress.Loopback, communicationServer.PortCSforGM);
                communicationServer.AcceptGameMaster();
                IMessageSenderReceiver senderReceiverGameMaster = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());

                //connecting agent
                var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
                AgentConnectTask.Start();
                while (communicationServer.PortCSforAgents == 0) Thread.Sleep(50);
                TcpClient agentSide = new TcpClient("localhost", communicationServer.PortCSforAgents);
                IMessageSenderReceiver senderReceiverAgent = new StreamMessageSenderReceiver(agentSide.GetStream(), new Parser());


                //ensuring agent is registered in communication server
                senderReceiverAgent.Send(new Message<JoinGameRequest>(new JoinGameRequest { TeamId = "red" }));
                gameMasterSide.GetStream().Read(new byte[1], 0, 1);

                //sending message to agent
                var sentMessage = new Message<GameStarted>()
                {
                    AgentId = 1,
                    MessagePayload = new GameStarted()
                };
                senderReceiverGameMaster.Send(sentMessage);


                //waiting for message in agent
                var expectedMessageId = MessageType.GameStarted;
                Message receivedMessage = null;
                Semaphore semaphore = new Semaphore(0, 100);

                senderReceiverAgent.StartReceiving(m =>
                {
                    semaphore.Release();
                    receivedMessage = m;
                },(e) =>{});
                semaphore.WaitOne();

                Assert.IsFalse(receivedMessage == null);
                Assert.AreEqual(receivedMessage.MessageId, expectedMessageId);

                //after
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
                //connecting game master
                communicationServer.StartConnectingGameMaster();
                TcpClient gameMasterSide = new TcpClient();
                gameMasterSide.Connect("localhost", communicationServer.PortCSforGM);
                communicationServer.AcceptGameMaster();
                IMessageSenderReceiver senderReceiverGameMaster
                    = new StreamMessageSenderReceiver(gameMasterSide.GetStream(), new Parser());

                //connecting agent
                var AgentConnectTask = new Task(() => communicationServer.ConnectAgents());
                AgentConnectTask.Start();
                while (communicationServer.PortCSforAgents == 0) Thread.Sleep(50);
                TcpClient agentSide = new TcpClient("localhost", communicationServer.PortCSforAgents);
                IMessageSenderReceiver senderReceiverAgent = new StreamMessageSenderReceiver(agentSide.GetStream(), new Parser());


                //ensuring agent is registered in communication server
                senderReceiverAgent.Send(new Message<JoinGameRequest>(new JoinGameRequest { TeamId = "red" }));
                Semaphore s = new Semaphore(0, 1);
                Message receivedMessage = null;
                senderReceiverGameMaster.StartReceiving(m => {receivedMessage = m; s.Release(); });
                s.WaitOne();

                var gameStartMessage = new Message<GameStarted>()
                {
                    AgentId = 1,
                    MessagePayload = new GameStarted()
                };
                senderReceiverGameMaster.Send(gameStartMessage);

                //sending message to game master
                receivedMessage = null;
                var expectedMessageId = MessageType.DiscoveryRequest;

                var message = new Message<DiscoveryRequest>()
                {
                    AgentId = 1,
                    MessagePayload = new DiscoveryRequest()
                };
                senderReceiverAgent.Send(message);

                //waiting for message in game master
                s.WaitOne();
                Assert.IsFalse(receivedMessage == null);
                Assert.AreEqual(receivedMessage.MessageId, expectedMessageId);

                //after
                gameMasterSide.Close();
                agentSide.Close();
                senderReceiverGameMaster.Dispose();
                senderReceiverAgent.Dispose();
            }
        }
    }
}
