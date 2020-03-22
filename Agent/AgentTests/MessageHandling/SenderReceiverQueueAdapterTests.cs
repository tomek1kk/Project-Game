using Microsoft.VisualStudio.TestTools.UnitTesting;
using Agent.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using CommunicationLibrary;
using System.Collections.Concurrent;
using CommunicationLibrary.Request;
using System.IO;
using AgentTests.HelperClasses;

namespace Agent.MessageHandling.Tests
{
    [TestClass()]
    public class SenderReceiverQueueAdapterTests
    {
        [TestMethod()]
        public void TestCanTransferMessage()
        {
            Stream stream = new EchoStream();

            Message<JoinGameRequest> expected = new Message<JoinGameRequest>
                (new JoinGameRequest { TeamId = "Hello" });

            SenderReceiverQueueAdapter adapter
                = new SenderReceiverQueueAdapter(
                    new StreamMessageSenderReceiver(stream, new Parser()));

            //when
            adapter.Send(expected);
            Message received = adapter.Take();

            //then
            Assert.AreEqual(expected.MessagePayload.TeamId,
                ((Message<JoinGameRequest>)received).MessagePayload.TeamId);

            //after
            adapter.Dispose();
        }
    }
}