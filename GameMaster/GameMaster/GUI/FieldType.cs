using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{

    public enum FieldType
    {
        Empty = 0,
        RedPlayer = 1,
        BluePlayer = 2,
        RedPlayerWithPiece = 3,
        BluePlayerWithPiece = 4,
        Piece = 5,
        Goal = 6,
        DiscoveredNonGoal = 7,
        DiscoveredGoal = 8,
        Sham = 9,
        RedPlayerWithSham = 10,
        BluePlayerWithSham = 11,
    }
}