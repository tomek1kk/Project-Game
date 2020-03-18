using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GameMaster.GUI;

namespace GameMaster.Game
{
    public abstract class AbstractField
    {
        private readonly int x;
        private readonly int y;
        private List<Player> whos_here;
        private List<AbstractPiece> pieces;

        public void LeavePlayer(Player player)
        {
            whos_here.Remove(player);
        }
        abstract public void PickUp(Player player);
        abstract public bool Put(AbstractPiece piece);
        public bool MoveHere(Player player)
        {
            whos_here.Add(player);
            return true;
        }
        public bool MoveOut(Player player)
        {
            if(whos_here.Contains(player))
            {
                whos_here.Remove(player);
                return true;
            }
            return false;
        }
        public bool ContainsPieces()
        {
            return pieces.Count > 0 ? true : false;
        }
        public int X
        {
            get => x;
        }
        public int Y
        {
            get => y;
        }
        abstract public FieldType GetFieldTypeForGUI();
    }
}
