using CommunicationLibrary;
using CommunicationLibrary.Error;
using CommunicationLibrary.Request;
using CommunicationLibrary.Response;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using CommunicationLibrary.Model;
using System.Text.Json;
using FluentAssertions;
using CommunicationLibrary.Information;

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
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":2,\"AgentId\":null}";
            var expected = new Message<DestroyPieceRequest>()
            {
                MessagePayload = new DestroyPieceRequest()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseDiscoveryRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":3,\"AgentId\":null}";
            var expected = new Message<DiscoveryRequest>()
            {
                MessagePayload = new DiscoveryRequest()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseExachangeInformationRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{\"AskedAgentId\":1},\"MessageId\":5,\"AgentId\":null}";
            var expected = new Message<ExchangeInformationRequest>()
            {
                MessagePayload = new ExchangeInformationRequest()
                {
                    AskedAgentId = 1
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseJoinGameRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{\"TeamId\":\"blue\"},\"MessageId\":6,\"AgentId\":null}";
            var expected = new Message<JoinGameRequest>()
            {
                MessagePayload = new JoinGameRequest()
                {
                    TeamId = "blue"
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseMoveRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{\"Direction\":\"N\"},\"MessageId\":7,\"AgentId\":null}";
            var expected = new Message<MoveRequest>()
            {
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePickPieceRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":8,\"AgentId\":null}";
            var expected = new Message<PickPieceRequest>()
            {
                MessagePayload = new PickPieceRequest()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePutPieceRequest()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":9,\"AgentId\":null}";
            var expected = new Message<PutPieceRequest>()
            {
                MessagePayload = new PutPieceRequest()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseCheckHoldedPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{\"Sham\":true},\"MessageId\":101,\"AgentId\":null}";
            var expected = new Message<CheckHoldedPieceResponse>()
            {
                MessagePayload = new CheckHoldedPieceResponse()
                {
                    Sham = true
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseDestroyPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":102,\"AgentId\":null}";
            var expected = new Message<DestroyPieceResponse>()
            {
                MessagePayload = new DestroyPieceResponse()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseDiscoveryResponse()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{\"DistanceFromCurrent\":3,\"DistanceN\":1,\"DistanceNE\":2,\"DistanceE\":3,\"DistanceSE\":4,\"DistanceS\":5,\"DistanceSW\":6,\"DistanceW\":7,\"DistanceNW\":8},\"MessageId\":103,\"AgentId\":null}";
            var expected = new Message<DiscoveryResponse>()
            {
                MessagePayload = new DiscoveryResponse()
                {
                    DistanceFromCurrent = 3,
                    DistanceN = 1,
                    DistanceNE = 2,
                    DistanceE = 3,
                    DistanceSE = 4,
                    DistanceS = 5,
                    DistanceSW = 6,
                    DistanceW = 7,
                    DistanceNW = 8
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseExchangeInformationResponse()
        {
            //// Arrange
            //string jsonString = "{\"MessageId\":4,\"AgentId\":null}";

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(ExchangeInformationResponse));
        }

        [TestMethod]
        public void TestParseJoinGameResponse()
        {
            //// Arrange
            //string jsonString = "{\"MessageId\":107,\"AgentId\":null}";

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(JoinGameResponse));
        }

        [TestMethod]
        public void TestParseMoveResponse()
        {
            //// Arrange
            //string jsonString = "{\"MessageId\":108,\"AgentId\":null}";

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(MoveResponse));
        }

        [TestMethod]
        public void TestParsePickPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":109,\"AgentId\":null}";
            var expected = new Message<PickPieceResponse>()
            {
                MessagePayload = new PickPieceResponse()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePutPieceResponse()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":110,\"AgentId\":null}";
            var expected = new Message<PutPieceResponse>()
            {
                MessagePayload = new PutPieceResponse()
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseMoveError()
        {
            // Arrange
            string jsonString = "{\"MessagePayload\":{},\"MessageId\":901,\"AgentId\":null}";
            var expected = new Message<MoveError>()
            {
                MessagePayload = new MoveError()
                {
                    Position = (10, 20)
                }
            };
            var x = parser.AsString(expected);

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            Assert.IsInstanceOfType(result, typeof(MoveError));
        }

        [TestMethod]
        public void TestParsePickPieceError()
        {
            //// Arrange
            //string jsonString = "{\"MessageId\":902,\"AgentId\":null}";

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(PickPieceError));
        }

        [TestMethod]
        public void TestParsePutPieceError()
        {
            //// Arrange
            //string jsonString = "{\"MessageId\":903,\"AgentId\":null}";

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(PutPieceError));
        }

        [TestMethod]
        public void TestParseNotDefinedError()
        {
            //// Arrange
            //string jsonString = "{\"MessageId\":905,\"AgentId\":null}";

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            //Assert.IsInstanceOfType(result, typeof(NotDefinedError));
        }

        [TestMethod]
        public void TestParseMessageNotFound()
        {
            //// Arrange
            //string jsonString = "\"MessagePayload\":{\"Direction\":\"N\"},\"MessageId\":7,\"AgentId\":null}";
            ////var expected = new Message<>

            //// Act
            //var result = parser.Parse(jsonString);

            //// Assert
            ////Assert.IsInstanceOfType(result.MessageId, typeof(NotDefinedError));
        }

        [TestMethod]
        public void TestGameStarted()
        {
            // Arrange
            string expected = "{\"MessagePayload\":{\"AgentId\":0,\"AlliesIds\":[3,5,7],\"LeaderId\":0,\"EnemiesIds\":null,\"TeamId\":null,\"BoardSize\":null,\"GoalAreaSize\":0,\"NumberOfPlayers\":null,\"NumberOfPieces\":0,\"NumberOfGoals\":0,\"Penalties\":{\"Move\":null,\"CheckForSham\":\"check\",\"Discovery\":null,\"DestroyPiece\":\"xd\",\"PutPiece\":null,\"InformationExchange\":null},\"ShamPieceProbability\":0,\"Position\":{\"X\":10,\"Y\":20}},\"MessageId\":105,\"AgentId\":null}";
            Message<GameStarted> message = new Message<GameStarted>()
            {
                MessagePayload = new GameStarted()
                {
                    Penalties = new Penalties()
                    {
                        CheckForSham = "check",
                        DestroyPiece = "xd"
                    },
                    Position = new Position()
                    {
                        X = 10,
                        Y = 20
                    },
                    AlliesIds = new List<int>() { 3, 5, 7 }
                }
            };

            // Act
            var result = parser.AsString(message);

            //Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestPenaltyNotWaitedError()
        {
            // Arrange
            string expected = "{\"MessagePayload\":{\"WaitUntill\":\"2020-03-19T11:50:55.5\"},\"MessageId\":904,\"AgentId\":null}";
            Message<PenaltyNotWaitedError> message = new Message<PenaltyNotWaitedError>()
            {
                MessagePayload = new PenaltyNotWaitedError()
                {
                    WaitUntill = new DateTime(2020, 3, 19, 11, 50, 55, 500)
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
