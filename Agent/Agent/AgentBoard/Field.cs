using System;
namespace Agent.AgentBoard
{
    public class Field
    {
        public Field()
        {
            fieldTaken = false;
            distToPiece = int.MaxValue;
            visited = false;
        }
        public bool visited { get; set; }
        public bool fieldTaken { get; set; }
        public int distToPiece { get; set; }
    }

}
