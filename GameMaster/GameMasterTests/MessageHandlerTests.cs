using CommunicationLibrary;
using CommunicationLibrary.Error;
using FluentAssertions;
using GameMaster.Configuration;
using GameMaster.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMasterTests
{
    [TestClass]
    public class MessageHandlerTests
    {
        MessageHandlerMock messageHandler = new MessageHandlerMock();
        Message message = new Message<NotDefinedError>();
        Map map = new Map(new GMConfiguration() { BoardX = 10, BoardY = 10, NumberOfPlayers = 10 });

        class MessageHandlerMock : MessageHandler
        {
            public int AgentId => _agentId;

            protected override void ClearHandler()
            {
                return;
            }
            protected override void CheckAgentPenaltyIfNeeded(Map map)
            {
                return;
            }

            protected override bool CheckRequest(Map map)
            {
                return false;
            }

            protected override void Execute(Map map)
            {
                return;
            }

            protected override Message GetResponse(Map map)
            {
                return new Message<NotDefinedError>()
                {
                };
            }

            protected override void ReadMessage(MessagePayload payload)
            {
                return;
            }

            protected override void SetTimeout(GMConfiguration config, Map map)
            {
                return;
            }

            public void SetHasTimePenalty(bool value)
            {
                _hasTimePenalty = value;
            }
        }

        [TestMethod]
        public void TestProcessRequest()
        {
            // Arrange
            var expectedId = 3;
            messageHandler.SetHasTimePenalty(false);
            message.AgentId = expectedId;
            map.AddPlayer(Team.Blue, expectedId);
            var expectedResult = new Message<NotDefinedError>()
            {
                AgentId = expectedId
            };

            // Act
            var result = messageHandler.ProcessRequest(map, message, null);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestProcessRequestPenaltyNotWaited()
        {
            // Arrange
            var expectedId = 3;
            messageHandler.SetHasTimePenalty(true);
            message.AgentId = expectedId;
            map.AddPlayer(Team.Blue, expectedId);
            var expectedResult = new Message<PenaltyNotWaitedError>()
            {
                AgentId = expectedId,
                MessagePayload = new PenaltyNotWaitedError()
                {

                }
            };

            // Act
            var result = messageHandler.ProcessRequest(map, message, null);

            // Assert
            result.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestProcessRequestIdAssignment()
        {
            // Arrange
            var expectedId = 3;
            messageHandler.SetHasTimePenalty(true);
            message.AgentId = expectedId;
            map.AddPlayer(Team.Blue, expectedId);

            // Act
            messageHandler.ProcessRequest(map, message, null);

            // Assert
            messageHandler.AgentId.Should().Be(expectedId);
        }
    }
}
