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
    public class DestroyPieceTests
    {
        DestroyPieceRequestHandler handler = new DestroyPieceRequestHandler();
        GMConfiguration config = new GMConfiguration()
        {
            BoardX = 40,
            BoardY = 40,
            TeamSize = 5
        };

        [TestMethod]
        public void TestDestroyPieceHasPiece()
        {
            //given
            var agentId = 2;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);
            map.Players[agentId].Holding = new Piece();

            var message = new Message<DestroyPieceRequest>()
            {
                AgentId = agentId,
            };
            Message<DestroyPieceResponse> expectedResult = new Message<DestroyPieceResponse>()
            {
                AgentId = agentId,
                MessagePayload = new DestroyPieceResponse()
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

        [TestMethod]
        public void TestDestroyPieceNoPiece()
        {
            //given
            var agentId = 2;
            var map = new Map(config);
            map.AddPlayer(Team.Blue, agentId);
            map.Players[agentId].Holding = null;

            var message = new Message<DestroyPieceRequest>()
            {
                AgentId = agentId,
            };
            Message<NotDefinedError> expectedResult = new Message<NotDefinedError>()
            {
                AgentId = agentId,
                MessagePayload = new NotDefinedError()
                {
                    HoldingPiece = false,
                    Position = new Position()
                    {
                        X = map.Players[agentId].X,
                        Y = map.Players[agentId].Y
                    }
                }
            };

            //when
            Message response = handler.ProcessRequest(map, message, config);

            //then
            response.Should().BeEquivalentTo(expectedResult);
        }

    }
}
