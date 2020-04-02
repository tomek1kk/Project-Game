using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationLibrary;
using System;
using System.Collections.Generic;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading.Tasks;
using System.IO;
using Moq;
using System.Collections.Concurrent;
using CommunicationLibraryTests.HelperClasses;
using CommunicationLibrary.Request;
using System.Threading;

namespace CommunicationLibrary.Tests
{
    //EchoStream and StreamRWJoin used whereever possible to simulate tcp stream
    //because tests with them take on average < 150ms
    //and tests with tcp client and listener take on average 2s.

    //However they don't work like tcp streams in disconnection scenarios,
    //so tcp streams have to be used there
    [TestClass()]
    public class StreamMessageSenderReceiverTests
    {
        class TestParser : IParser
        {

            public string AsString(Message message)
            {
                return ((Message<JoinGameRequest>)(Message)message).MessagePayload.TeamId.ToString();
            }

            public Message Parse(string messageString)
            {
                return new Message<JoinGameRequest>(
                    new JoinGameRequest { TeamId = messageString });
            }
        }

        [TestMethod()]
        public void TestStreamMessageSenderReceiverCanReceiveMessage()
        {
            //given
            String expected = "Hello world";

            byte[] inputBuffer = new byte[20];

            byte[] textBytes = Encoding.UTF8.GetBytes(expected);
            byte[] lengthBytes = BitConverter.GetBytes((ushort)textBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(lengthBytes);
            Array.Copy(lengthBytes, 0, inputBuffer, 0, 2);
            Array.Copy(textBytes, 0, inputBuffer, 2, textBytes.Length);
            Stream stream = new MemoryStream(inputBuffer);

            StreamMessageSenderReceiver streamMessageSenderReceiver
                = new StreamMessageSenderReceiver(stream, new TestParser());
            BlockingCollection<Message> messages = new BlockingCollection<Message>();
            streamMessageSenderReceiver.StartReceiving(message => messages.Add(message));

            //when
            Message received = messages.Take();

            //then
            Assert.AreEqual(expected, ((Message<JoinGameRequest>)received).MessagePayload.TeamId);
        }

        [TestMethod()]
        public void TestStreamMessageSenderReceiverCanTransferMessage()
        {
            Stream stream = new EchoStream();

            Message<JoinGameRequest> expected = new Message<JoinGameRequest>
                (new JoinGameRequest { TeamId = "Hello" });

            StreamMessageSenderReceiver streamMessageSenderReceiver
                = new StreamMessageSenderReceiver(stream, new TestParser());
            BlockingCollection<Message> messages = new BlockingCollection<Message>();
            streamMessageSenderReceiver.StartReceiving(message => messages.Add(message));

            //when
            streamMessageSenderReceiver.Send(expected);
            Message received = messages.Take();

            //then
            Assert.AreEqual(expected.MessagePayload.TeamId,
                ((Message<JoinGameRequest>)received).MessagePayload.TeamId);
        }

        [TestMethod()]
        public void TestStreamMessageSenderReceiverCanUseParser()
        {
            Stream stream = new EchoStream();

            Message<JoinGameRequest> expected = new Message<JoinGameRequest>
                (new JoinGameRequest { TeamId = "Hello" });
            StreamMessageSenderReceiver streamMessageSenderReceiver
                = new StreamMessageSenderReceiver(stream, new Parser());

            BlockingCollection<Message> messages = new BlockingCollection<Message>();
            streamMessageSenderReceiver.StartReceiving(message => messages.Add(message));

            //when
            streamMessageSenderReceiver.Send(expected);
            Message received = messages.Take();

            //then
            Assert.AreEqual(expected.MessagePayload.TeamId,
                ((Message<JoinGameRequest>)received).MessagePayload.TeamId);
        }
        [TestMethod()]
        public void TestCallbackCalledOnDisconnect()
        {
            //given
            (TcpClient clientSide, TcpClient serverSide) = HelperFunctions.TcpConnectClientAndServer();

            Exception receivedException = null;
            StreamMessageSenderReceiver streamMessageSenderReceiver
                = new StreamMessageSenderReceiver(clientSide.GetStream(), new Parser());
            Semaphore semaphore = new Semaphore(0, 1);
            streamMessageSenderReceiver.StartReceiving(message => { }, e =>
            { receivedException = e; semaphore.Release(); });

            //when
            serverSide.Close();
            semaphore.WaitOne();

            //then
            Assert.IsNotNull(receivedException);
            Assert.IsInstanceOfType(receivedException, typeof(CommunicationLibrary.Exceptions.DisconnectedException));

            //after
            streamMessageSenderReceiver.Dispose();
            clientSide.Close();
            semaphore.Dispose();
        }


        [TestMethod()]
        public void TestCallbackCalledOnParseError()
        {
            Exception receivedException = null;
            Stream agentToGmStream = new EchoStream();
            Stream gmToAgentStream = new EchoStream();
            Stream agentSideStream = new StreamRWJoin(gmToAgentStream, agentToGmStream);
            Stream gmSideStream = new StreamRWJoin(agentToGmStream, gmToAgentStream);

            StreamMessageSenderReceiver agentSenderReceiver
                = new StreamMessageSenderReceiver(agentSideStream, new Parser());
            Semaphore semaphore = new Semaphore(0, 100);
            agentSenderReceiver.StartReceiving(message => { }, e =>
            { receivedException = e; semaphore.Release(); });

            string messageText = System.Text.Json.JsonSerializer.Serialize(new { a = "abc" });
            byte[] messageBytes = Encoding.UTF8.GetBytes(messageText);
            byte[] messageLengthBytes = BitConverter.GetBytes((ushort)messageBytes.Length);
            if (!BitConverter.IsLittleEndian) Array.Reverse(messageLengthBytes);

            //when
            gmSideStream.Write(messageLengthBytes, 0, 2);
            gmSideStream.Write(messageBytes, 0, messageBytes.Length);
            semaphore.WaitOne();

            //then
            Assert.IsNotNull(receivedException);
            Assert.IsInstanceOfType(receivedException, typeof(CommunicationLibrary.Exceptions.ParsingException));

            //after
            agentSenderReceiver.Dispose();
            gmSideStream.Close();
            semaphore.Dispose();
        }



        [TestMethod()]
        public void TestCallbackCalledOnReceiveCallbackError()
        {
            //given
            Exception receivedException = null;
            string expected = "callback";
            var (agentSide, gmSide) = HelperFunctions.GetGmAgentConnections();
            Semaphore semaphore = new Semaphore(0, 100);
            agentSide.StartReceiving(message => throw new Exception(expected), e =>
            { receivedException = e; semaphore.Release(); });

            //when
            gmSide.Send(new Message<CheckHoldedPieceRequest>(new CheckHoldedPieceRequest()));
            semaphore.WaitOne();

            //then
            Assert.IsNotNull(receivedException);
            Assert.AreEqual(receivedException.InnerException.Message, expected);

            //after
            agentSide.Dispose();
            gmSide.Dispose();
            semaphore.Dispose();
        }

        [TestMethod()]
        [ExpectedException(typeof(CommunicationLibrary.Exceptions.DisconnectedException))]
        public void TestExceptionThrownOnSendToDisconnected()
        {
            //given
            (TcpClient clientSide, TcpClient serverSide) = HelperFunctions.TcpConnectClientAndServer();
            serverSide.Close();
            var senderReceiver = new StreamMessageSenderReceiver(clientSide.GetStream(), new Parser());

            //when
            senderReceiver.Send(new Message<CheckHoldedPieceRequest>(new CheckHoldedPieceRequest()));

            //then
            //DisconnectedExceptionThrown

            //after
            senderReceiver.Dispose();
        }

    }
}