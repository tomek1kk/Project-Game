using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;

namespace GameMaster.GUI.Tests
{
    [TestClass()]
    public class GuiMantainerTests
    {
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGuiCantBeStoppedIfNotStarted()
        {
            //given
            GuiMantainer guiMantainer = new GuiMantainer();

            //when
            guiMantainer.StopGui();

            //then
            //InvalidOperationException caught
        }
    }
}