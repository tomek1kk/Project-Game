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
        public AgentConfiguration ReadConfiguration()
        {
            // todo: read configuration from json file
            return new AgentConfiguration()
            {
                CsIp = "127.0.0.1",
                CsPort = 8080,
                TeamId = Enum.GetName(typeof(Team), Team.Blue)
            };
        }
       
        static void Main(string[] args)
        {
            Agent agent = new Agent();
            var configuration = agent.ReadConfiguration();

            TcpClient client = new TcpClient(configuration.CsIp, configuration.CsPort);
            NetworkStream stream = client.GetStream();
            StreamMessageSenderReceiver streamMessageSenderReceiver = new StreamMessageSenderReceiver(stream, new Parser());
            streamMessageSenderReceiver.StartReceiving(message => Console.WriteLine("Got message: " + message.MessageId));
            streamMessageSenderReceiver.Send(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = "blue" } });
            
            Random r = new Random();
            while (true)
            {

                Console.ReadKey();
                var p = r.Next() % 2;
                if (p == 0)
                    streamMessageSenderReceiver.Send(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest { TeamId = "blue" } });
                else
                    streamMessageSenderReceiver.Send(new Message<PickPieceRequest>() { MessagePayload = new PickPieceRequest() });
            }
            streamMessageSenderReceiver.Dispose();

        }


    }
}

