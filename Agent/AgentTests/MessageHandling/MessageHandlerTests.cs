using Microsoft.VisualStudio.TestTools.UnitTesting;
using Agent.MessageHandling;
using System;
using System.Collections.Generic;
using System.Text;
using AgentTests.HelperClasses;
using CommunicationLibrary;
using Moq;
using Agent.Exceptions;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using System.IO;
using CommunicationLibrary.Request;
using System.Threading.Tasks;
using CommunicationLibrary.Response;
using System.Threading;
using System.Diagnostics;
using Agent.Strategies;
using System.Drawing;

namespace Agent.MessageHandling.Tests
{
    [TestClass()]
    public class MessageHandlerTests
    {
        private GameStarted defaultGameStartedMessage = new GameStarted
        {
            AgentId = 0,
            AlliesIds = new List<int> { 1 },
            BoardSize = new BoardSize { X = 10, Y = 10 },
            EnemiesIds = new List<int> { 2, 3 },
            GoalAreaSize = 3,
            LeaderId = 1,
            NumberOfGoals = 3,
            NumberOfPieces = 5,
            NumberOfPlayers = new NumberOfPlayers { Allies = 2, Enemies = 2 },
            Penalties = new Penalties
            {
                CheckForSham = "100",
                DestroyPiece = "100",
                Discovery = "100",
                InformationExchange = "100",
                Move = "100",
                PutPiece = "100"
            },
            Position = new Position { X = 2, Y = 2 },
            ShamPieceProbability = 0.5,
            TeamId = "red"
        };

        private Message gameEndedMessage = new Message<GameEnded>(new GameEnded());

        private (SenderReceiverQueueAdapter agentSide, SenderReceiverQueueAdapter gmSide)
            GetGmAgentConnections()
        {
            Stream agentToGmStream = new EchoStream();
            Stream gmToAgentStream = new EchoStream();
            Stream agentSideStream = new StreamRWJoin(gmToAgentStream, agentToGmStream);
            Stream gmSideStream = new StreamRWJoin(agentToGmStream, gmToAgentStream);
            var agentQueueAdapter
                = new SenderReceiverQueueAdapter(
                    new StreamMessageSenderReceiver(
                        agentSideStream,
                        new Parser()));
            var gmQueueAdapter
                = new SenderReceiverQueueAdapter(
                    new StreamMessageSenderReceiver(
                        gmSideStream,
                        new Parser()));
            return (agentQueueAdapter, gmQueueAdapter);
        }

        [TestMethod()]
        [ExpectedException(typeof(AgentInfoNotValidException))]
        public void TestConstructorThrowsExceptionIfGameStartedNull()
        {
            //given

            var senderReceiverQueueAdapter
                = new SenderReceiverQueueAdapter(
                    new StreamMessageSenderReceiver(
                        new EchoStream(),
                        new Parser()));
            var strategyMock = new Mock<IStrategy>();
            AgentInfo agentInfo = new AgentInfo(strategyMock.Object, null);

            //when
            _ = new MessageHandler(senderReceiverQueueAdapter, agentInfo);

            //then
            //AgentInfoNotValidException thrown
        }
        [TestMethod()]
        public void TestSendsStrategyMessages()
        {
            //given
            var expected = new Message<DiscoveryRequest>(new DiscoveryRequest());

            var (agentSide, gmSide) = GetGmAgentConnections();


            var strategyMock = new Mock<IStrategy>();
            strategyMock.Setup(strategy => strategy.MakeDecision(It.IsAny<AgentInfo>()))
                .Returns(expected);

            AgentInfo agentInfo = new AgentInfo(strategyMock.Object, defaultGameStartedMessage);

            MessageHandler messageHandler = new MessageHandler(agentSide, agentInfo);
            new Task(() => messageHandler.HandleMessages()).Start();

            //when
            Message actual = gmSide.Take();

            //then
            Assert.AreEqual(expected.MessageId, actual.MessageId);

            //after
            gmSide.Dispose();
            agentSide.Dispose();
        }

        [TestMethod()]
        public void TestEndsAfterGameOverMessage()
        {
            //given
            var (agentSide, gmSide) = GetGmAgentConnections();

            var strategyMock = new Mock<IStrategy>();

            strategyMock.Setup(strategy => strategy.MakeDecision(It.IsAny<AgentInfo>()))
                .Returns(new Message<DiscoveryRequest>(new DiscoveryRequest()));

            AgentInfo agentInfo = new AgentInfo(strategyMock.Object, defaultGameStartedMessage);

            MessageHandler messageHandler = new MessageHandler(agentSide, agentInfo);
            Task t = new Task(() => messageHandler.HandleMessages());
            t.Start();

            //when
            gmSide.Send(gameEndedMessage);

            //then
            t.Wait();

            //after
            gmSide.Dispose();
            agentSide.Dispose();
        }

        [TestMethod()]
        public void TestRelaysResponseToStrategy()
        {
            //given
            var expected = new Message<DiscoveryResponse>(new DiscoveryResponse());
            Message actual = null;

            var (agentSide, gmSide) = GetGmAgentConnections();

            var strategyMock = new Mock<IStrategy>();

            strategyMock.Setup(strategy => strategy.MakeDecision(It.IsAny<AgentInfo>()))
                .Returns(new Message<DiscoveryRequest>(new DiscoveryRequest()));

            strategyMock.Setup(strategy =>
            strategy.UpdateMap(It.IsAny<Message>(), new Point(defaultGameStartedMessage.Position.X.Value, defaultGameStartedMessage.Position.Y.Value)))
                .Callback<Message,Point>((message,point) => actual = message);

            AgentInfo agentInfo = new AgentInfo(strategyMock.Object, defaultGameStartedMessage);

            MessageHandler messageHandler = new MessageHandler(agentSide, agentInfo);
            Task t = new Task(() => messageHandler.HandleMessages());
            t.Start();

            //when
            gmSide.Send(expected);
            gmSide.Send(gameEndedMessage);

            //then
            t.Wait();
            Assert.AreEqual(expected.MessageId, actual?.MessageId);

            //after
            gmSide.Dispose();
            agentSide.Dispose();
        }

        [TestMethod()]
        public void TestSendsNextRequestAfterDelay()
        {
            //given
            var minDelay = Int32.Parse(defaultGameStartedMessage.Penalties.Discovery) - 10;
            //discovery penalty + precision

            var (agentSide, gmSide) = GetGmAgentConnections();

            var strategyMock = new Mock<IStrategy>();

            strategyMock.Setup(strategy => strategy.MakeDecision(It.IsAny<AgentInfo>()))
                .Returns(new Message<DiscoveryRequest>(new DiscoveryRequest()));

            AgentInfo agentInfo = new AgentInfo(strategyMock.Object, defaultGameStartedMessage);

            MessageHandler messageHandler = new MessageHandler(agentSide, agentInfo);
            new Task(() => messageHandler.HandleMessages()).Start();

            gmSide.Take();
            gmSide.Send(new Message<DiscoveryResponse>(new DiscoveryResponse()));
            Stopwatch stopwatch = Stopwatch.StartNew();

            //when
            gmSide.Take();

            //then

            Assert.IsTrue(minDelay < stopwatch.ElapsedMilliseconds,
                $"The actual delay was {stopwatch.ElapsedMilliseconds}");

            //after
            gmSide.Dispose();
            agentSide.Dispose();
        }
    }
}