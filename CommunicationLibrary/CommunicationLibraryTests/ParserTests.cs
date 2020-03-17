using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.Json;
using FluentAssertions;

namespace CommunicationLibrary.Tests
{
    [TestClass]
    public class ParserTests
    {
        IParser parser = new Parser();

        [TestMethod]
        public void TestParseCheckHoldedPieceRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":1,\"AgentId\":null}";
            var expected = new Message<CheckHoldedPieceRequest>()
            {
                MessagePayload = new CheckHoldedPieceRequest()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseDestroyPieceRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":2,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(DestroyPieceRequest));
        }
        [TestMethod]
        public void TestParseDiscoveryRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":3,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(DiscoveryRequest));
        }
        [TestMethod]
        public void TestParseExachangeInformationRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":5,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ExchangeInformationRequest));
        }
        [TestMethod]
        public void TestParseJoinGameRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":6,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JoinGameRequest));
        }
        [TestMethod]
        public void TestParseMoveRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":7,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(MoveRequest));
        }

        [TestMethod]
        public void TestParsePickPieceRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":8,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PickPieceRequest));
        }

        [TestMethod]
        public void TestParsePutPieceRequest()
        {
            // Arrange
            string jsonString = "{\"MessageId\":9,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PutPieceRequest));
        }

        [TestMethod]
        public void TestParseCheckHoldedPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":101,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(CheckHoldedPieceResponse));
        }

        [TestMethod]
        public void TestParseDestroyPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":102,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(DestroyPieceResponse));
        }

        [TestMethod]
        public void TestParseDiscoveryResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":103,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(DiscoveryResponse));
        }

        [TestMethod]
        public void TestParseExchangeInformationResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":4,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(ExchangeInformationResponse));
        }

        [TestMethod]
        public void TestParseJoinGameResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":107,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(JoinGameResponse));
        }

        [TestMethod]
        public void TestParseMoveResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":108,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(MoveResponse));
        }

        [TestMethod]
        public void TestParsePickPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":109,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PickPieceResponse));
        }

        [TestMethod]
        public void TestParsePutPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessageId\":110,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PutPieceResponse));
        }

        [TestMethod]
        public void TestParseMoveError()
        {
            // Arrange
            string jsonString = "{\"MessageId\":901,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(MoveError));
        }

        [TestMethod]
        public void TestParsePickPieceError()
        {
            // Arrange
            string jsonString = "{\"MessageId\":902,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PickPieceError));
        }

        [TestMethod]
        public void TestParsePutPieceError()
        {
            // Arrange
            string jsonString = "{\"MessageId\":903,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(PutPieceError));
        }

        [TestMethod]
        public void TestParseNotDefinedError()
        {
            // Arrange
            string jsonString = "{\"MessageId\":905,\"AgentId\":null}";

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(NotDefinedError));
        }

        [TestMethod]
        public void TestParseMessageNotFound()
        {
            // Arrange
            string jsonString = "\"MessagePayload\":{\"Direction\":\"N\"},\"MessageId\":7,\"AgentId\":null}";
            //var expected = new Message<>

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            //Assert.IsInstanceOfType(result.MessageId, typeof(NotDefinedError));
        }

        [TestMethod]
        public void TestParseMoveRequestConcrete()
        {
            // Arrange
            //string json = JsonSerializer.Serialize(expected);

            //// Act
            //var result = parser.Parse(json);

            //// Assert
            //Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestTest()
        {
            Message<MoveRequest> m = new Message<MoveRequest>()
            {
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            var x = parser.AsString<MoveRequest>(m);
            Message<CheckHoldedPieceRequest> m2 = new Message<CheckHoldedPieceRequest>()
            {
                MessagePayload = new CheckHoldedPieceRequest()
            };
            var x2 = parser.AsString<CheckHoldedPieceRequest>(m2);

        }
        
        [TestMethod]
        public void TestTest2()
        {
            string x = "{\"MessagePayload\":{\"Direction\":\"N\"},\"MessageId\":7,\"AgentId\":null}";
            var y = parser.Parse(x);

        }
    }
}
