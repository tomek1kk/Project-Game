using CommunicationLibrary;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Text;
using System.Threading;

namespace Agent.MessageHandling
{
  
    public class SenderReceiverQueueAdapter : IDisposable
    {
        private BlockingCollection<Message> _queue = new BlockingCollection<Message>();
        private IMessageSenderReceiver _adaptedSenderReceiver;
        public SenderReceiverQueueAdapter(IMessageSenderReceiver adaptedSenderReceiver)
        {
            _adaptedSenderReceiver = adaptedSenderReceiver;
            _adaptedSenderReceiver.StartReceiving(message => _queue.Add(message));
        }
        public Message Take()
        {
            return _queue.Take();
        }

        public Message Take(CancellationToken cancellationToken)
        {
            return _queue.Take(cancellationToken);
        }
        public void Send(Message message)
        {
            _adaptedSenderReceiver.Send(message);
        }

        public void Dispose()
        {
            _adaptedSenderReceiver.Dispose();
        }
    }
}
