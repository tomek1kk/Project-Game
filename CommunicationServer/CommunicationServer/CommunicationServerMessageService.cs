using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using CommunicationLibrary;

namespace CommunicationServer
{
    public class CommunicationServerMessageService : IMessageService
    {
        private TcpListener server;
        private int gameMasterPort;
        private int agentPort;
        private int port;
        private string ip;

        public void InitService()
        {
            // GET CONFIG
            IPAddress ipAddress = IPAddress.Parse(ip);

            server = new TcpListener(ipAddress, port);
            server.Start();
        }

        public void ListenForMessages()
        {
            TcpClient agent = new TcpClient("Agent", agentPort);
            TcpClient gameMaster = new TcpClient("GameMaster", gameMasterPort);

            while (true)
            {

            }
        }

        public bool Send<T>(T message, int port) where T : Message
        {
            throw new NotImplementedException();
        }
    }
}
