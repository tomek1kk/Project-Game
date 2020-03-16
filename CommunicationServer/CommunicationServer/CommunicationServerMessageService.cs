using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using CommunicationLibrary;

namespace CommunicationServer
{
    public class CommunicationServerMessageService : IMessageSenderReceiver
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

        private void ProcessRequests(object argument)
        {
            TcpClient client = (TcpClient)argument;

            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());
            string s = String.Empty;
            while (!(s = reader.ReadLine()).Equals("Exit") || (s == null))
            {
                Console.WriteLine("Received message:");
                Console.WriteLine(s);
                writer.WriteLine("From server: " + s);
                writer.Flush();
            }
            reader.Close();
            writer.Close();
            client.Close();
            Console.WriteLine("Agent connection closed!");
        }

        public void Send(Message m)
        {
            throw new NotImplementedException();
        }

        public void StartReceiving(Action<Message> receiveCallback)
        {
            TcpListener listener = null;
            listener = new TcpListener(IPAddress.Any, 8080);
            listener.Start();
            Console.WriteLine("Server started");
            while (true)
            {
                Console.WriteLine("Waiting for connections");
                TcpClient client = listener.AcceptTcpClient();
                Console.WriteLine("Accepted new client connection...");
                Thread t = new Thread(ProcessRequests);
                t.Start(client);
            }
        }

        public void Dispose()
        {
            throw new NotImplementedException();
        }
    }
}
