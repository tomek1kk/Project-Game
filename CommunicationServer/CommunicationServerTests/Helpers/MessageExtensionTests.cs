using CommunicationLibrary;
using CommunicationLibrary.Information;
using CommunicationLibrary.Response;
using CommunicationServerNamespace.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationServerTests.Helpers
{
    [TestClass]
    public class MessageExtensionTests
    {
        [TestMethod]
        public void TestIsEndGameMethod()
        {
            //given
            Message messageGameEnded= new Message<GameEnded>(new GameEnded());
            Message messageJoinGame = new Message<JoinGameResponse>(new JoinGameResponse());
            //when
            bool resultMessegeGameEnded = messageGameEnded.IsEndGame();
            bool resultMessageJoinGame = messageJoinGame.IsEndGame();
            //then
            Assert.AreEqual(false, resultMessageJoinGame);
            Assert.AreEqual(true, resultMessegeGameEnded);
        }
        [TestMethod]
        public void TestIsStartGameMethod()
        {
            //given
            Message messageGameStart= new Message<GameStarted>(new GameStarted());
            Message messageJoinGame = new Message<JoinGameResponse>(new JoinGameResponse());
            //when
            bool resultMessegeGameStart= messageGameStart.IsGameStarted();
            bool resultMessageJoinGame = messageJoinGame.IsGameStarted();
            //then
            Assert.AreEqual(false, resultMessageJoinGame);
            Assert.AreEqual(true, resultMessegeGameStart);
        }

    }
}
