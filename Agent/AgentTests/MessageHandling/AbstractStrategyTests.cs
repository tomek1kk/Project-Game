using Agent.Strategies;
using CommunicationLibrary;
using CommunicationLibrary.Model;
using CommunicationLibrary.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using Agent;

namespace AgentTests.MessageHandling
{
    [TestClass]
    public class AbstractStrategyTests
    {
        
        int width = 30;
        int height = 40;
        string teamId = "Red";
        int goalAreaSize = 10;
        Point position = new Point(4, 6);

        private class MyAbstractStrategy : Strategy
        {
            public MyAbstractStrategy(int width, int height, string teamId, int goalAreaSize) : base(width, height, teamId, goalAreaSize) { }

            public override Message MakeDecision(AgentInfo agent)
            {
                return new Message<JoinGameResponse>();
            }
        }

        [TestMethod]
        public void TestDiscoveryStrategyAction()
        {
            //given
            Strategy strategy = new MyAbstractStrategy(width, height, teamId, goalAreaSize);
            DiscoveryResponse discoveryResponse = new DiscoveryResponse()
            {
                DistanceNW = 1,
                DistanceN = 2,
                DistanceNE = 3,
                DistanceW = 4,
                DistanceFromCurrent = 5,
                DistanceE = 6,
                DistanceSW = 7,
                DistanceS = 8,
                DistanceSE = 9
            };
            //when
            strategy.UpdateMap(new Message<DiscoveryResponse>(discoveryResponse), position);
            //then
            Assert.AreEqual(1, strategy.Board.Board[position.X - 1, position.Y + 1].DistToPiece);
            Assert.AreEqual(2, strategy.Board.Board[position.X, position.Y + 1].DistToPiece);
            Assert.AreEqual(3, strategy.Board.Board[position.X + 1, position.Y + 1].DistToPiece);
            Assert.AreEqual(4, strategy.Board.Board[position.X - 1, position.Y].DistToPiece);
            Assert.AreEqual(5, strategy.Board.Board[position.X, position.Y].DistToPiece);
            Assert.AreEqual(6, strategy.Board.Board[position.X + 1, position.Y].DistToPiece);
            Assert.AreEqual(7, strategy.Board.Board[position.X - 1, position.Y - 1].DistToPiece);
            Assert.AreEqual(8, strategy.Board.Board[position.X, position.Y - 1].DistToPiece);
            Assert.AreEqual(9, strategy.Board.Board[position.X + 1, position.Y - 1].DistToPiece);
        }
        [TestMethod]
        public void TestMoveStrategyAction()
        {
            //given
            Strategy strategy = new MyAbstractStrategy(width, height, teamId, goalAreaSize);
            MoveResponse moveResponse = new MoveResponse()
            {
                ClosestPiece = 4,
                CurrentPosition = new Position() { X = position.X, Y = position.Y },
                MadeMove = true
            };
            //when
            strategy.UpdateMap(new Message<MoveResponse>(moveResponse), position);
            //then
            Assert.AreEqual(moveResponse.ClosestPiece, strategy.Board.Board[position.X, position.Y].DistToPiece);
        }
        [TestMethod]
        public void TestPutPieceWithNormalOnGoalFieldStrategyAction()
        {
            //given
            Strategy strategy = new MyAbstractStrategy(width, height, teamId, goalAreaSize);
            PutPieceResponse putPieceResponse = new PutPieceResponse() { PutResult = PutResultEnum.NormalOnGoalField };
            //when
            strategy.UpdateMap(new Message<PutPieceResponse>(putPieceResponse), position);
            //then
            Assert.AreEqual(Agent.Board.GoalInfo.DiscoveredGoal, strategy.Board.Board[position.X, position.Y].goalInfo);
        }
        [TestMethod]
        public void TestPutPieceNormalOnNonGoalFieldStrategyAction()
        {
            //given
            Strategy strategy = new MyAbstractStrategy(width, height, teamId, goalAreaSize);
            PutPieceResponse putPieceResponse = new PutPieceResponse() { PutResult = PutResultEnum.NormalOnNonGoalField };
            //when
            strategy.UpdateMap(new Message<PutPieceResponse>(putPieceResponse), position);
            //then
            Assert.AreEqual(Agent.Board.GoalInfo.DiscoveredNotGoal, strategy.Board.Board[position.X, position.Y].goalInfo);
        }
        [TestMethod]
        public void TestPutPieceShamOnGoalAreaFieldStrategyAction()
        {
            //given
            Strategy strategy = new MyAbstractStrategy(width, height, teamId, goalAreaSize);
            PutPieceResponse putPieceResponse = new PutPieceResponse() { PutResult = PutResultEnum.ShamOnGoalArea };
            //when
            strategy.UpdateMap(new Message<PutPieceResponse>(putPieceResponse), position);
            //then
            Assert.AreEqual(Agent.Board.GoalInfo.IDK, strategy.Board.Board[position.X, position.Y].goalInfo);
        }
        [TestMethod]
        public void TestPutPieceTaskFieldStrategyAction()
        {
            //given
            Strategy strategy = new MyAbstractStrategy(width, height, teamId, goalAreaSize);
            PutPieceResponse putPieceResponse = new PutPieceResponse() { PutResult = PutResultEnum.TaskField };
            //when
            strategy.UpdateMap(new Message<PutPieceResponse>(putPieceResponse), position);
            //then
            Assert.AreEqual(Agent.Board.GoalInfo.IDK, strategy.Board.Board[position.X, position.Y].goalInfo);
        }
    }
}
