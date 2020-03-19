using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Information
{
    public class GameStarted : MessagePayload
    {
        public int AgentId { get; set; }
        public IEnumerable<int> AlliesIds { get; set; }
        public int LeaderId { get; set; }
        public IEnumerable<int> EnemiesIds { get; set; }
        public string TeamId { get; set; }
        public BoardSize BoardSize { get; set; }
        public int GoalAreaSize { get; set; }
        public NumberOfPlayers NumberOfPlayers { get; set; }
        public int NumberOfPieces { get; set; }
        public int NumberOfGoals { get; set; }
        public Penalties Penalties { get; set; }
        public double ShamPieceProbability { get; set; }
        public Position Position { get; set; }

        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
