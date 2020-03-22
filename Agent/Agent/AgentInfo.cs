using System;
using Agent.AgentBoard;
using CommunicationLibrary;
using CommunicationLibrary.Information;

namespace Agent
{
    public class AgentInfo
    {
        public AgentInfo(IStrategy strategy, bool isLeader, (int, int) position)
        {
            Position = position;
            Strategy = strategy;
            HasPiece = false;
            IsLeader = isLeader;
        }
        public (int, int) Position { get; set; }
        public bool IsLeader { get; set; }
        public bool HasPiece { get; set; }
        public IStrategy Strategy { get; set; }
        public GameStarted GameStartedMessage { get; set; }

        public void UpdateFromMessage(Message received)
        {
            //todo updating based on responses and game start message
        }
    }
}
