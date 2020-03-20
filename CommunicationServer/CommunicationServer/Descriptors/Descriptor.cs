using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServer
{
    public class Descriptor
    {
        private static int id = 0;

        public int Id { get; private set; }
        protected TcpClient _tcpClient;// do we need that?
        protected NetworkStream _networkStream;// do we need that?
        protected StreamMessageSenderReceiver _streamMessageSenderReceiver;

        public Descriptor(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();
            _streamMessageSenderReceiver = new StreamMessageSenderReceiver(_networkStream, new Parser());
            Id = id++;
        }


        protected void StartReceiving(Action<Message> action)
        {
            _streamMessageSenderReceiver.StartReceiving(action);
        }

        protected void SendMessage(Message message)
        {
            _streamMessageSenderReceiver.Send(message);
        }
    }
}
