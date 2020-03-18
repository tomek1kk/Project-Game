using CommunicationLibrary.RawMessageProcessing;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommunicationLibrary
{
    public class StreamMessageSenderReceiver : IMessageSenderReceiver
    {
        private Stream _tcpStream;
        private Thread _receivingThread;
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();
        private Action<Message> _receiveCallback;
        private IParser _parser;
        public StreamMessageSenderReceiver(Stream tcpStream, IParser parser)
        {
            _tcpStream = tcpStream;
            _parser = parser;
            _receivingThread = 
                new Thread(()=>ReceivingThreadFunction(_cancellationTokenSource.Token));
        }

        private void ReceivingThreadFunction(CancellationToken cancellationToken)
        {
            //providing raw message reader with limited access to tcp stream that cancels when
            //token is used
            RawMessageReader reader = new RawMessageReader(
                (buffer, count) =>
                {
                    var resTask = _tcpStream.ReadAsync(buffer, 0, count, cancellationToken);
                    resTask.Wait();
                    return resTask.Result;
                }
                );

            //message reading loop
            while(!cancellationToken.IsCancellationRequested)
            {
                string nextMessageString;
                try
                {
                    nextMessageString = reader.GetNextMessageAsString();
                }
                catch(Exception)
                //either input stream has closed or thread was cancelled, in both cases we end the thread
                {
                    break;
                }
                Message nextMessage = _parser.Parse(nextMessageString);
                _receiveCallback(nextMessage);
            }
        }
        public void Dispose()
        {
            _cancellationTokenSource.Cancel();
            _receivingThread.Join();
        }

        public void Send(Message m)
        {
            RawMessageSender rawMessageSender = new RawMessageSender(
                (bytes, count) => _tcpStream.Write(bytes, 0, count));
            string messageString = _parser.AsString(m);
            rawMessageSender.SendMessage(messageString);
        }

        public void StartReceiving(Action<Message> receiveCallback)
        {
            _receiveCallback = receiveCallback;
            _receivingThread.Start();
        }
    }
}
