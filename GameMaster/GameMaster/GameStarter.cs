using CommunicationLibrary;
using CommunicationLibrary.Information;
using CommunicationLibrary.Model;
using GameMaster.Configuration;
using GameMaster.Game;
using System.Collections.Generic;
using System.Linq;

namespace GameMaster
{
    public class GameStarter
    {
        private IMessageSenderReceiver _communicator;
        private GMConfiguration _configuration;

        public GameStarter(IMessageSenderReceiver communicator, GMConfiguration configuration)
        {
            _communicator = communicator;
            _configuration = configuration;
        }

        public void StartGame(Dictionary<int, Player> players)
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
                        AlliesIds = players.Values.Where(p => p.Team == player.Value.Team && p != player.Value).Select(p => p.AgentId).ToList(),
                        BoardSize = new BoardSize() { X = _configuration.BoardX, Y = _configuration.BoardY },
                        EnemiesIds = players.Values.Where(p => p.Team != player.Value.Team).Select(p => p.AgentId).ToList(),
                        GoalAreaSize = _configuration.GoalAreaHeight,
                        LeaderId = players.Values.Where(p => p.Team == player.Value.Team && p.IsLeader).Select(p => p.AgentId).FirstOrDefault(),
                        NumberOfGoals = _configuration.NumberOfGoals,
                        NumberOfPieces = _configuration.NumberOfPieces,
                        NumberOfPlayers = new NumberOfPlayers()
                        {
                            Allies = players.Values.Where(p => p.Team == player.Value.Team && p != player.Value).Count(),
                            Enemies = players.Values.Where(p => p.Team != player.Value.Team).Count()
                        },
                        Penalties = new Penalties()
                        {
                            CheckForSham = _configuration.CheckForShamPenalty.ToString(),
                            DestroyPiece = _configuration.DestroyPiecePenalty.ToString(),
                            Discovery = _configuration.DiscoveryPenalty.ToString(),
                            InformationExchange = _configuration.InformationExchangePenalty.ToString(),
                            Move = _configuration.MovePenalty.ToString(),
                            PutPiece = _configuration.PutPenalty.ToString()
                        },
                        Position = new Position()
                        {
                            X = player.Value.X,
                            Y = player.Value.Y
                        },
                        ShamPieceProbability = _configuration.ShamPieceProbability,
                        TeamId = player.Value.Team.AsString()
                    }
                };
                messages.Add(message);
            }
            messages.ForEach(m => _communicator.Send(m));
        }
    }
}
