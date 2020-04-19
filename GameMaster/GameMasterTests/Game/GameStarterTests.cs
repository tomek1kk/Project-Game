using CommunicationLibrary;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using FluentAssertions;
using GameMaster;
using GameMaster.Configuration;
using GameMaster.Game;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace GameMasterTests.Game
{
    [TestClass]
    public class GameStarterTests
    {
        class MockCommunicator : IMessageSenderReceiver
        {
            List<Message> messages = new List<Message>();
            public void Dispose()
            {
                return;
            }

            public void Send(Message m)
            {
                messages.Add(m);
            }

            public void StartReceiving(Action<Message> receiveCallback)
            {
                return;
            }

            public void StartReceiving(Action<Message> receiveCallback, Action<Exception> errorCallback)
            {
                return;
            }

            public List<Message> GetResult()
            {
                return messages;
            }
        }
        GMConfiguration configuration = new GMConfiguration()
        {

        };
        MockCommunicator communicator = new MockCommunicator();

        [TestMethod]
        public void TestStartGameManyPlayers()
        {
            // Arrange
            Dictionary<int, Player> players = new Dictionary<int, Player>()
            {
                { 1, new Player(Team.Blue, 1, false) { Position = new Field(10, 10) } },
                { 2, new Player(Team.Blue, 2, false) { Position = new Field(10, 10) } },
                { 3, new Player(Team.Red, 3, false) { Position = new Field(10, 10) } },
                { 4, new Player(Team.Red, 4, false) { Position = new Field(10, 10) } },
            };
            GameStarter gameStarter = new GameStarter(communicator, configuration);

            var player1MessageExpected = new Message<GameStarted>()
            {
                AgentId = 1,
                MessagePayload = new GameStarted()
                {
                    AgentId = 1,
                    AlliesIds = new List<int>() { 2 },
                    BoardSize = new BoardSize() { X = 0, Y = 0},
                    EnemiesIds = new List<int>() { 3, 4 },
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 1,
                        Enemies = 2
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Blue"
                }
            };

            var player2MessageExpected = new Message<GameStarted>()
            {
                AgentId = 2,
                MessagePayload = new GameStarted()
                {
                    AgentId = 2,
                    AlliesIds = new List<int>() { 1 },
                    BoardSize = new BoardSize() { X = 0, Y = 0 },
                    EnemiesIds = new List<int>() { 3, 4 },
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 1,
                        Enemies = 2
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Blue"
                }
            };

            var player3MessageExpected = new Message<GameStarted>()
            {
                AgentId = 3,
                MessagePayload = new GameStarted()
                {
                    AgentId = 3,
                    AlliesIds = new List<int>() { 4 },
                    BoardSize = new BoardSize() { X = 0, Y = 0 },
                    EnemiesIds = new List<int>() { 1, 2 },
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 1,
                        Enemies = 2
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Red"
                }
            };

            var player4MessageExpected = new Message<GameStarted>()
            {
                AgentId = 4,
                MessagePayload = new GameStarted()
                {
                    AgentId = 4,
                    AlliesIds = new List<int>() { 3 },
                    BoardSize = new BoardSize() { X = 0, Y = 0 },
                    EnemiesIds = new List<int>() { 1, 2 },
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 1,
                        Enemies = 2
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Red"
                }
            };


            // Act
            gameStarter.StartGame(players);
            var result = communicator.GetResult();
            var player1Message = result.Find(m => m.AgentId == 1);
            var player2Message = result.Find(m => m.AgentId == 2);
            var player3Message = result.Find(m => m.AgentId == 3);
            var player4Message = result.Find(m => m.AgentId == 4);


            // Assert
            player1Message.Should().BeEquivalentTo(player1MessageExpected);
            player2Message.Should().BeEquivalentTo(player2MessageExpected);
            player3Message.Should().BeEquivalentTo(player3MessageExpected);
            player4Message.Should().BeEquivalentTo(player4MessageExpected);
        }


        [TestMethod]
        public void TestStartGameOneTeam()
        {
            // Arrange
            Dictionary<int, Player> players = new Dictionary<int, Player>()
            {
                { 1, new Player(Team.Blue, 1, false) { Position = new Field(10, 10) } },
                { 2, new Player(Team.Blue, 2, false) { Position = new Field(10, 10) } }
            };
            GameStarter gameStarter = new GameStarter(communicator, configuration);

            var player1MessageExpected = new Message<GameStarted>()
            {
                AgentId = 1,
                MessagePayload = new GameStarted()
                {
                    AgentId = 1,
                    AlliesIds = new List<int>() { 2 },
                    BoardSize = new BoardSize() { X = 0, Y = 0 },
                    EnemiesIds = new List<int>(),
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 1,
                        Enemies = 0
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Blue"
                }
            };

            var player2MessageExpected = new Message<GameStarted>()
            {
                AgentId = 2,
                MessagePayload = new GameStarted()
                {
                    AgentId = 2,
                    AlliesIds = new List<int>() { 1 },
                    BoardSize = new BoardSize() { X = 0, Y = 0 },
                    EnemiesIds = new List<int>(),
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 1,
                        Enemies = 0
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Blue"
                }
            };
          
            // Act
            gameStarter.StartGame(players);
            var result = communicator.GetResult();
            var player1Message = result.Find(m => m.AgentId == 1);
            var player2Message = result.Find(m => m.AgentId == 2);


            // Assert
            player1Message.Should().BeEquivalentTo(player1MessageExpected);
            player2Message.Should().BeEquivalentTo(player2MessageExpected);
        }

        [TestMethod]
        public void TestStartGameOnePlayer()
        {
            // Arrange
            Dictionary<int, Player> players = new Dictionary<int, Player>()
            {
                { 1, new Player(Team.Blue, 1, false) { Position = new Field(10, 10) } }
            };
            GameStarter gameStarter = new GameStarter(communicator, configuration);

            var player1MessageExpected = new Message<GameStarted>()
            {
                AgentId = 1,
                MessagePayload = new GameStarted()
                {
                    AgentId = 1,
                    AlliesIds = new List<int>(),
                    BoardSize = new BoardSize() { X = 0, Y = 0 },
                    EnemiesIds = new List<int>(),
                    NumberOfPlayers = new NumberOfPlayers()
                    {
                        Allies = 0,
                        Enemies = 0
                    },
                    Penalties = new Penalties()
                    {
                        CheckForSham = "0",
                        DestroyPiece = "0",
                        Discovery = "0",
                        InformationExchange = "0",
                        Move = "0",
                        PutPiece = "0"
                    },
                    Position = new Position() { X = 10, Y = 10 },
                    TeamId = "Blue"
                }
            };

            // Act
            gameStarter.StartGame(players);
            var result = communicator.GetResult();
            var player1Message = result.Find(m => m.AgentId == 1);


            // Assert
            player1Message.Should().BeEquivalentTo(player1MessageExpected);
        }

        [TestMethod]
        public void TestStartGameNoPlayers()
        {
            // Arrange
            Dictionary<int, Player> players = new Dictionary<int, Player>();
            GameStarter gameStarter = new GameStarter(communicator, configuration);

            // Act
            gameStarter.StartGame(players);
            var result = communicator.GetResult();

            // Assert
            result.Should().BeEquivalentTo(new List<Player>());
        }


    }
}
