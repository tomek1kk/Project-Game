﻿using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections;
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

        public Agent()
        {
            configuration = new AgentConfiguration()
            {
                CsIp = "127.0.0.1",
                CsPort = 8080,
                TeamId = Enum.GetName(typeof(Team), Team.Blue)
            };
            Thread thread = new Thread(Communicate);
            thread.Start();
            JoinTheGame();
        }

        static void Main(string[] args)
        {
            Agent agent = new Agent();
        }

        public void JoinTheGame()
        {
            JoinGameRequest joinGameRequest = new JoinGameRequest()
            {
                TeamId = configuration.TeamId
            };
            messageQueue.Enqueue(JsonParser.ToJSON<JoinGameRequest>(joinGameRequest));
        }

        private void Communicate()
        {
            IPAddress ipAddress = IPAddress.Parse(configuration.CsIp);

            TcpClient client = new TcpClient(ipAddress.ToString(), configuration.CsPort);

            Console.WriteLine("Agent connected");
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());

            while (true) // while game has not ended
            {
                if (messageQueue.Count > 0)
                {
                    string m = messageQueue.Dequeue();
                    writer.WriteLine(m);
                    writer.Flush();
                    if (m != "dc")
                    {
                        String server_string = reader.ReadLine();
                        Console.WriteLine(server_string);
                    }
                    else
                        break;
                }
            }
            reader.Close();
            writer.Close();
            client.Close();

        }
    }
}

