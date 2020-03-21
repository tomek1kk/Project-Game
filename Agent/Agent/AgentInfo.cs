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
            this.board = new Field[board.BoardSize.X,board.BoardSize.Y];
        }
        public (int, int) position;
        public bool isLeader;
        public bool hasPiece; 
        public Field[,] board;
        IStrategy strategy;
    }
}
