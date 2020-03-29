using Microsoft.VisualStudio.TestTools.UnitTesting;
using CommunicationLibrary.Request;
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

namespace GameMasterTests.MessageHandlers.Tests
{
    [TestClass()]
    public class MoveTests
    {
        [TestMethod()]
        public void TestMoveHandlerOutsideN()
        {
            //given
            var players = new List<(int x, int y, int id, Team team)>() { (0, 0, 1, Team.Red) };
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
            //GMConfiguration config = GMConfiguration.ReadConfiguration(new string[0]);
            GMConfiguration config = new GMConfiguration()
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
            Message<MoveError> expectedResult = new Message<MoveError>
            {
                AgentId = 1,
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 0,
                        Y = 0
                    }
                }
            };
            //when
            Message response = moveHandler.ProcessRequest(map, message, config);

            //then
            Assert.IsInstanceOfType(response, expectedResult.GetType());
            Assert.AreEqual(expectedResult.MessagePayload.Position.X, ((MoveError)response.GetPayload()).Position.X);
            Assert.AreEqual(expectedResult.MessagePayload.Position.Y, ((MoveError)response.GetPayload()).Position.Y);
        }
    }
}


