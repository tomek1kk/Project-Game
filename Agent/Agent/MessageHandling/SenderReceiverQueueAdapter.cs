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
        private Action<Exception> _errorCallback;
        public SenderReceiverQueueAdapter(IMessageSenderReceiver adaptedSenderReceiver, Action<Exception> errorCallback = null)
        {
            _adaptedSenderReceiver = adaptedSenderReceiver;
            _errorCallback = errorCallback;
            _adaptedSenderReceiver.StartReceiving(
                message => _queue.Add(message),
                exception=>_errorCallback?.Invoke(exception));
        }
        public Message Take()
        {
            return _queue.Take();
        }

        public void SetErrorCallback(Action<Exception> errorCallback)
        {
            _errorCallback = errorCallback;
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
