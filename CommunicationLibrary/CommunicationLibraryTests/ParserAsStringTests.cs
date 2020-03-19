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
            string expected = "{\"MessagePayload\":{},\"MessageId\":1,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{},\"MessageId\":2,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{},\"MessageId\":3,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"AskedAgentId\":1},\"MessageId\":5,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"TeamId\":\"blue\"},\"MessageId\":6,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"Direction\":\"N\"},\"MessageId\":7,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{},\"MessageId\":8,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{},\"MessageId\":9,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"Sham\":true},\"MessageId\":101,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{},\"MessageId\":102,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"DistanceFromCurrent\":3,\"DistanceN\":1,\"DistanceNE\":2,\"DistanceE\":3,\"DistanceSE\":4,\"DistanceS\":5,\"DistanceSW\":6,\"DistanceW\":7,\"DistanceNW\":8},\"MessageId\":103,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"RespondToID\":10,\"Distances\":[10,20,30,50],\"RedTeamGoalAreaInformations\":[\"r1\",\"r2 \"],\"BlueTeamGoalAreaInformations\":[\"i1\",\"i2\"]},\"MessageId\":4,\"AgentId\":null}";
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
        public void TestParseJoinGameResponse()
        {
            // Arrange
            string expected = "{\"MessagePayload\":{\"Accepted\":true,\"AgentID\":10},\"MessageId\":107,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"MadeMove\":true,\"CurrentPosition\":{\"X\":3,\"Y\":2},\"ClosestPiece\":5},\"MessageId\":108,\"AgentId\":null}";

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
            string expected = "{\"MessagePayload\":{},\"MessageId\":109,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{},\"MessageId\":110,\"AgentId\":null}";
            var message = new Message<PutPieceResponse>()
            {
                MessagePayload = new PutPieceResponse()
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
            string expected = "{\"MessagePayload\":{\"Position\":{\"X\":10,\"Y\":20}},\"MessageId\":901,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"ErrorSubtype\":\"cannot pick here\"},\"MessageId\":902,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"ErrorSubtype\":\"cannot put here\"},\"MessageId\":903,\"AgentId\":5}";
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
            string expected = "{\"MessagePayload\":{\"Position\":{\"X\":2,\"Y\":10},\"HoldingPiece\":true},\"MessageId\":905,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"Winner\":\"blue\"},\"MessageId\":104,\"AgentId\":null}";
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
            string expected = "{\"MessagePayload\":{\"AskingId\":1,\"Leader\":true,\"TeamId\":\"blue\"},\"MessageId\":106,\"AgentId\":null}";
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
        public void TestAsStringPenaltyNotWaitedError()
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
