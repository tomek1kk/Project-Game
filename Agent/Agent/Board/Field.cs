using System;
namespace Agent.Board
{
    public enum GoalInfo
    {
        IDK,
        DiscoveredNotGoal,
        DiscoveredGoal
    }
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

        public GoalInfo goalInfo { get; set; } = GoalInfo.IDK;

        public Field()
        {
            DistToPiece = int.MaxValue;
        }
    }
}
