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
                (buffer, count, offset) =>
                {
                    var resTask = _tcpStream.ReadAsync(buffer, offset, count, cancellationToken);
                    resTask.Wait();
                    return resTask.Result;
                }
                );

            //message reading loop
            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    
                    string nextMessageString = GetNextMessageAsString(reader);
                    Message nextMessage = ParseMessage(nextMessageString);
                    CallMessageCallback(nextMessage);

                }
                catch(OperationCanceledException)
                {
                    break;
                }
                catch(Exception e)
                {
                    if (_errorCallback != null)
                        _errorCallback.Invoke(e);
                }
            }
        }
        private string GetNextMessageAsString(RawMessageReader reader)
        {
            try
            {
                return reader.GetNextMessageAsString();
            }
            catch (Exception e)
            {
                throw new DisconnectedException(e);
            }
        }

        private Message ParseMessage(string messageString)
        {
            try
            {
                return _parser.Parse(messageString);
            }
            catch (Exception e)
            {
                throw new ParsingException(messageString, e);
            }
        }

        private void CallMessageCallback(Message message)
        {
            try
            {
                _receiveCallback(message);
            }
            catch (Exception e)
            {
                throw new Exception("Exception thrown in callback", e);
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
        /// <param name="m">Message to be sent</param>
        /// <exception cref="DisconnectedException">thrown if sending message fails</exception>
        public void Send(Message m)
        {
            RawMessageSender rawMessageSender = new RawMessageSender(
                (bytes, count) => _tcpStream.Write(bytes, 0, count));
            string messageString = _parser.AsString(m);
            try
            {
                rawMessageSender.SendMessage(messageString);
                SendStreamCheckingMessage();
            }
            catch(Exception e)
            {
                throw new DisconnectedException(e);
            }
        }

        private void SendStreamCheckingMessage()
        {
            //network stream checks connection error on send, but will throw exception on
            //next send after one which lead to discovery of disconnection
            //so we need to call send two times - first to make socket realize it is disconnected
            //second to make it call an exception
            _tcpStream.Write(new byte[0], 0, 0);
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
