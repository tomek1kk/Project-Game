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
            // Arrange
            string jsonString = "{\"MessagePayload\":{\"RespondToID\":10,\"Distances\":[10,20,30,50],\"RedTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"BlueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"MessageId\":4,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"RespondToID\":10,\"Distances\":[10,20,30,50],\"RedTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"BlueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"MessageId\":111,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"Accepted\":true,\"AgentID\":10},\"MessageId\":107,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"MadeMove\":true,\"CurrentPosition\":{\"X\":3,\"Y\":2},\"ClosestPiece\":5},\"MessageId\":108,\"AgentId\":null}";

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
            string jsonString = "{\"MessagePayload\":{\"Position\":{\"X\":10,\"Y\":20}},\"MessageId\":901,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"ErrorSubtype\":\"cannot pick here\"},\"MessageId\":902,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"ErrorSubtype\":\"cannot put here\"},\"MessageId\":903,\"AgentId\":5}";
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
            string jsonString = "{\"MessagePayload\":{\"Position\":{\"X\":2,\"Y\":10},\"HoldingPiece\":true},\"MessageId\":905,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"AgentId\":0,\"AlliesIds\":[3,5,7],\"LeaderId\":0,\"EnemiesIds\":null,\"TeamId\":null,\"BoardSize\":null,\"GoalAreaSize\":0,\"NumberOfPlayers\":null,\"NumberOfPieces\":0,\"NumberOfGoals\":0,\"Penalties\":{\"Move\":null,\"CheckForSham\":\"check\",\"Discovery\":null,\"DestroyPiece\":\"xd\",\"PutPiece\":null,\"InformationExchange\":null},\"ShamPieceProbability\":0,\"Position\":{\"X\":10,\"Y\":20}},\"MessageId\":105,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"WaitUntill\":\"2020-03-19T11:50:55.5\"},\"MessageId\":904,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"Winner\":\"blue\"},\"MessageId\":104,\"AgentId\":null}";
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
            string jsonString = "{\"MessagePayload\":{\"TeamId\":\"blue\",\"AskingId\":1,\"Leader\":true},\"MessageId\":106,\"AgentId\":null}";
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
