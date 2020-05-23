using CommunicationLibrary.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace CommunicationLibrary.Information
{
    public class GameStarted : MessagePayload
    {
        [System.Text.Json.Serialization.JsonPropertyName("agentID")]
        public int AgentId { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("alliesIDs")]
        public IEnumerable<int> AlliesIds { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("leaderID")]
        public int LeaderId { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("enemiesIDs")]
        public IEnumerable<int> EnemiesIds { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("teamID")]
        public string TeamId { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("boardSize")]
        public BoardSize BoardSize { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("goalAreaSize")]
        public int GoalAreaSize { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("numberOfPlayers")]
        public NumberOfPlayers NumberOfPlayers { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("numberOfPieces")]
        public int NumberOfPieces { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("numberOfGoals")]
        public int NumberOfGoals { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("penalties")]
        public Penalties Penalties { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("shamPieceProbability")]
        public double ShamPieceProbability { get; set; }
        [System.Text.Json.Serialization.JsonPropertyName("position")]
        public Position Position { get; set; }

        public override bool ValidateMessage()
        {
            return true;
        }
    }
}
