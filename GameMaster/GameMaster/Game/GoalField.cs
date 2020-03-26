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

        public override FieldType GetFieldTypeForGUI()
        {
            if (_whos_here.Count > 0)
            {
                if (_whos_here[0].IsHolding)
                    return _whos_here[0].Team == Team.Red ? FieldType.RedPlayerWithPiece : FieldType.BluePlayerWithPiece;
                return _whos_here[0].Team == Team.Red ? FieldType.RedPlayer : FieldType.BluePlayer;
            }
            if (_discovered)
                return FieldType.DiscoveredGoal;
            else
                return FieldType.Goal;
        }

        public override AbstractPiece PickUp()
        {
            return null;
        }

        public override bool Put(AbstractPiece piece)
        {
            throw new NotImplementedException();
        }
    }
}
