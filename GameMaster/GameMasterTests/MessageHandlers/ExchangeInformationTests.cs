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
    [TestClass()]
    public class ExchangeInformationTests
    {
        private GMConfiguration config = new GMConfiguration()
        {
            BoardX = 40,
            BoardY = 40,
            CsIP = "127.0.0.1",
            CsPort = 8080,
            TeamID = 3,
            MovePenalty = 1500,
            AskPenalty = 1000,
            DiscoveryPenalty = 700,
            PutPenalty = 500,
            CheckForShamPenalty = 700,
            ResponsePenalty = 1000,
            GoalAreaHight = 5,
            NumberOfGoals = 5,
            NumberOfPieces = 10,
            ShamPieceProbability = 20
        };
        [TestMethod()]
        public void TestPlayerAsksAnother()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 5, y: 5, 1, Team.Red), (x: 4, y: 5, 2, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<ExchangeInformationRequest>()
            {
                AgentId = 1,
                MessagePayload = new ExchangeInformationRequest()
                {
                    AskedAgentId = 2
                }
            };
            var exchangeInformationHandler = new ExchangeInformationRequestHandler();
            Message<RedirectedExchangeInformationRequest> expectedResult = new Message<RedirectedExchangeInformationRequest>()
            {
                AgentId = 2,
                MessagePayload = new RedirectedExchangeInformationRequest()
                {
                    AskingId = 1,
                    Leader = false,
                    TeamId = "red"
                }
            };
            //when
            Message response = exchangeInformationHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
        [TestMethod()]
        public void TestPlayerAsksNotExistingAgnet()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 5, y: 5, 1, Team.Red)};
            var map = new Map(players: players);
            var message = new Message<ExchangeInformationRequest>()
            {
                AgentId = 1,
                MessagePayload = new ExchangeInformationRequest()
                {
                    AskedAgentId = 2
                }
            };
            var exchangeInformationHandler = new ExchangeInformationRequestHandler();
            Message<NotDefinedError> expectedResult = new Message<NotDefinedError>()
            {
                AgentId = 1,
                MessagePayload = new NotDefinedError()
                {
                    Position = new Position() { X = 5, Y = 5},
                    HoldingPiece = false
                }
            };
            //when
            Message response = exchangeInformationHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
        [TestMethod()]
        public void TestRedirectionInformationOnSafedExchange()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 5, y: 5, 1, Team.Red), (x: 4, y: 5, 2, Team.Red) };
            var map = new Map(players: players);
            map.SaveInformationExchange(1, 2);
            var message = new Message<ExchangeInformationResponse>()
            {
                AgentId = 2,
                MessagePayload = new ExchangeInformationResponse()
                {
                    RespondToID = 1
                }
            };
            var exchangeInformationHandler = new ExchangeInformationResponseHandler();
            Message<ExchangeInformationGMResponse> expectedResult = new Message<ExchangeInformationGMResponse>()
            {
                AgentId = 1,
                MessagePayload = new ExchangeInformationGMResponse()
                {
                    RespondToID = 1
                }
            };
            //when
            Message response = exchangeInformationHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
        [TestMethod()]
        public void TestRedirectionInformationOnNotSafedExchange()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 5, y: 5, 1, Team.Red), (x: 4, y: 5, 2, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<ExchangeInformationResponse>()
            {
                AgentId = 2,
                MessagePayload = new ExchangeInformationResponse()
                {
                    RespondToID = 1
                }
            };
            var exchangeInformationHandler = new ExchangeInformationResponseHandler();
            Message<NotDefinedError> expectedResult = new Message<NotDefinedError>()
            {
                AgentId = 2,
                MessagePayload = new NotDefinedError()
                {
                    Position = new Position() { X = 4, Y = 5 },
                    HoldingPiece = false
                }
            };
            //when
            Message response = exchangeInformationHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
    }
}