using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public class Field : AbstractField
    {
        public Field(int x, int y) : base(x, y)
        {
        }

        public override FieldType GetFieldTypeForGUI()
        {
            if (_whos_here.Count > 0)
            {
                if (_whos_here[0].IsHolding)
                    return _whos_here[0].Team == Team.Red ? FieldType.RedPlayerWithPiece : FieldType.BluePlayerWithPiece;
                return _whos_here[0].Team == Team.Red ? FieldType.RedPlayer : FieldType.BluePlayer;
            }
            if (_discovered)
                return FieldType.DiscoveredNonGoal;
            else if (_pieces.Count > 0)
                return FieldType.Piece;
            else
                return FieldType.Empty;
        }

        public override AbstractPiece PickUp()
        {
            AbstractPiece piece = _pieces.Last();
            _pieces.RemoveAt(_pieces.Count - 1);
            return piece;
        }

        public override bool Put(AbstractPiece piece)
        {
            //TODO
            throw new NotImplementedException();
        }
    }
}
