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
            if (whos_here.Count > 0)
            {
                if (whos_here[0].IsHolding)
                    return whos_here[0].Team == Team.Red ? FieldType.RedPlayerWithPiece : FieldType.BluePlayerWithPiece;
                return whos_here[0].Team == Team.Red ? FieldType.RedPlayer : FieldType.BluePlayer;
            }
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
