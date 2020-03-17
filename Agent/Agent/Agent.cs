using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading;

namespace Agent
{
    public class Agent
    {
        public static Queue<string> messageQueue = new Queue<string>();
        private AgentConfiguration configuration;
        private IParser parser = new Parser();

        public Agent()
        {
            configuration = new AgentConfiguration()
            {
                CsIp = "127.0.0.1",
                CsPort = 8080,
                TeamId = Enum.GetName(typeof(Team), Team.Blue)
            };

            TcpClient client = new TcpClient(configuration.CsIp, configuration.CsPort);
            NetworkStream stream = client.GetStream();
            StreamMessageSenderReceiver streamMessageSenderReceiver = new StreamMessageSenderReceiver(stream, new Parser());

            int i = 0;
            while (i++ < 100)
            {
                streamMessageSenderReceiver.Send(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = "blue" } });
                streamMessageSenderReceiver.Send(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = "red" } });
                Console.ReadKey();
            }
            streamMessageSenderReceiver.Dispose();
        }

        static void Main(string[] args)
        {
            Agent agent = new Agent();
        }

        public void JoinTheGame()
        {

        }
    }
}

