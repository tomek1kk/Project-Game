using GameMaster.GUI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GameMaster.Game
{
    public class Field : AbstractField
    {
        public override FieldType GetFieldTypeForGUI()
        {
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
