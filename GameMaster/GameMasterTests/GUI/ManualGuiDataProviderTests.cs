using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMaster.GUI.Tests
{
    [TestClass()]
    public class ManualGuiDataProviderTests
    {
        [TestMethod()]
        public void TestValuesSetInConstructorAffectReturnedBoard()
        {
            //given
            int width = 15;
            int height = 20;
            int goalAreaHeight = 10;
            var manualGuiDataProvider = new ManualGuiDataProvider(width, height, goalAreaHeight);

            //when
            var result = manualGuiDataProvider.GetCurrentBoardModel();

            //then
            Assert.AreEqual(width, result.Width);
            Assert.AreEqual(height, result.Height);
            Assert.AreEqual(goalAreaHeight, result.GoalAreaHeight);
        }

        [TestMethod()]
        public void TestFieldsArrayDimensionsSetBasedOnConstructorValues()
        {
            //given
            int width = 15;
            int height = 20;
            int goalAreaHeight = 10;
            var manualGuiDataProvider = new ManualGuiDataProvider(width, height, goalAreaHeight);

            //when
            var result = manualGuiDataProvider.GetCurrentBoardModel();

            //then
            Assert.AreEqual(width, result.Fields.GetLength(0));
            Assert.AreEqual(height, result.Fields.GetLength(1));
        }
        [TestMethod()]
        public void TestFieldChangesVisibleInBoardModel()
        {
            //given
            int width = 15;
            int height = 20;
            int goalAreaHeight = 10;
            var manualGuiDataProvider = new ManualGuiDataProvider(width, height, goalAreaHeight);
            manualGuiDataProvider.SetField(1, 5, FieldType.BluePlayer);

            //when
            var result = manualGuiDataProvider.GetCurrentBoardModel();

            //then
            Assert.AreEqual(FieldType.BluePlayer, result.Fields[1, 5]);
        }
    }
}