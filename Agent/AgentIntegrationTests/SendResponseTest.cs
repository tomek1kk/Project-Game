using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationLibrary;
using CommunicationLibrary.Request;
using Agent;
using System.IO;
using System.Net.Sockets;
using System.Threading.Tasks;
using System.Net;
using CommunicationLibrary.Information;

namespace AgentIntegrationTests
{
    [TestClass]
    class SendResponseTest
    {
        [TestMethod]
        public void TestSendingGameJoin()
        {
            File.WriteAllText("TMPpath.txt",
               "{\"CsIP\": \"127.0.0.1\"," +
               "\"CsPort\": 8080," +
               "\"teamID\": \"red\"," +
               "\"strategy\": 1}");
            string[] args = new string[1] { "./TMPpath.txt" };

            AgentConfiguration configuration = AgentConfiguration.ReadConfiguration(args);

            TcpListener serverSideListener = new TcpListener(IPAddress.Any, configuration.CsPort);
            serverSideListener.Start();
            TcpClient serverSide = null;
            var task = new Task(() => serverSide = serverSideListener.AcceptTcpClient());
            task.Start();

            Agent.Agent agent = new Agent.Agent(configuration);
            agent.StartListening();

            task.Wait();
            serverSideListener.Stop();
            IMessageSenderReceiver senderReceiver = new StreamMessageSenderReceiver(serverSide.GetStream(), new Parser());
            senderReceiver.StartReceiving(message =>
            {
                Assert.AreEqual(message.MessageId, MessageType.JoinGameRequest);
            });
        }
    }
}
