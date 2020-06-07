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

namespace GameMasterTests.Game.Tests
{
    [TestClass()]
    public class MapTests
    {
        [TestMethod()]
        public void TestRandomGenerator()
        {
            //given
            Random rand = new Random(10);
            int randomCount = 5;
            int rangeFrom = 0;
            int rangeTo = 199;
            int shuffleCount = 200;
            List<int> properResult = new List<int>() { 155, 29, 10, 9, 71 };

            //when
            var returnedList = Map.TakeRandomsFromRange(randomCount, rangeFrom, rangeTo, rand, shuffleCount);

            //then
            Assert.IsNotNull(returnedList);
            for (int i = 0; i < randomCount; i++)
               Assert.AreEqual(properResult[i], returnedList[i]);
        }
        [TestMethod()]
        public void TestClosestPieceForFieldMethodForOnePiece()
        {
            //given
            var pieces = new List<(int x, int y)>() { (x: 5, y: 5) };
            var map = new Map(realPieces: pieces);
            //when
            //then
            Assert.AreEqual(0, map.ClosestPieceForField(map[5, 5]));
            Assert.AreEqual(1, map.ClosestPieceForField(map[6, 5]));
            Assert.AreEqual(1, map.ClosestPieceForField(map[5, 6]));
            Assert.AreEqual(6, map.ClosestPieceForField(map[8, 8]));
            Assert.AreEqual(10, map.ClosestPieceForField(map[0, 0]));
        }
        [TestMethod()]
        public void TestClosestPieceForFieldMethodForMultiplePieces()
        {
            //given
            var pieces = new List<(int x, int y)>() { (x: 5, y: 5), (x: 2, y: 5), (x: 7, y: 5), (x: 7, y: 3) };
            var map = new Map(realPieces: pieces);
            //when
            //then1
            Assert.AreEqual(0, map.ClosestPieceForField(map[5, 5]));
            Assert.AreEqual(0, map.ClosestPieceForField(map[2, 5]));
            Assert.AreEqual(0, map.ClosestPieceForField(map[7, 5]));
            Assert.AreEqual(0, map.ClosestPieceForField(map[7, 3]));
            Assert.AreEqual(1, map.ClosestPieceForField(map[3, 5]));
            Assert.AreEqual(3, map.ClosestPieceForField(map[0, 6]));
            Assert.AreEqual(4, map.ClosestPieceForField(map[0, 3]));
            Assert.AreEqual(1, map.ClosestPieceForField(map[5, 4]));
            Assert.AreEqual(2, map.ClosestPieceForField(map[6, 4]));
            Assert.AreEqual(3, map.ClosestPieceForField(map[9, 4]));
        }
        [TestMethod()]
        public void TestAddPieceMethod()
        {
            //given
            var map = new Map();
            //when
            map.AddPiece();
            //then
            int count = 0;
            for (int x = 0; x <= 9; x++)
                for (int y = 3; y < 7; y++)
                    count += map[x, y].ContainsPieces() ? 1 : 0;
            Assert.AreEqual(1, count);
        }
        [TestMethod()]
        public void TestAddPieceWithArgumentMethod()
        {
            //given
            var map = new Map();
            var rand = new Random();
            //when
            map.AddPiece(rand);
            //then
            int count = 0;
            for (int x = 0; x <= 9; x++)
                for (int y = 3; y < 7; y++)
                    count += map[x, y].ContainsPieces() ? 1 : 0;
            Assert.AreEqual(1, count);
        }
        [TestMethod()]
        public void TestIsInsideMapMethod()
        {
            //given
            var map = new Map();
            //when
            //then
            Assert.AreEqual(true, map.IsInsideMap(5, 5));
            Assert.AreEqual(true, map.IsInsideMap(0, 0));
            Assert.AreEqual(true, map.IsInsideMap(9, 9));
            Assert.AreEqual(false, map.IsInsideMap(-1, 0));
            Assert.AreEqual(false, map.IsInsideMap(0, -1));
            Assert.AreEqual(false, map.IsInsideMap(5, 10));
            Assert.AreEqual(false, map.IsInsideMap(10, 5));
            Assert.AreEqual(false, map.IsInsideMap(10, 10));
        }
        [TestMethod()]
        public void TestIsInsideRedGoalAreaMethod()
        {
            //given
            var map = new Map();
            //when
            //then
            Assert.AreEqual(true, map.IsInsideRedGoalArea(8, 8));
            Assert.AreEqual(true, map.IsInsideRedGoalArea(9, 9));
            Assert.AreEqual(true, map.IsInsideRedGoalArea(0, 8));
            Assert.AreEqual(true, map.IsInsideRedGoalArea(5, 7));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(10, 3));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(5, 5));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(1, 1));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(5, 6));
        }
        [TestMethod()]
        public void TestIsInsideBlueGoalAreaMethod()
        {
            //given
            var map = new Map();
            //when
            //then
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(2, 2));
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(9, 0));
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(0, 2));
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(5, 0));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(10, 3));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(5, 5));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(8, 8));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(5, 6));
        }
        [TestMethod()]
        public void TestIsInsideRedGoalAreaPositionArgumentMethod()
        {
            //given
            var map = new Map();
            //when
            //then
            Assert.AreEqual(true, map.IsInsideRedGoalArea(map[8, 8]));
            Assert.AreEqual(true, map.IsInsideRedGoalArea(map[9, 9]));
            Assert.AreEqual(true, map.IsInsideRedGoalArea(map[0, 8]));
            Assert.AreEqual(true, map.IsInsideRedGoalArea(map[5, 7]));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(map[5, 5]));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(map[1, 1]));
            Assert.AreEqual(false, map.IsInsideRedGoalArea(map[5, 6]));
        }
        [TestMethod()]
        public void TestIsInsideBlueGoalAreaPositionArgumentWithMethod()
        {
            //given
            var map = new Map();
            //when
            //then
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(map[2, 2]));
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(map[9, 0]));
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(map[0, 2]));
            Assert.AreEqual(true, map.IsInsideBlueGoalArea(map[5, 0]));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(map[5, 5]));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(map[8, 8]));
            Assert.AreEqual(false, map.IsInsideBlueGoalArea(map[5, 6]));
        }
        [TestMethod()]
        public void TestAddingMaxPlayers()
        {
            //given
            int playersInTeamCount = 20;
            int playersCount = playersInTeamCount * 2;
            bool[] results = new bool[playersCount];
            bool[,] virtualPlayersMap = new bool[10, 10];
            var map = new Map(teamSize: playersInTeamCount);
            //when
            for (int i = 0; i < playersCount; i++)
                results[i] = map.AddPlayer(i % 2 == 0 ? Team.Red : Team.Blue, i);
            //then
            for (int i = 0; i < playersCount; i++)
            {
                Assert.AreEqual(true, results[i]);
                Assert.IsTrue(map.GetPlayerById(i).X >= 0 && map.GetPlayerById(i).X < 10);
                Assert.IsTrue(map.GetPlayerById(i).Y >= 2 && map.GetPlayerById(i).Y < 7);
                Assert.IsFalse(virtualPlayersMap[map.GetPlayerById(i).X, map.GetPlayerById(i).Y]);
                virtualPlayersMap[map.GetPlayerById(i).X, map.GetPlayerById(i).Y] = true;
            }
        }
        [TestMethod()]
        public void TestAddingTooManyPlayers()
        {
            //given
            int playersInTeamCount = 21;
            int playersCount = playersInTeamCount*2;
            List<bool> results = new List<bool>();
            bool[,] virtualPlayersMap = new bool[10, 10];
            var map = new Map(teamSize: playersInTeamCount);
            //when
            for (int i = 0; i < playersCount; i++)
                results.Add(map.AddPlayer(Team.Blue, i));
            //then
            Assert.IsTrue(results.Contains(false));
            for (int i = 0; i < playersCount; i++)
            {
                if (!results[i])
                    continue;
                Assert.IsTrue(map.GetPlayerById(i).X >= 0 && map.GetPlayerById(i).X < 10);
                Assert.IsTrue(map.GetPlayerById(i).Y >= 2 && map.GetPlayerById(i).Y < 7);
                Assert.IsFalse(virtualPlayersMap[map.GetPlayerById(i).X, map.GetPlayerById(i).Y]);
                virtualPlayersMap[map.GetPlayerById(i).X, map.GetPlayerById(i).Y] = true;
            }
        }
    }
}
