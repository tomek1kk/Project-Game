using System;
namespace Agent.AgentBoard
{
    public class Field
    {
        private int distToPiece;
        public DateTime LastUpdateDistToPiece { get; private set; }
        public int DistToPiece
        {
            get => distToPiece;
            set
            {
                distToPiece = value;
                LastUpdateDistToPiece = DateTime.Now;
            }
        }

        public bool IsDiscoveredGoal { get; set; } = false;

        public Field()
        {
            DistToPiece = int.MaxValue;
        }
    }
}
