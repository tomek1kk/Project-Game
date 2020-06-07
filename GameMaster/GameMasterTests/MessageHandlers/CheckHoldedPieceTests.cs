using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using CommunicationLibrary.Error;
using CommunicationLibrary;
using CommunicationLibrary.Model;
using GameMaster.Game;
using GameMaster.MessageHandlers;
using GameMaster.Configuration;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using FluentAssertions;

namespace GameMasterTests.MessageHandlers.Tests
{
    [TestClass()]
    public class CheckHoldedPieceTests
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
        public void PlayerDoNotHavePiece()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x:0, y:0, id:1, team:Team.Red) };
            var map = new Map(players: players);
            var message = new Message<CheckHoldedPieceRequest>()
            {
                AgentId = 1
            };
            var player = map.GetPlayerById(1);
            player.Holding = null;
            var moveHandler = new CheckHoldedPieceRequestHandler();
            Message<NotDefinedError> expectedResult = new Message<NotDefinedError>
            {
                AgentId = 1,
                MessagePayload = new NotDefinedError()
                {
                    Position = new Position()
                    {
                        X = player.Position.X,
                        Y = player.Position.Y
                    },
                    HoldingPiece = false
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);
            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
        [TestMethod()]
        public void PlayerHaveShamPiece()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 0, y: 0, id: 1, team: Team.Red) };
            var map = new Map(players: players);
            var message = new Message<CheckHoldedPieceRequest>()
            {
                AgentId = 1
            };
            var player = map.GetPlayerById(1);
            player.Holding = new ShamPiece();
            var moveHandler = new CheckHoldedPieceRequestHandler();
            Message<CheckHoldedPieceResponse> expectedResult = new Message<CheckHoldedPieceResponse>
            {
                AgentId = 1,
                MessagePayload = new CheckHoldedPieceResponse()
                {
                    Sham = true
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);
            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
        [TestMethod()]
        public void PlayerHaveRealPiece()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (x: 0, y: 0, id: 1, team: Team.Red) };
            var map = new Map(players: players);
            var message = new Message<CheckHoldedPieceRequest>()
            {
                AgentId = 1
            };
            var player = map.GetPlayerById(1);
            player.Holding = new Piece();
            var moveHandler = new CheckHoldedPieceRequestHandler();
            Message<CheckHoldedPieceResponse> expectedResult = new Message<CheckHoldedPieceResponse>
            {
                AgentId = 1,
                MessagePayload = new CheckHoldedPieceResponse()
                {
                    Sham = false
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);
            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
    }
}
