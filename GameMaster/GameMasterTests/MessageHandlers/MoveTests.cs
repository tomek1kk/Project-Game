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
    public class MoveTests
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
        public void TestMoveHandlerOutsideN()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 9, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 0,
                        Y = 9
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 9 });
        }
        [TestMethod()]
        public void TestMoveHandlerInsideN()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 8, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveResponse> expectedResult = new Message<MoveResponse>
            {
                AgentId = 1,
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = new Position() { X = 0, Y = 9 },
                    ClosestPiece = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 9 });
        }
        [TestMethod()]
        public void TestMoveHandlerOutsideS()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (9, 0, 1, Team.Blue) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "S"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 9,
                        Y = 0
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 9, Y = 0 });
        }
        [TestMethod()]
        public void TestMoveHandlerInsideS()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (9, 1, 1, Team.Blue) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "S"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveResponse> expectedResult = new Message<MoveResponse>
            {
                AgentId = 1,
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = new Position() { X = 9, Y = 0 },
                    ClosestPiece = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 9, Y = 0 });
        }
        [TestMethod()]
        public void TestMoveHandlerOutsideE()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (9, 0, 1, Team.Blue) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "E"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 9,
                        Y = 0
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 9, Y = 0 });
        }
        [TestMethod()]
        public void TestMoveHandlerInsideE()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (8, 5, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "E"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveResponse> expectedResult = new Message<MoveResponse>
            {
                AgentId = 1,
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = new Position() { X = 9, Y = 5 },
                    ClosestPiece = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 9, Y = 5 });
        }
        [TestMethod()]
        public void TestMoveHandlerOutsideW()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 5, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "W"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 0,
                        Y = 5
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 5 });
        }
        [TestMethod()]
        public void TestMoveHandlerInsideW()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (1, 5, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "W"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveResponse> expectedResult = new Message<MoveResponse>
            {
                AgentId = 1,
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = new Position() { X = 0, Y = 5 },
                    ClosestPiece = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 5 });
        }
        [TestMethod()]
        public void TestMoveBluePlayerIntoRedArea()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 6, 1, Team.Blue) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 0,
                        Y = 6
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 6 });
        }
        [TestMethod()]
        public void TestMoveRedPlayerIntoRedArea()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 6, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveResponse> expectedResult = new Message<MoveResponse>
            {
                AgentId = 1,
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = new Position() { X = 0, Y = 7 },
                    ClosestPiece = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 7 });
        }
        [TestMethod()]
        public void TestMoveRedPlayerIntoBlueArea()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 3, 1, Team.Red) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "S"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 0,
                        Y = 3
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 3 });
        }
        [TestMethod()]
        public void TestMoveBluePlayerIntoBlueArea()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 3, 1, Team.Blue) };
            var map = new Map(players: players);
            var message = new Message<MoveRequest>()
            {
                AgentId = 1,
                MessagePayload = new MoveRequest()
                {
                    Direction = "S"
                }
            };
            var moveHandler = new MoveRequestHandler();
            Message<MoveResponse> expectedResult = new Message<MoveResponse>
            {
                AgentId = 1,
                MessagePayload = new MoveResponse()
                {
                    MadeMove = true,
                    CurrentPosition = new Position() { X = 0, Y = 2 },
                    ClosestPiece = int.MaxValue
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
            map.GetPlayerById(1).Position.Should().BeEquivalentTo(new Position() { X = 0, Y = 2 });
        }
    }
}


