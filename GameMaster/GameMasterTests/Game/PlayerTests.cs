using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;

namespace GameMaster.Game.Tests
{
    [TestClass()]
    public class PlayerTests
    {
        [TestMethod()]
        public void TestProperPlayerLock()
        {
            //given
            var player = new Player(new DateTime(2010, 4, 10, 9, 0, 0));

            //when
            var returnedValue = player.TryLock(DateTime.Now);

            //then
            Assert.IsTrue(returnedValue);
        }
        [TestMethod()]
        public void TestLockPlayerBeforeHisLockTimeExpire()
        {
            //given
            var player = new Player(DateTime.Now.AddSeconds(2) );

            //when
            var returnedValue = player.TryLock(DateTime.Now);

            //then
            Assert.IsTrue(!returnedValue);
        }
    }
}