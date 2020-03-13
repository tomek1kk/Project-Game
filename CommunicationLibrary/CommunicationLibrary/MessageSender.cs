using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;

namespace CommunicationLibrary
{
    public static class MessageSender
    {
        private static readonly int port;
        private static readonly string server;
        static MessageSender()
        {
            port = 2000;
            server = "server";
        }

        public static bool Send<T>(T message) where T : Message
        {
            if (message.ValidateMessage() == false)
                return false;

            string json = JsonSerializer.Serialize<T>(message);

            //TcpClient tcpClient = new TcpClient(server, port);
            Byte[] data = System.Text.Encoding.UTF8.GetBytes(json);

            //NetworkStream stream = tcpClient.GetStream();
            for (int i = 0; i < data.Length; i++)
                Console.Write((char)data[i]);
            Console.WriteLine();
            Console.WriteLine("port: " + port + ", server: " + server);
            //stream.Write(data, 0, data.Length);


            return true;
        }
    }
}
