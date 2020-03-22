using System;
using Agent.AgentBoard;

namespace Agent
{
    public class AgentInfo
    {
        public AgentInfo(IStrategy strategy, bool isLeader, AgentBoardInfo board, (int, int) position)
        {
            this.position = position;
            this.strategy = strategy;
            this.hasPiece = false;
            this.isLeader = isLeader;
            this.board = new Field[(int)board.BoardSize.X, (int)board.BoardSize.Y];
        }
        public (int, int) position;
        public bool isLeader;
        public bool hasPiece; 
        IStrategy strategy;
    }
}
