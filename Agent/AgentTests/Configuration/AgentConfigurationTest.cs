using Agent;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace AgentTests.Configuration
{
    [TestClass]
    public class AgentConfigurationTest
    {
        [TestMethod]
        public void ReadDefaultConfiguration()
        {
            //Given
            File.WriteAllText("TMPpath.txt",
               "{\"CsIP\": \"127.0.0.1\"," +
               "\"CsPort\": 8080," +
               "\"teamID\": \"red\"," +
               "\"strategy\": 1}");
            string[] args = new string[1] { "./TMPpath.txt" };

            //When
            AgentConfiguration config = AgentConfiguration.ReadConfiguration(args);
            //Then
            Assert.AreEqual("127.0.0.1", config.CsIp);
            Assert.AreEqual(8080, config.CsPort);
            Assert.AreEqual("red", config.TeamId);
            Assert.AreEqual(1, config.Strategy);
            //After
            File.Delete("TMPpath.txt");
        }
    }
}
