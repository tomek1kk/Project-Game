using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public class Piece:AbstractPiece
    {
        public override bool IsSham()
        {
            return false;
        }
    }
}
