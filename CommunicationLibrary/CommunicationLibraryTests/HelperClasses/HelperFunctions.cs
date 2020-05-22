using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading.Tasks;

namespace CommunicationLibraryTests.HelperClasses
{
    static class HelperFunctions
    {
        public static (StreamMessageSenderReceiver agentSide, StreamMessageSenderReceiver gmSide)
            GetGmAgentConnections()
        {
            Stream agentToGmStream = new EchoStream();
            Stream gmToAgentStream = new EchoStream();
            Stream agentSideStream = new StreamRWJoin(gmToAgentStream, agentToGmStream);
            Stream gmSideStream = new StreamRWJoin(agentToGmStream, gmToAgentStream);
            var agentReceiver
                = new StreamMessageSenderReceiver(
                        agentSideStream,
                        new Parser());
            var gmReceiver
                = new StreamMessageSenderReceiver(
                        gmSideStream,
                        new Parser());
            return (agentReceiver, gmReceiver);
        }

        public static (TcpClient client, TcpClient server) TcpConnectClientAndServer()
        {
            TcpListener serverSideListener = new TcpListener(IPAddress.Any, 0);
            serverSideListener.Start();
            int port = ((IPEndPoint)serverSideListener.LocalEndpoint).Port;
            TcpClient serverSide = null;
            var task = new Task(() => serverSide = serverSideListener.AcceptTcpClient());
            task.Start();
            TcpClient clientSide = new TcpClient("localhost", port);
            task.Wait();
            serverSideListener.Stop();
            return (clientSide, serverSide);
        }
    }
}
