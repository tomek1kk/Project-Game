using CommunicationLibrary;
using CommunicationLibrary.Request;
using CommunicationServerNamespace;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommunicationServerTests
{
    [TestClass]
    public class CommunicationServerTests
    {
        private Configuration config = new Configuration()
        {
            GMPort = 8081,
            AgentPort = 8080,
            CsIP = "127.0.0.1"
        };
        [TestMethod]
        public void TestGMConnection()
        {
            //given
            CommunicationServer cs = new CommunicationServer(config);
            //when
            cs.StartConnectingGameMaster();
            Thread thread = new Thread(cs.AcceptGameMaster);
            thread.Start();
            //then
            //doesnt throw exceptions
            Thread.Sleep(100);
            TcpClient tcpClient = new TcpClient(cs.IpAddress, cs.PortCSforGM);
            NetworkStream networkStream = tcpClient.GetStream();
            networkStream.Close();

            networkStream.Dispose();
            tcpClient.Dispose();
            networkStream.Dispose();
        }

        [TestMethod]
        public void TestAgentConnection()
        {
            //given
            CommunicationServer cs = new CommunicationServer(config);
            //when
            Thread thread = new Thread(cs.ConnectAgents);
            thread.Start();
            //then
            //doesnt throw exceptions
            Thread.Sleep(100);
            TcpClient tcpClient = new TcpClient(cs.IpAddress, cs.PortCSforAgents);
            NetworkStream networkStream = tcpClient.GetStream();
            StreamMessageSenderReceiver streamMessageSenderReceiver = new StreamMessageSenderReceiver(networkStream, new Parser());
            streamMessageSenderReceiver.Send(new Message<JoinGameRequest>(new JoinGameRequest()));
            networkStream.Close();

            networkStream.Dispose();
            tcpClient.Dispose();
            networkStream.Dispose();
        }
    }
}
