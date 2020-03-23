using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Agent;
using Agent.Strategies;
using Moq;
using CommunicationLibrary.Model;
using CommunicationLibrary.Information;
using CommunicationLibrary;
using CommunicationLibrary.Response;

namespace AgentTests.AgentInfoTests
{
    [TestClass]
    public class AgentInfoTests
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

        [TestMethod]
        public void TestUpdateAgentPosition()
        {
            //given
            var strategy = new Mock<IStrategy>();
            AgentInfo agentInfo = new AgentInfo(strategy.Object, defaultGameStartedMessage);
            MoveResponse moveResponse = new MoveResponse() { CurrentPosition = new Position() { X = 3, Y = 4 } };
            //when
            Message message = new Message<MoveResponse>(moveResponse);
            agentInfo.UpdateFromMessage(message);
            //then
            Assert.AreEqual(moveResponse.CurrentPosition.X.Value, agentInfo.Position.X);
            Assert.AreEqual(moveResponse.CurrentPosition.Y.Value, agentInfo.Position.Y);
        }
        [TestMethod]
        public void TestCheckHoldedPieceIfSham()
        {
            //given
            var strategy = new Mock<IStrategy>();
            AgentInfo agentInfo = new AgentInfo(strategy.Object, defaultGameStartedMessage);
            CheckHoldedPieceResponse checkHoldedPiece = new CheckHoldedPieceResponse() { Sham = true };
            PickPieceResponse pickPiece = new PickPieceResponse();
            //when
            Message pickUp = new Message<PickPieceResponse>(pickPiece);
            agentInfo.UpdateFromMessage(pickUp);
            Message checkUp = new Message<CheckHoldedPieceResponse>(checkHoldedPiece);
            agentInfo.UpdateFromMessage(checkUp);
            //then
            Assert.AreEqual(false, agentInfo.HasPiece);
        }
        [TestMethod]
        public void TestCheckHoldedPieceIfNotSham()
        {
            //given
            var strategy = new Mock<IStrategy>();
            AgentInfo agentInfo = new AgentInfo(strategy.Object, defaultGameStartedMessage);
            CheckHoldedPieceResponse checkHoldedPiece = new CheckHoldedPieceResponse() { Sham = false };
            PickPieceResponse pickPiece = new PickPieceResponse();
            //when
            Message pickUp = new Message<PickPieceResponse>(pickPiece);
            agentInfo.UpdateFromMessage(pickUp);
            Message checkUp = new Message<CheckHoldedPieceResponse>(checkHoldedPiece);
            agentInfo.UpdateFromMessage(checkUp);
            //then
            Assert.AreEqual(true, agentInfo.HasPiece);
        }
        [TestMethod]
        public void TestPutPieceSetsHasPieceToFalse()
        {
            //given
            var strategy = new Mock<IStrategy>();
            AgentInfo agentInfo = new AgentInfo(strategy.Object, defaultGameStartedMessage);
            PutPieceResponse putPiece = new PutPieceResponse();
            PickPieceResponse pickPiece = new PickPieceResponse();
            //when
            Message pickUp = new Message<PickPieceResponse>(pickPiece);
            agentInfo.UpdateFromMessage(pickUp);
            Message checkUp = new Message<PutPieceResponse>(putPiece);
            agentInfo.UpdateFromMessage(checkUp);
            //then
            Assert.AreEqual(false, agentInfo.HasPiece);
        }
    }
}
