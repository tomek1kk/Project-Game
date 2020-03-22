using System;
using CommunicationLibrary.Model;

namespace Agent.AgentBoard
{
    public class AgentBoardInfo
    {
        public BoardSize BoardSize { get; set; }
        public int GoalAreaSize { get; set; }
        public int NumberOfPieces { get; set; }
        public int NumberOfGoals { get; set; }
        public Penalties Penalties { get; set; }
        public double ShamPieceProbability { get; set; }
    }
}
