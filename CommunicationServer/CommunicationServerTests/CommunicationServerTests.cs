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
        [TestMethod]
        public void TestGMConnection()
        {
            //given
            CommunicationServer cs = new CommunicationServer();
            //when
            Thread thread = new Thread(cs.ConnectGameMaster);
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
            CommunicationServer cs = new CommunicationServer();
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
