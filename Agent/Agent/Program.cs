using CommunicationLibrary;
using CommunicationLibrary.Request;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;

namespace Agent
{
    class Program
    {
        private static Queue<Message> messageQueue = new Queue<Message>();
        static void Main(string[] args)
        {
            messageQueue.Enqueue(new MoveRequest());
            messageQueue.Enqueue(new DiscoveryRequest());

            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            int port = 8080;
            Console.WriteLine("Attempting to connect to server at IP address: " + ipAddress.ToString() + ",  port: " + port);
            TcpClient client = new TcpClient(ipAddress.ToString(), port);
            Console.WriteLine("Connection successful!");
            StreamReader reader = new StreamReader(client.GetStream());
            StreamWriter writer = new StreamWriter(client.GetStream());

            while (true) // while game has not ended
            {
                if (messageQueue.Count > 0)
                {
                    Message m = messageQueue.Dequeue();
                    string s = JsonSerializer.Serialize<Message>(m);
                    writer.WriteLine(s);
                    writer.Flush();
                    if (s != "dc")
                    {
                        String server_string = reader.ReadLine();
                        Console.WriteLine(server_string);
                    }
                }
            }
            reader.Close();
            writer.Close();
            client.Close();

        }
    }
}
