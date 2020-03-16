using CommunicationLibrary.MessageReceiving;
using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace CommunicationLibrary
{
    class TCPDescriptor : IDescriptor
    {
        private TcpClient _client;
        private Thread _receivingThread;
        private CancellationTokenSource cancellationTokenSource;
        private Action<Message> _receiveCallback;
        public TCPDescriptor(TcpClient client)
        {
            _client = client;
            _receivingThread = 
                new Thread(()=>ReceivingThreadFunction(cancellationTokenSource.Token));
            _receivingThread.Start();
        }

        private void ReceivingThreadFunction(CancellationToken cancellationToken)
        {
            RawMessageReader reader = new RawMessageReader(
                (buffer, count) =>
                {
                    var resTask = _client.GetStream().ReadAsync(buffer, 0, 2, cancellationToken);
                    resTask.Wait();
                    return resTask.Result;
                }
                );
            while(!cancellationToken.IsCancellationRequested)
            {
            }
        }
        public void Dispose()
        {
            cancellationTokenSource.Cancel();
        }

        public void Send(Message m)
        {
            throw new NotImplementedException();
        }

        public void SetReceiveCallback(Action<Message> receiveCallback)
        {
            _receiveCallback = receiveCallback;
        }
    }
}
