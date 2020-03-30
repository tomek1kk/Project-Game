using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServerNamespace
{
    public class Descriptor : IDisposable
    {
        private static int id = 0;

        public int Id { get; private set; }
        protected TcpClient _tcpClient;
        protected NetworkStream _networkStream;
        protected StreamMessageSenderReceiver _streamMessageSenderReceiver;

        public Descriptor(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();
            _streamMessageSenderReceiver = new StreamMessageSenderReceiver(_networkStream, new Parser());
            Id = id++;
        }

        public virtual void StartReceiving(Action<Message> action)
        {
            _streamMessageSenderReceiver.StartReceiving(action);
        }

        public virtual void SendMessage(Message message)
        {
            _streamMessageSenderReceiver.Send(message);
        }

        public void Dispose()
        {
            _tcpClient.Dispose();
            _networkStream.Dispose();
            _streamMessageSenderReceiver.Dispose();
        }
    }
}
