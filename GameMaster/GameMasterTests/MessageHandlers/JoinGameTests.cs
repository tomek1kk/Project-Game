using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Model;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using FluentAssertions;
using GameMaster.Configuration;
using GameMaster.Game;
using GameMaster.MessageHandlers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMasterTests.MessageHandlers.Tests
{
    [TestClass]
    public class JoinGameTests
    {
        JoinGameRequestHandler handler = new JoinGameRequestHandler();
        GMConfiguration config = new GMConfiguration()
        {
            BoardX = 40,
            BoardY = 40,
            NumberOfPlayers = 10
        };

        [TestMethod]
        public void TestJoinGameAccepted()
        {
            //given
            var agentId = 2;
            var map = new Map(config);

            var message = new Message<JoinGameRequest>()
            {
                AgentId = agentId,
                MessagePayload = new JoinGameRequest()
                {
                    TeamId = "blue"
                }
            };
            Message<JoinGameResponse> expectedResult = new Message<JoinGameResponse>()
            {
                AgentId = agentId,
                MessagePayload = new JoinGameResponse()
                {
                    Accepted = true,
                    AgentID = agentId
                }
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestJoinGamePlayerAlreadyOnMap()
        {
            //given
            var agentId = 2;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);

            var message = new Message<JoinGameRequest>()
            {
                AgentId = agentId,
                MessagePayload = new JoinGameRequest()
                {
                    TeamId = "blue"
                }
            };
            Message<NotDefinedError> expectedResult = new Message<NotDefinedError>()
            {
                AgentId = agentId,
                MessagePayload = new NotDefinedError()
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestJoinGameTeamsFull()
        {
            //given
            var agentId = 11;
            var maxNumberOfPlayers = 10; // should be in configuration
            var map = new Map(config);
            for (int i = 0; i < maxNumberOfPlayers; i++)
                map.AddPlayer(Team.Blue, i + 1);

            var message = new Message<JoinGameRequest>()
            {
                AgentId = agentId,
                MessagePayload = new JoinGameRequest()
                {
                    TeamId = "blue"
                }
            };
            Message<JoinGameResponse> expectedResult = new Message<JoinGameResponse>()
            {
                AgentId = agentId,
                MessagePayload = new JoinGameResponse()
                {
                    Accepted = false,
                    AgentID = agentId
                }
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

    }
}