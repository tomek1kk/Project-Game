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
        [TestMethod()]
        [ExpectedException(typeof(InvalidOperationException))]
        public void TestGuiCantBeStartedTwoTimes()
        {
            //given
            GuiMantainer guiMantainer = new GuiMantainer();
            var mock = new Mock<IGuiDataProvider>();
            mock.Setup(guiDataProvider => guiDataProvider.GetCurrentBoardModel())
                .Returns(new BoardModel
                {
                    Width = 5,
                    Height = 5,
                    GoalAreaHeight = 1,
                    Fields = new FieldType[5,5]
                });


            //when
            guiMantainer.StartGui(mock.Object);
            guiMantainer.StartGui(mock.Object);

            //then
            //InvalidOperationException caught
        }
    }
}