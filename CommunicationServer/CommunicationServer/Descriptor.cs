using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;

namespace CommunicationServer
{
    public class Descriptor
    {
        private static int id = 1;

        public int Id { get; private set; }
        private TcpClient _tcpClient;// do we need that?
        private NetworkStream _networkStream;// do we need that?
        private StreamMessageSenderReceiver _streamMessageSenderReceiver;

        public Descriptor(TcpClient tcpClient)
        {
            _tcpClient = tcpClient;
            _networkStream = tcpClient.GetStream();
            _streamMessageSenderReceiver = new StreamMessageSenderReceiver(_networkStream, new Parser());
            Id = id++;
        }

        public void StartReceiving(Action<Message> action)
        {
            _streamMessageSenderReceiver.StartReceiving(action);
        }
         
        public void SendMessage<T>(Message message) where T: MessagePayload
        {
            //it cannot be generic
            _streamMessageSenderReceiver.Send<T>((Message<T>)message);
        }





    }
}
