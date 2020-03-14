using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.GUI
{

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
