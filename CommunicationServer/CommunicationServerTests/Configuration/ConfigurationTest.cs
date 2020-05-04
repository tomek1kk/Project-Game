using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using CommunicationServerNamespace;

namespace CommunicationServerTests.ConfigurationTests
{
    [TestClass]
    public class ConfigurationTest
    {
        [TestMethod()]
        public void CreateConfigurationWithGivenPath()
        {
            //Given
            string configuration =
                "{\"CsIP\": \"127.0.0.1\"," +
                "\"GMPort\": 8081," +
                "\"AgentPort\": 8080}";
            string pathToTMPFile = "testConfig";
            File.WriteAllText(pathToTMPFile, configuration);
            //When
            Configuration config = Configuration.ReadConfiguration(new string[] { "./" + pathToTMPFile });
            //Then
            Assert.AreEqual("127.0.0.1", config.CsIP);
            Assert.AreEqual(8081, config.GMPort);
            Assert.AreEqual(8080, config.AgentPort);
            File.Delete(pathToTMPFile);
        }

    }
}
