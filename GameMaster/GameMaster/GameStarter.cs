using CommunicationLibrary;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using GameMaster.Configuration;
using GameMaster.Game;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster
{
    public static class GameStarter
    {
        public static IMessageSenderReceiver Communicator
        {
            get; set;
        }
        public static GMConfiguration Configuration
        {
            get; set;
        }

        public static void StartGame(Dictionary<int, Player> players)
        {
            List<Message> messages = new List<Message>();
            foreach (var player in players)
            {
                var message = new Message<GameStarted>()
                {
                    AgentId = player.Key,
                    MessagePayload = new GameStarted()
                    {
                        AgentId = player.Key,
                        AlliesIds = players.Values.Where(p => p.Team == player.Value.Team && p != player.Value).Select(p => p.AgentId),
                        BoardSize = new BoardSize() { X = Configuration.BoardX, Y = Configuration.BoardY },
                        EnemiesIds = players.Values.Where(p => p.Team != player.Value.Team).Select(p => p.AgentId),
                        GoalAreaSize = Configuration.GoalAreaHight,
                        LeaderId = players.Values.Where(p => p.Team == player.Value.Team && p.IsLeader).Select(p => p.AgentId).First(),
                        NumberOfGoals = Configuration.NumberOfGoals,
                        NumberOfPieces = Configuration.NumberOfPieces,
                        NumberOfPlayers = new NumberOfPlayers()
                        {
                            Allies = players.Values.Where(p => p.Team == player.Value.Team && p != player.Value).Count(),
                            Enemies = players.Values.Where(p => p.Team != player.Value.Team).Count()
                        },
                        Penalties = new Penalties()
                        {
                            CheckForSham = Configuration.CheckForShamPenalty.ToString(),
                            DestroyPiece = Configuration.DestroyPiecePenalty.ToString(),
                            Discovery = Configuration.DiscoveryPenalty.ToString(),
                            InformationExchange = Configuration.AskPenalty.ToString(),
                            Move = Configuration.MovePenalty.ToString(),
                            PutPiece = Configuration.PutPenalty.ToString()
                        },
                        Position = new Position()
                        {
                            X = player.Value.X,
                            Y = player.Value.Y
                        },
                        ShamPieceProbability = 0.5, // TODO
                        TeamId = player.Value.Team.ToString()
                    }
                };
                messages.Add(message);
            }
            messages.ForEach(m => Communicator.Send(m));
        }
    }
}
