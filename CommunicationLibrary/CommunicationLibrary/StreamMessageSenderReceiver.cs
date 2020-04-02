using CommunicationLibrary.Exceptions;
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
        private Action<Exception> _errorCallback;
        public StreamMessageSenderReceiver(Stream tcpStream, IParser parser)
        {
            _tcpStream = tcpStream;
            _parser = parser;
            _receivingThread =
                new Thread(() => ReceivingThreadFunction(_cancellationTokenSource.Token));
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
            while (!cancellationToken.IsCancellationRequested)
            {
                string nextMessageString;
                try
                {

                    try
                    {
                        nextMessageString = reader.GetNextMessageAsString();
                    }
                    catch (Exception e)
                    {
                        throw new DisconnectedException(e);
                    }
                    Message nextMessage;
                    try
                    {
                        nextMessage = _parser.Parse(nextMessageString);
                    }
                    catch (Exception e)
                    {
                        throw new ParsingException(e);
                    }
                    try
                    {
                        _receiveCallback(nextMessage);
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception thrown in callback", e);
                    }
                }
                //either input stream has closed or thread was cancelled, in both cases we end the thread
                catch(Exception e)
                {
                    if (_errorCallback != null)
                        _errorCallback.Invoke(e);
                    break;
                }
            }
        }
        public void Dispose()
        {
            _tcpStream.Dispose();
            if(_receiveCallback != null)
            {
                _cancellationTokenSource.Cancel();
                _receivingThread.Join();
            }
        }

        /// <summary>
        /// Sends a message through connection
        /// </summary>
        /// <param name="m"></param>
        /// <exception cref="DisconnectedException">thrown if sending message fails</exception>
        public void Send(Message m)
        {
            RawMessageSender rawMessageSender = new RawMessageSender(
                (bytes, count) => _tcpStream.Write(bytes, 0, count));
            string messageString = _parser.AsString(m);
            try
            {
                rawMessageSender.SendMessage(messageString);
            }
            catch(Exception e)
            {
                throw new DisconnectedException(e);
            }
        }

        /// <summary>
        /// Starts new thread that listens for incoming messages on this connection
        /// </summary>
        /// <param name="receiveCallback">Will be called whenever new message arrives on this connection</param>
        public void StartReceiving(Action<Message> receiveCallback)
        {
            _receiveCallback = receiveCallback;
            _receivingThread.Start();
        }

        /// <summary>
        /// Starts new thread that listens for incoming messages on this connection
        /// </summary>
        /// <param name="receiveCallback">Will be called whenever new message arrives on this connection</param>
        /// <param name="errorCallback">
        /// Will be called whenever receiving thread throws an exception.
        /// Its argument is:
        /// DisconnectedException - if thread failed while reading from stream
        /// ParsingException - if thread failed while parsing read message
        /// Exception - if <paramref name="receiveCallback"/> throws an exception,
        /// exception thrown by receive callback will be its inner exception
        /// </param>
        public void StartReceiving(Action<Message> receiveCallback, Action<Exception> errorCallback)
        {
            _errorCallback = errorCallback;
            StartReceiving(receiveCallback);
        }
    }
}
