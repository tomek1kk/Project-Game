using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.GUI.Tests
{
    [TestClass()]
    public class CallbackGuiActionsExcecutorTests
    {
        [TestMethod()]
        public void TestCallsCallbackOnGameStart()
        {
            //given
            bool gameStartCalled = false;
            var executor = new CallbackGuiActionsExcecutor(() => gameStartCalled = true);

            //when
            executor.StartGame();

            //then
            Assert.IsTrue(gameStartCalled);
        }
    }
}