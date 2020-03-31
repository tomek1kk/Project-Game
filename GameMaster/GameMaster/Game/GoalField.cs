using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public class GoalField : AbstractField
    {
        public GoalField(int _x, int _y) : base(_x, _y)
        {
        }

        public override bool IsGoalField { get => true; }

        protected override FieldType GetGUIFieldFromField ()
        {
            return ContainsPieces() ? FieldType.DiscoveredGoal : FieldType.Goal;
        }

        public override AbstractPiece PickUp()
        {
            return null;
        }

        public override void Put(AbstractPiece piece)
        {
            _pieces.Add(piece);
        }
    }
}
