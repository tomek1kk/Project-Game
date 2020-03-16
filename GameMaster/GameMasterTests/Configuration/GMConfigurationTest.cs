using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using GameMaster.Configuration;

namespace GameMasterTests.Configuration.Tests
{
    [TestClass()]
    public class GMConfigurationTest
    {
        [TestMethod()]
        public void CreateConfigurationWithGivenPath()
        {
            //Given
            string configuration = 
                "{\"boardX\": 40," +
                " \"boardY\": 40," +
                "\"CsIP\": \"127.0.0.1\"," +
                "\"CsPort\": 8080," +
                "\"teamID\": 3," +
                "\"movePenalty\": 1500," +
                "\"askPenalty\": 1000," +
                "\"discoveryPenalty\": 700," +
                "\"putPenalty\": 500," +
                "\"checkForShamPenalty\": 700," +
                "\"responsePenalty\": 1000," +
                "\"goalAreaHight\": 5," +
                "\"numberOfGoals\": 5," +
                "\"numberOfPieces\": 10}";
            string pathToTMPFile = "testConfig";
            File.WriteAllText(pathToTMPFile, configuration);
            //When
            GMConfiguration gmConfig = GMConfiguration.ReadConfiguration(new string[] { "./"+pathToTMPFile });
            //Then
            Assert.AreEqual(40, gmConfig.BoardX);
            Assert.AreEqual(40, gmConfig.BoardY);
            Assert.AreEqual("127.0.0.1", gmConfig.CsIP);
            Assert.AreEqual(8080, gmConfig.CsPort);
            Assert.AreEqual(3, gmConfig.TeamID);
            Assert.AreEqual(1500, gmConfig.MovePenalty);
            Assert.AreEqual(1000, gmConfig.AskPenalty);
            Assert.AreEqual(700, gmConfig.DiscoveryPenalty);
            Assert.AreEqual(500, gmConfig.PutPenalty);
            Assert.AreEqual(700, gmConfig.CheckForShamPenalty);
            Assert.AreEqual(5, gmConfig.GoalAreaHight);
            Assert.AreEqual(5, gmConfig.NumberOfGoals);
            Assert.AreEqual(10, gmConfig.NumberOfPieces);
            File.Delete(pathToTMPFile);
        }

    }
}
