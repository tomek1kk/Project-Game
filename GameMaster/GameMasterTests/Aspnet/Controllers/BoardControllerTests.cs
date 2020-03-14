using Microsoft.VisualStudio.TestTools.UnitTesting;
using GameMaster.Aspnet.Controllers;
using System;
using System.Collections.Generic;
using System.Text;
using Moq;
using GameMaster.GUI;

namespace GameMaster.Aspnet.Controllers.Tests
{
    [TestClass()]
    public class BoardControllerTests
    {
        [TestMethod()]
        public void TestGetBoardModelReturnsBoardThatGuiDataProviderProvides()
        {
            //given
            var mock = new Mock<IGuiDataProvider>();
            var providedBoardModel = new BoardModel
            {
                Width = 3,
                Height = 3,
                GoalAreaHeight = 1,
                Fields = new FieldType[3, 3]
            };
            providedBoardModel.Fields[1, 1] = FieldType.DiscoveredGoal;
            mock.Setup(guiDataProvider => guiDataProvider.GetCurrentBoardModel())
                .Returns(providedBoardModel);
            var boardController = new BoardController(mock.Object);

            //when
            var resultBoardModel = boardController.GetBoardModel();

            //then
            Assert.AreEqual(providedBoardModel.Height, resultBoardModel.Height);
            Assert.AreEqual(providedBoardModel.Width, resultBoardModel.Width);
            Assert.AreEqual(providedBoardModel.GoalAreaHeight, resultBoardModel.GoalAreaHeight);
            CollectionAssert.AreEqual(providedBoardModel.Fields, resultBoardModel.Fields);

        }
    }
}