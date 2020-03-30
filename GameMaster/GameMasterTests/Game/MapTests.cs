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
    }
}
