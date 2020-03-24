using System;
namespace Agent.AgentBoard
{
    public class Field
    {
        private bool visited;
        public DateTime LastUpdateVisited { get; private set; }
        public bool Visited
        {
            get => visited;
            set
            {
                visited = value;
                LastUpdateVisited = DateTime.Now;
            }
        }

        private bool fieldTaken;
        public DateTime LastUpdateFieldTaken { get; private set; }
        public bool FieldTaken
        {
            get => fieldTaken;
            set
            {
                fieldTaken = value;
                LastUpdateFieldTaken = DateTime.Now;
            }
        }

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

        public Field()
        {
            FieldTaken = false;
            DistToPiece = int.MaxValue;
            Visited = false;
        }
    }
}
