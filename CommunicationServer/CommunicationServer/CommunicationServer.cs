using CommunicationLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Text.Json;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;

namespace CommunicationServer
{
    public class CommunicationServer
    {
        private Dictionary<int, Descriptor> correlation;
        private Descriptor GMDescriptor;


        static void Main(string[] args)
        {
            // TODO: Get config
            IPAddress ipAddress = IPAddress.Parse("127.0.0.1");
            TcpListener tcpListener = new TcpListener(ipAddress, 8080);
            tcpListener.Start();

            while (true)
            {
                Console.Write("Waiting for a connection... ");
                Parser parser = new Parser();
                TcpClient client = tcpListener.AcceptTcpClient();
                Console.WriteLine("Connected!");
                Thread t = new Thread(HandleClient);
                t.Start(client);

            }
        }

        private static void HandleClient(object client)
        {
            Console.WriteLine("In new thread");

            NetworkStream stream = ((TcpClient)client).GetStream();

            StreamMessageSenderReceiver streamMessageSenderReceiver = new StreamMessageSenderReceiver(stream, new Parser());
            BlockingCollection<Message> messages = new BlockingCollection<Message>();

            streamMessageSenderReceiver.StartReceiving(message =>
            {
                Console.WriteLine("Received message: " + message.MessageId);
                messages.Add(message);
                SendResponse(message, streamMessageSenderReceiver);
            });

        }

        private static void SendResponse(Message message, IMessageSenderReceiver messageSender)
        {
            switch (message.MessageId)
            {
                case MessageType.JoinGameRequest:
                    messageSender.Send(new Message<JoinGameRequest>() { MessagePayload = new JoinGameRequest() { TeamId = "bbb" } });
                    break;
                default:
                    messageSender.Send(new Message<NotDefinedError>() { MessagePayload = new NotDefinedError() { HoldingPiece = true } });
                    break;

            }
        }

    }
}
