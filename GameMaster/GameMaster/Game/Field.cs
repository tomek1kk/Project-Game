using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public class Field : AbstractField
    {
        public Field(int _x, int _y) : base(_x, _y)
        {
        }

        public override FieldType GetFieldTypeForGUI()
        {
            if (discovered)
                return FieldType.DiscoveredNonGoal;
            else if (pieces.Count > 0)
                return FieldType.Piece;
            else
                return FieldType.Empty;
        }

        public override void PickUp(Player player)
        {
            //TODO
            throw new NotImplementedException();
        }

        public override bool Put(AbstractPiece piece)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
