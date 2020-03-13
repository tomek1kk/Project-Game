using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.AspNet.Models
{
    public class BoardModel
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int GoalAreaHeight { get; set; }
        public FieldType[,] Fields { get; set; }
    }
    public enum FieldType
    {
        Empty,
        RedPlayer,
        BluePlayer,
        RedPlayerWithPiece,
        BluePlayerWithPiece,
        Piece,
        Goal,
        DiscoveredNonGoal,
        DiscoveredGoal,
    }
}
