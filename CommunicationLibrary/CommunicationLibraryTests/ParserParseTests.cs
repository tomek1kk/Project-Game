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
    public class ParserParseTests
    {
        IParser parser = new Parser();

        [TestMethod]
        public void TestParseCheckHoldedPieceRequest()
        {
            // Arrange
            string jsonString = "{\"payload\":{},\"messageID\":1,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{},\"messageID\":2,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{},\"messageID\":3,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{\"askedAgentID\":1},\"messageID\":5,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{\"teamID\":\"blue\"},\"messageID\":6,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{\"direction\":\"N\"},\"messageID\":7,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{},\"messageID\":8,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{},\"messageID\":9,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{\"sham\":true},\"messageID\":101,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{},\"messageID\":102,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{\"distanceFromCurrent\":3,\"distanceN\":1,\"distanceNE\":2,\"distanceE\":3,\"distanceSE\":4,\"distanceS\":5,\"distanceSW\":6,\"distanceW\":7,\"distanceNW\":8},\"messageID\":103,\"agentID\":null}";
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
            // Arrange
            string jsonString = "{\"payload\":{\"respondToID\":10,\"distances\":[10,20,30,50],\"redTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"blueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"messageID\":4,\"agentID\":null}";
            var expected = new Message<ExchangeInformationResponse>()
            {
                MessagePayload = new ExchangeInformationResponse()
                {
                    BlueTeamGoalAreaInformations = new List<string>() { "i1", "i2" },
                    RedTeamGoalAreaInformations = new List<string>() { "r1", "r2 "},
                    RespondToID = 10,
                    Distances = new List<int>() { 10, 20, 30, 50 }
                }
            };

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseExchangeInformationGMResponse()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"respondToID\":10,\"distances\":[10,20,30,50],\"redTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"blueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"messageID\":111,\"agentID\":null}";
            var expected = new Message<ExchangeInformationGMResponse>()
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
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseJoinGameResponse()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"accepted\":true,\"agentID\":10},\"messageID\":107,\"agentID\":null}";
            var expected = new Message<JoinGameResponse>()
            {
                MessagePayload = new JoinGameResponse()
                {
                    Accepted = true,
                    AgentID = 10
                }
            };

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseMoveResponse()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"madeMove\":true,\"currentPosition\":{\"x\":3,\"y\":2},\"closestPiece\":5},\"messageID\":108,\"agentID\":null}";

            var expected = new Message<MoveResponse>()
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
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePickPieceResponse()
        {
            // Arrange
            string jsonString = "{\"payload\":{},\"messageID\":109,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{},\"messageID\":110,\"agentID\":null}";
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
            string jsonString = "{\"payload\":{\"position\":{\"x\":10,\"y\":20}},\"messageID\":901,\"agentID\":null}";
            var expected = new Message<MoveError>()
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
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePickPieceError()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"errorSubtype\":\"cannot pick here\"},\"messageID\":902,\"agentID\":null}";
            var expected = new Message<PickPieceError>()
            {
                MessagePayload = new PickPieceError()
                {
                    ErrorSubtype = "cannot pick here"
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePutPieceError()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"errorSubtype\":\"cannot put here\"},\"messageID\":903,\"agentID\":5}";
            var expected = new Message<PutPieceError>()
            {
                AgentId = 5,
                MessagePayload = new PutPieceError()
                {
                    ErrorSubtype = "cannot put here"
                }
            };
            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseNotDefinedError()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"position\":{\"x\":2,\"y\":10},\"holdingPiece\":true},\"messageID\":905,\"agentID\":null}";
            var expected = new Message<NotDefinedError>()
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
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseGameStarted()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"agentID\":0,\"alliesIDs\":[3,5,7],\"leaderID\":0,\"enemiesIDs\":null,\"TeamID\":null,\"boardSize\":null,\"goalAreaSize\":0,\"numberOfPlayers\":null,\"numberOfPieces\":0,\"numberOfGoals\":0,\"penalties\":{\"move\":null,\"checkForSham\":\"check\",\"discovery\":null,\"destroyPiece\":\"xd\",\"putPiece\":null,\"informationExchange\":null},\"shamPieceProbability\":0,\"position\":{\"x\":10,\"y\":20}},\"messageID\":105,\"agentID\":null}";
            var expected = new Message<GameStarted>()
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
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParsePenaltyNotWaitedError()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"waitUntil\":\"2020-03-19T11:50:55.5\"},\"messageID\":904,\"agentID\":null}";
            var expected = new Message<PenaltyNotWaitedError>()
            {
                MessagePayload = new PenaltyNotWaitedError()
                {
                    WaitUntill = new DateTime(2020, 3, 19, 11, 50, 55, 500)
                }
            };

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseGameEnded()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"winner\":\"blue\"},\"messageID\":104,\"agentID\":null}";
            var expected = new Message<GameEnded>()
            {
                MessagePayload = new GameEnded()
                {
                    Winner = "blue"
                }
            };

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }

        [TestMethod]
        public void TestParseRedirectedExchangeInformationRequest()
        {
            // Arrange
            string jsonString = "{\"payload\":{\"teamID\":\"blue\",\"askingID\":1,\"leader\":true},\"messageID\":106,\"agentID\":null}";
            var expected = new Message<RedirectedExchangeInformationRequest>()
            {
                MessagePayload = new RedirectedExchangeInformationRequest()
                {
                    AskingId = 1,
                    Leader = true,
                    TeamId = "blue"
                }
            };

            // Act
            var result = parser.Parse(jsonString);

            // Assert
            result.Should().BeEquivalentTo(expected);
        }
    }
}
