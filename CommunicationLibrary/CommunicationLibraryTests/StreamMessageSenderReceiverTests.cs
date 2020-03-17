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

namespace CommunicationLibrary.Tests
{
    [TestClass()]
    public class StreamMessageSenderReceiverTests
    {
        class TestMessage : Message
        {
            public string Text { get; set; }

            //public override int MessageId
            //{
            //    get
            //    {
            //        return 123;
            //    }
            //}

            public override bool ValidateMessage()
            {
                return true;
            }
        }
        class TestParser : IParser
        {
            public string AsString<T>(T message) where T : Message
            {
                return message.MessageId.ToString();
            }

            public Message Parse(string messageString)
            {
                return new TestMessage { Text = messageString };
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
            Assert.AreEqual(expected, ((TestMessage)received).Text);
        }

        [TestMethod()]
        public void TestStreamMessageSenderReceiverCanTransferMessage()
        {
            TestMessage expected = new TestMessage();
            Stream stream = new EchoStream();
            BinaryReader reader = new BinaryReader(stream);

            StreamMessageSenderReceiver streamMessageSenderReceiver
                = new StreamMessageSenderReceiver(stream, new TestParser());
            BlockingCollection<Message> messages = new BlockingCollection<Message>();
            streamMessageSenderReceiver.StartReceiving(message => messages.Add(message));

            //when
            streamMessageSenderReceiver.Send(expected);
            Message received = messages.Take();

            //then
            Assert.AreEqual(expected.MessageId, ((TestMessage)received).MessageId);
        }
    }
}