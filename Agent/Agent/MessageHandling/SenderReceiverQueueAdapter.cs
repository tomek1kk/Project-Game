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

        public Message TryTake(CancellationToken cancellationToken, int millisecondsTimeout)
        {
            try
            {
                //queue.take(cancellationToken) doesn't cancel for some reason
                _queue.TryTake(out Message result, millisecondsTimeout, cancellationToken);
                return result;
            }
            catch(OperationCanceledException)
            {
                return null;
            }
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
