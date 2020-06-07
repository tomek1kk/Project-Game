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
    public class DiscoveryTests
    {
        private GMConfiguration config = new GMConfiguration()
        {
            BoardX = 40,
            BoardY = 40,
            CsIP = "127.0.0.1",
            CsPort = 8080,
            MovePenalty = 1500,
            DiscoveryPenalty = 700,
            PutPenalty = 500,
            CheckForShamPenalty = 700,
            InformationExchangePenalty = 1000,
            GoalAreaHeight = 5,
            NumberOfGoals = 5,
            NumberOfPieces = 10,
            ShamPieceProbability = 20
        };
        [TestMethod()]
        public void TestPlayerStandsOnPiece()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 5, y: 5, 1, Team.Red) };
            var pieces = new List<(int x, int y)>() { (x: 5, y: 5) };
            var map = new Map(players: players, realPieces: pieces);
            var message = new Message<DiscoveryRequest>()
            {
                AgentId = 1,
                MessagePayload = new DiscoveryRequest(){}
            };
            var moveHandler = new DiscoveryRequestHandler();
            Message<DiscoveryResponse> expectedResult = new Message<DiscoveryResponse>
            {
                AgentId = 1,
                MessagePayload = new DiscoveryResponse()
                {
                    DistanceFromCurrent = 0,
                    DistanceE = 1,
                    DistanceN = 1,
                    DistanceW = 1,
                    DistanceS = 1,
                    DistanceNE = 2,
                    DistanceNW = 2,
                    DistanceSE = 2,
                    DistanceSW = 2
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
        [TestMethod()]
        public void TestPlayerOnEdgeOfMap()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 0, y: 5, 1, Team.Red) };
            var pieces = new List<(int x, int y)>() { (x: 0, y: 5) };
            var map = new Map(players: players, realPieces: pieces);
            var message = new Message<DiscoveryRequest>()
            {
                AgentId = 1,
                MessagePayload = new DiscoveryRequest() { }
            };
            var moveHandler = new DiscoveryRequestHandler();
            Message<DiscoveryResponse> expectedResult = new Message<DiscoveryResponse>
            {
                AgentId = 1,
                MessagePayload = new DiscoveryResponse()
                {
                    DistanceFromCurrent = 0,
                    DistanceE = 1,
                    DistanceN = 1,
                    DistanceW = int.MaxValue,
                    DistanceS = 1,
                    DistanceNE = 2,
                    DistanceNW = int.MaxValue,
                    DistanceSE = 2,
                    DistanceSW = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
    }
}
