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

namespace GameMasterTests.MessageHandlers
{
    [TestClass]
    public class PickPieceTests
    {
        PickPieceRequestHandler handler = new PickPieceRequestHandler();
        GMConfiguration config = new GMConfiguration()
        {
            BoardX = 40,
            BoardY = 40,
        };

        [TestMethod]
        public void TestPickPieceShouldPick()
        {
            //given
            var agentId = 2;
            var positionX = 10;
            var positionY = 10;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);
            map.Players[agentId].Holding = null;

            map[positionX, positionY].PutGeneratedPiece(new Piece());
            map.Players[agentId].Position = map[positionX, positionY];

            var message = new Message<PickPieceRequest>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceRequest()
            };
            Message<PickPieceResponse> expectedResult = new Message<PickPieceResponse>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceResponse()
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestPickPieceNothingThere()
        {
            //given
            var agentId = 2;
            var positionX = 10;
            var positionY = 10;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);
            map.Players[agentId].Holding = null;
            map.Players[agentId].Position = map[positionX, positionY];

            var message = new Message<PickPieceRequest>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceRequest()
            };
            Message<PickPieceError> expectedResult = new Message<PickPieceError>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceError()
                {
                    ErrorSubtype = "NothingThere"
                }
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestPickPieceAlreadyHasPiece()
        {
            //given
            var agentId = 2;
            var positionX = 10;
            var positionY = 10;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);
            map.Players[agentId].Holding = new Piece();
            map.Players[agentId].Position = map[positionX, positionY];

            var message = new Message<PickPieceRequest>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceRequest()
            };
            Message<PickPieceError> expectedResult = new Message<PickPieceError>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceError()
                {
                    ErrorSubtype = "Other"
                }
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestPickPieceInGoalArea()
        {
            //given
            var agentId = 2;
            var positionX = 5;
            var positionY = 5 ;
            config.GoalAreaHight = 10;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);
            map.Players[agentId].Holding = null;
            map[positionX, positionY].PutGeneratedPiece(new Piece());
            map.Players[agentId].Position = map[positionX, positionY];

            var message = new Message<PickPieceRequest>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceRequest()
            };
            Message<PickPieceError> expectedResult = new Message<PickPieceError>()
            {
                AgentId = agentId,
                MessagePayload = new PickPieceError()
                {
                    ErrorSubtype = "Other"
                }
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }
    }
}
