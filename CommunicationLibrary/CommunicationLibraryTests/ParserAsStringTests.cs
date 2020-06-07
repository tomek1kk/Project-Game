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
    public class ParserAsStringTests
    {
        IParser parser = new Parser();

        [TestMethod]
        public void TestParseCheckHoldedPieceRequest()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":1}";
            var message = new Message<CheckHoldedPieceRequest>()
            {
                MessagePayload = new CheckHoldedPieceRequest()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseDestroyPieceRequest()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":2}";
            var message = new Message<DestroyPieceRequest>()
            {
                MessagePayload = new DestroyPieceRequest()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseDiscoveryRequest()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":3}";
            var message = new Message<DiscoveryRequest>()
            {
                MessagePayload = new DiscoveryRequest()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseExachangeInformationRequest()
        {
            // Arrange
            string expected = "{\"payload\":{\"AskedAgentId\":1},\"messageID\":5}";
            var message = new Message<ExchangeInformationRequest>()
            {
                MessagePayload = new ExchangeInformationRequest()
                {
                    AskedAgentId = 1
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseJoinGameRequest()
        {
            // Arrange
            string expected = "{\"payload\":{\"teamId\":\"blue\"},\"messageID\":6}";
            var message = new Message<JoinGameRequest>()
            {
                MessagePayload = new JoinGameRequest()
                {
                    TeamId = "blue"
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
        [TestMethod]
        public void TestParseMoveRequest()
        {
            // Arrange
            string expected = "{\"payload\":{\"direction\":\"N\"},\"messageID\":7}";
            var message = new Message<MoveRequest>()
            {
                MessagePayload = new MoveRequest()
                {
                    Direction = "N"
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePickPieceRequest()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":8}";
            var message = new Message<PickPieceRequest>()
            {
                MessagePayload = new PickPieceRequest()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePutPieceRequest()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":9}";
            var message = new Message<PutPieceRequest>()
            {
                MessagePayload = new PutPieceRequest()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseCheckHoldedPieceResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"sham\":true},\"messageID\":101}";
            var message = new Message<CheckHoldedPieceResponse>()
            {
                MessagePayload = new CheckHoldedPieceResponse()
                {
                    Sham = true
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseDestroyPieceResponse()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":102}";
            var message = new Message<DestroyPieceResponse>()
            {
                MessagePayload = new DestroyPieceResponse()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseDiscoveryResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"distanceFromCurrent\":3,\"distanceN\":1,\"distanceNE\":2,\"distanceE\":3,\"distanceSE\":4,\"distanceS\":5,\"distanceSW\":6,\"distanceW\":7,\"distanceNW\":8},\"messageID\":103}";
            var message = new Message<DiscoveryResponse>()
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
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseExchangeInformationResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"respondToID\":10,\"distances\":[10,20,30,50],\"redTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"blueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"messageID\":4}";
            var message = new Message<ExchangeInformationResponse>()
            {
                MessagePayload = new ExchangeInformationResponse()
                {
                    BlueTeamGoalAreaInformations = new List<string>() { "i1", "i2" },
                    RedTeamGoalAreaInformations = new List<string>() { "r1", "r2 " },
                    RespondToID = 10,
                    Distances = new List<int>() { 10, 20, 30, 50 }
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseExchangeInformationGMResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"respondToID\":10,\"distances\":[10,20,30,50],\"redTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"blueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"messageID\":111}";
            var message = new Message<ExchangeInformationGMResponse>()
            {
                MessagePayload = new ExchangeInformationGMResponse()
                {
                    BlueTeamGoalAreaInformations = new List<string>() { "i1", "i2" },
                    RedTeamGoalAreaInformations = new List<string>() { "r1", "r2 " },
                    RespondToID = 10,
                    Distances = new List<int>() { 10, 20, 30, 50 }
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseJoinGameResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"accepted\":true,\"agentID\":10},\"messageID\":107}";
            var message = new Message<JoinGameResponse>()
            {
                MessagePayload = new JoinGameResponse()
                {
                    Accepted = true,
                    AgentID = 10
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseMoveResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"madeMove\":true,\"currentPosition\":{\"x\":3,\"y\":2},\"closestPiece\":5},\"messageID\":108}";

            var message = new Message<MoveResponse>()
            {
                MessagePayload = new MoveResponse()
                {
                    ClosestPiece = 5,
                    CurrentPosition = new Position()
                    {
                        X = 3,
                        Y = 2
                    },
                    MadeMove = true
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePickPieceResponse()
        {
            // Arrange
            string expected = "{\"payload\":{},\"messageID\":109}";
            var message = new Message<PickPieceResponse>()
            {
                MessagePayload = new PickPieceResponse()
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePutPieceResponse()
        {
            // Arrange
            string expected = "{\"payload\":{\"PutResult\":\"TaskField\"},\"messageID\":110}";
            var message = new Message<PutPieceResponse>()
            {
                MessagePayload = new PutPieceResponse() 
                {
                   PutResult = PutResultEnum.TaskField
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseMoveError()
        {
            // Arrange
            string expected = "{\"payload\":{\"position\":{\"x\":10,\"y\":20}},\"messageID\":901}";
            var message = new Message<MoveError>()
            {
                MessagePayload = new MoveError()
                {
                    Position = new Position()
                    {
                        X = 10,
                        Y = 20
                    }
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePickPieceError()
        {
            // Arrange
            string expected = "{\"payload\":{\"errorSubtype\":\"cannot pick here\"},\"messageID\":902}";
            var message = new Message<PickPieceError>()
            {
                MessagePayload = new PickPieceError()
                {
                    ErrorSubtype = "cannot pick here"
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePutPieceError()
        {
            // Arrange
            string expected = "{\"payload\":{\"ErrorSubtype\":\"cannot put here\"},\"messageID\":903,\"agentID\":5}";
            var message = new Message<PutPieceError>()
            {
                AgentId = 5,
                MessagePayload = new PutPieceError()
                {
                    ErrorSubtype = "cannot put here"
                }
            };
            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseNotDefinedError()
        {
            // Arrange
            string expected = "{\"payload\":{\"position\":{\"x\":2,\"y\":10},\"holdingPiece\":true},\"messageID\":905}";
            var message = new Message<NotDefinedError>()
            {
                MessagePayload = new NotDefinedError()
                {
                    HoldingPiece = true,
                    Position = new Position()
                    {
                        X = 2,
                        Y = 10
                    }
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseGameEnded()
        {
            // Arrange
            string expected = "{\"payload\":{\"winner\":\"blue\"},\"messageID\":104}";
            var message = new Message<GameEnded>()
            {
                MessagePayload = new GameEnded()
                {
                    Winner = "blue"
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseRedirectedExchangeInformationRequest()
        {
            // Arrange
            string expected = "{\"payload\":{\"askingId\":1,\"leader\":true,\"teamId\":\"blue\"},\"messageID\":106}";
            var message = new Message<RedirectedExchangeInformationRequest>()
            {
                MessagePayload = new RedirectedExchangeInformationRequest()
                {
                    TeamId = "blue",
                    AskingId = 1,
                    Leader = true,
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestAsStringGameStarted()
        {
            // Arrange
            string expected = "{\"payload\":{\"agentID\":0,\"alliesIDs\":[3,5,7],\"leaderID\":0,\"goalAreaSize\":0,\"numberOfPieces\":0,\"numberOfGoals\":0,\"penalties\":{\"checkForSham\":\"check\",\"destroyPiece\":\"xd\"},\"shamPieceProbability\":0,\"position\":{\"x\":10,\"y\":20}},\"messageID\":105}";
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
        public void TestAsStringPenaltyNotWaitedError()
        {
            // Arrange
            string expected = "{\"payload\":{\"waitFor\":200},\"messageID\":904}";
            Message<PenaltyNotWaitedError> message = new Message<PenaltyNotWaitedError>()
            {
                MessagePayload = new PenaltyNotWaitedError()
                {
                    WaitFor = 200
                }
            };

            // Act
            var result = parser.AsString(message);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
