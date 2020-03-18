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

namespace CommunicationLibrary.Tests
{
    [TestClass()]
    public class StreamMessageSenderReceiverTests
    {
        class TestParser : IParser
        {

            public string AsString<T>(Message<T> message) where T : MessagePayload
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
    }
}