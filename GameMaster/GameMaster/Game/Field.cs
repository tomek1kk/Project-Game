using GameMaster.GUI;
using System.Linq;

namespace GameMaster.Game
{
    public class Field : AbstractField
    {
        public Field(int x, int y) : base(x, y)
        {
        }

        public override bool IsGoalField { get => false; }

        protected override FieldType GetGUIFieldFromField()
        {
            if (ContainsPieces())
            {
                if (_pieces[0].IsSham())
                {
                    return FieldType.Sham;
                }
                else
                {
                    return _discovered ? FieldType.DiscoveredNonGoal : FieldType.Piece;
                }
            }
            else
            {
                return FieldType.Empty;
            }
        }
        public override AbstractPiece PickUp()
        {
            AbstractPiece piece = _pieces.Last();
            _pieces.RemoveAt(_pieces.Count - 1);
            return piece;
        }

        public override void Put(AbstractPiece piece)
        {
            _pieces.Add(piece);
        }
    }
}
