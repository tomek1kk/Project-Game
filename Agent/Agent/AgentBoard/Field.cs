using System;
namespace Agent.AgentBoard
{
    public class Field
    {
        public DateTime LastUpdateVisited { get; private set; }
        public DateTime LastUpdateFieldTaken { get; private set; }
        public DateTime LastUpdateDistToPiece { get; private set; }
        public bool Visited { get; private set; }
        public bool FieldTaken { get; private set; }
        public int DistToPiece { get; private set; }

        public Field()
        {
            FieldTaken = false;
            DistToPiece = int.MaxValue;
            Visited = false;
        }

        public void UpdateDistance(int? distanceToPiece)
        {
            DistToPiece = distanceToPiece.Value;
            LastUpdateDistToPiece = DateTime.Now;
        }
        public void UpdateFieldTaken(bool fieldTaken)
        {
            FieldTaken = fieldTaken;
            LastUpdateFieldTaken = DateTime.Now;
        }
        public void UpdateVisited(bool visited)
        {
            Visited = visited;
            LastUpdateVisited = DateTime.Now;
        }
    }
}
