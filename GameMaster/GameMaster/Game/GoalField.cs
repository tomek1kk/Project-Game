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
            if (discovered)
                return FieldType.DiscoveredGoal;
            else
                return FieldType.Goal;
        }

        public override void PickUp(Player player)
        {
            throw new NotImplementedException();
        }

        public override bool Put(AbstractPiece piece)
        {
            throw new NotImplementedException();
        }
    }
}
